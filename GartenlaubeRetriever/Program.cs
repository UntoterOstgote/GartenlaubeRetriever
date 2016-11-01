using LinqToWiki;
using LinqToWiki.Generated;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntoterOstgote.Martus.Ocr.Postprocessing;

using CommonMark;
using UntoterOstgote.Martus.GartenlaubeKorpus.Services;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    class Program
    {
        private const string fullFilename = "Gartenlaube_1853_99_komplett.txt";
        private const string level4Filename = "Gartenlaube_1853_99_level4.txt";
        private const string testFileName = "test.txt";

        static void Main(string[] args)
        {
            //DownloadGartenlaubeFromWikisource();

            TestDownload();

            //Parsing out Level 4

            CommonMarkSettings.Default.OutputDelegate =
                (doc, output, settings) =>
                new GartenlaubeFormatter(output, settings).WriteDocument(doc);

            using (var reader = new System.IO.StreamReader(testFileName))
            {
                var html = CommonMarkConverter.Convert(reader.ReadToEnd());
                //Console.WriteLine(html);

                //using (StreamWriter streamWriter = new StreamWriter("test.html"))
                //{
                //    streamWriter.Write(html);
                //}
                //var document = CommonMark.CommonMarkConverter.Parse(reader);
                //foreach (var node in document.AsEnumerable())
                //{
                //    if (node.IsOpening && node.Inline != null)
                //    {
                //        Console.WriteLine(node.Inline.Tag.ToString());
                //    }
                //}
            }
            //using (var writer = new System.IO.StreamWriter("result.html"))
            //{
            //    CommonMark.CommonMarkConverter.Convert(reader, writer);
            //}




            // index location
            var indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Index");

            List<Text> texts = TextRepository.GetTexts();
            var writer = new TextWriter(indexPath);

            writer.AddUpdateTextToIndex(texts);

            var searcher = new TextSearcher(indexPath);


            Console.WriteLine("Search for Arne Janning");
            var res = searcher.SearchText("Arne", "Author");

            foreach (var item in res.SearchResultItems)
            {
                Console.WriteLine("Result with ID: {0}", item.Id);
                Console.WriteLine(texts.First(t => t.Id == item.Id).FullText);
            }


            SpellChecker checker = new SpellChecker();
            //checker.Train(level4Filename);
            checker.Train(level4Filename, true);

            int wordCount = 0;

            if (checker.DataOnWord("Geheimniß", out wordCount))
            {
                Console.WriteLine("Geheimniß: " + wordCount +  " mal.");
                Console.WriteLine(checker.Correct("Gekeimniß"));

            }
    

            Console.WriteLine("Ready!");
            Console.ReadLine();
        }

        private static void TestDownload()
        {

            GartenlaubeService service = new GartenlaubeService(fullFilename);
            service.Run();

            //var wikisource = new Wiki("MartusBot/1.0, janningarne@gmail.com ", "https://de.wikisource.org");

            ////var result = (from pages in wikisource.Query.allpages()
            ////              where pages.prefix == "Die Gartenlaube (1853) 533"
            ////              select pages).ToEnumerable().First();
            //PagesSource<Page> page = wikisource.CreateTitlesSource("Seite:Die Gartenlaube (1853) 533.jpg");
            //var test = page.Select(
            //    p =>
            //    new
            //    {
            //        Text = p.revisions()
            //        .Select(revision => revision.value)
            //        .FirstOrDefault()
            //    }).ToEnumerable();

            //foreach (var item in test)
            //{
            //    Console.WriteLine(item.Text);
            //}

        }

        private static void DownloadGartenlaubeFromWikisource()
        {
            using (StreamWriter writer = new StreamWriter(fullFilename))
            {
                var wikisource = new Wiki("MartusBot/1.0, janningarne@gmail.com ", "https://de.wikisource.org");
                var categoryPages = (from members in wikisource.Query.categorymembers()
                             where members.title == "Kategorie:Die Gartenlaube"
                             select members.title)
                .ToEnumerable();

                Console.WriteLine("Found " + categoryPages.Count() + " categoryPages");

                foreach (var topLevelCategory in categoryPages)
                {
                    if (topLevelCategory.StartsWith("Die Gartenlaube (18"))
                    {
                        var subPages = (from members in wikisource.Query.categorymembers()
                                        where members.title == "Kategorie:" + topLevelCategory.Replace(' ', '_')
                                        select members)
                            .Pages
                            .Select(
                                page =>
                                    new
                                    {
                                        Title = page.info.title,
                                        Text = page.revisions()
                                        .Select(revision => revision.value)
                                        .FirstOrDefault()
                                    })
                            .ToEnumerable();

                        foreach (var contentPage in subPages)
                        {
                            Console.WriteLine("Writing: " + contentPage.Title);
                            writer.WriteLine(contentPage.Text);
                        }
                    }
                }
            }
        }
    }
}
