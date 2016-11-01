using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;


namespace UntoterOstgote.Martus.Ocr.Postprocessing
{
    public class SpellChecker
    {
        private Dictionary<string, int> NWORDS;
        private static readonly string alphabet = @"abcdefghijklmnopqrstuvwxyz";
        private static readonly char[] alphabetArray = alphabet.ToArray();
        private static string regExStrWordsEN = @"[a-z]+";
        private static Regex regExCompWordsEN = new Regex(regExStrWordsEN, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string regExStrWordsDE = @"[a-zäöüß]+";
        private static Regex regExCompWordsDE = new Regex(regExStrWordsDE, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public SpellChecker()
        {
            NWORDS = new Dictionary<string, int>(1000000);
        }

        public void Train(string trainPath, bool createNew)
        {
            if (createNew)
            {
                using (StreamReader reader = new StreamReader(trainPath))
                {
                    int lineCounter = 0;
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineCounter++;
                        MatchCollection matches = regExCompWordsDE.Matches(line);
                        foreach (Match match in matches)
                        {
                            string word = match.Value.ToLower();
                            int wordCount;
                            if (NWORDS.TryGetValue(word, out wordCount))
                            {
                                NWORDS[word] = ++wordCount;
                            }
                            else
                            {
                                NWORDS.Add(word, 1);
                            }
                        }

                        if (lineCounter % 100000 == 0)
                        {
                            Console.WriteLine("Line: " + lineCounter);
                        }    
                    }

                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream("NWORDS.bin", FileMode.Create))
                    {
                        formatter.Serialize(stream, NWORDS);
                    }
                }
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream("NWORDS.bin", FileMode.Open))
                {
                    this.NWORDS = (Dictionary<string, int>) formatter.Deserialize(stream);
                }
            }

            //string trainText = File.ReadAllText(trainPath, Encoding.UTF8);
            //MatchCollection mColl = regExCompWordsDE.Matches(trainText);
            //foreach (Match match in mColl)
            //{
            //    string word = match.Value.ToLower();
            //    int counts;
            //    if (NWORDS.TryGetValue(word, out counts))
            //        NWORDS[word] = ++counts;
            //    else
            //        NWORDS.Add(word, 1);
            //}
        }

        private HashSet<string> Edits1(string word)
        {
            //List<Tuple<string, string>> splitsList = new List<Tuple<string, string>>();
            List<Utils.MyTupleStr> splitsList = new List<Utils.MyTupleStr>();

            List<string> deletesList = new List<string>();
            List<string> transposesList = new List<string>();
            List<string> replacesList = new List<string>();
            List<string> insertsList = new List<string>();

            // Splits
            // splits = [(word[:i], word[i:]) for i in range(len(word) + 1)]
            for (int i = 0; i < word.Length + 1; i++)
            {
                //splitsList.Add(new Tuple<string, string>(word.Substring(0, i), word.Substring(i)));
                splitsList.Add(new Utils.MyTupleStr(word.Substring(0, i), word.Substring(i)));
            }

            // Deletes
            // deletes = [a + b[1:] for a, b in splits if b]
            foreach (var tuple in splitsList)
            {
                string a = tuple.Item1;
                string b = tuple.Item2;
                if (b != "")
                    deletesList.Add(a + b.Substring(1));
            }

            // Transposes
            // transposes = [a + b[1] + b[0] + b[2:] for a, b in splits if len(b) > 1]
            foreach (var tuple in splitsList)
            {
                string a = tuple.Item1;
                string b = tuple.Item2;
                if (b.Length > 1)
                    transposesList.Add($"{a}{b[1]}{b[0]}{b.Substring(2)}");  // Note: Test the substring of a index that doesn't exists.                
            }

            // Replaces
            // replaces = [a + c + b[1:] for a, b in splits for c in alphabet if b]
            foreach (var tuple in splitsList)
            {
                string a = tuple.Item1;
                string b = tuple.Item2;
                if (b != "")
                {
                    foreach (var c in alphabetArray)
                        replacesList.Add($"{a}{c}{b.Substring(1)}");  // Note: Test the substring of a index that doesn't exists.
                }
            }

            // Inserts
            // inserts = [a + c + b     for a, b in splits for c in alphabet]
            foreach (var tuple in splitsList)
            {
                string a = tuple.Item1;
                string b = tuple.Item2;
                foreach (var c in alphabetArray)
                    insertsList.Add($"{a}{c}{b}");
            }

            int capacity = deletesList.Count + transposesList.Count + replacesList.Count + insertsList.Count;
            deletesList.Capacity = capacity;
            deletesList.AddRange(transposesList);
            deletesList.AddRange(replacesList);
            deletesList.AddRange(insertsList);

            HashSet<string> set = new HashSet<string>(deletesList);
            return set;
        }

        private HashSet<String> KnownEdits2(string word)
        {
            HashSet<string> set = new HashSet<string>();
            // set(e2 for e1 in edits1(word) for e2 in edits1(e1) if e2 in NWORDS)
            foreach (var e1 in Edits1(word))
            {
                foreach (var e2 in Edits1(e1))
                {
                    if (NWORDS.ContainsKey(e2))
                        set.Add(e2);
                }
            }
            return set;
        }

        private HashSet<String> Known(IEnumerable<string> words)
        {
            HashSet<string> set = new HashSet<string>();
            // set(w for w in words if w in NWORDS)
            foreach (var w in words)
            {
                if (NWORDS.ContainsKey(w))
                    set.Add(w);
            }
            return set;
        }

        // def correct(word):
        //    candidates = known([word]) or known(edits1(word)) or known_edits2(word) or[word]
        //    return max(candidates, key= NWORDS.get)
        public string Correct(string word)
        {
            word = word.ToLower();
            HashSet<string> set = new HashSet<string>();
            set = Known(new List<string> { word });
            if (set.Count == 0)
            {
                set = Known(Edits1(word));
                if (set.Count == 0)
                {
                    set = KnownEdits2(word);
                    if (set.Count == 0)
                        set.Add(word);
                }
            }

            int maxCount = 0;
            string maxWord = word;
            foreach (var w in set)
            {
                int count;
                if (NWORDS.TryGetValue(w, out count))
                {
                    if (count > maxCount)
                    {
                        maxCount = count;
                        maxWord = w;
                    }
                }
            }
            return maxWord;
        }

        public bool DataOnWord(string word, out int counts)
        {
            if (NWORDS.TryGetValue(word.ToLower(), out counts))
                return true;
            else
                return false;
        }
    }
}

