using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.Ocr.Postprocessing
{
    public class TestSpellChecker
    {
        public static void RunTests()
        {
            string trainFilePath = @"big.txt";
            SpellChecker sp = new SpellChecker();
            sp.Train(trainFilePath, true);

            TestWord(sp, "Helo", "Hello");

            int counts = 0;
            string word = "Hello";
            if (sp.DataOnWord(word, out counts))
                Console.WriteLine($"word: {word} counts: {counts}");

            word = "felo";
            if (sp.DataOnWord(word, out counts))
                Console.WriteLine($"word: {word} counts: {counts}");

            //>>> correct('speling')
            //    'spelling'
            //>>> correct('korrecter')
            //    'corrector'

            TestWord(sp, "speling", "spelling");
            TestWord(sp, "korrecter", "corrector");  // Corrector doesn't exists in the big.txt file.

            /*
            correct('economtric') => 'economic'(121); expected 'econometric'(1)
            correct('embaras') => 'embargo'(8); expected 'embarrass'(1)
            correct('colate') => 'coat'(173); expected 'collate'(1)
            correct('orentated') => 'orentated'(1); expected 'orientated'(1)
            correct('unequivocaly') => 'unequivocal'(2); expected 'unequivocally'(1)
            correct('generataed') => 'generate'(2); expected 'generated'(1)
            correct('guidlines') => 'guideline'(2); expected 'guidelines'(1)
            */

            TestWord(sp, "economtric", "economic");
            TestWord(sp, "embaras", "embargo");
            TestWord(sp, "colate", "coat");
            TestWord(sp, "orentated", "orentated");
            TestWord(sp, "unequivocaly", "unequivocal");
            TestWord(sp, "generataed", "generate");
            TestWord(sp, "guidlines", "guideline");
        }

        private static void TestWord(SpellChecker sp, string word, string expected)
        {
            Console.WriteLine($"correct('{word}') => '{sp.Correct(word)}'; expected '{expected}'");
        }
    }
}