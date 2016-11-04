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
using UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Lucene;
using UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    static class Program
    {
        private const int startYear = 53;
        private const int endYear = 57;

        static void Main(string[] args)
        {
            GartenlaubeService service = new GartenlaubeService(startYear, endYear);

            service.Run(false);

//            WriteGartenlaubeIndex(service);

            SearchTest(service, "Luther", "FullText");
            SearchTest(service, "Stoll*", "Author");

            //DownloadGartenlaubeFromWikisource();



            //WriteGartenlaubeIndex();
            //SearchTest("Amerika");


            //SpellChecker checker = new SpellChecker();
            //checker.Train(level4Filename);
            //    checker.Train(level4Filename, true);

            //int wordCount = 0;

            //if (checker.DataOnWord("Geheimniß", out wordCount))
            //{
            //    Console.WriteLine("Geheimniß: " + wordCount +  " mal.");
            //    Console.WriteLine(checker.Correct("Gekeimniß"));

            //}


            Console.WriteLine("Ready!");
            Console.ReadLine();
        }

        private static void WriteGartenlaubeIndex(GartenlaubeService service)
        {
            List<GartenlaubeArticle> query = (from y in service.GartenlaubeYears
                        from i in y.GartenlaubeIssues
                        from a in i.Articles
                        select a).ToList();

            string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("GartenlaubeIndex_{0}_{1}", startYear, endYear));
            var articleWriter = new GartenlaubeArticleWriter(indexPath);

            foreach (GartenlaubeArticle article in query)
            {
                articleWriter.AddUpdateGartenlaubeArticleToIndex(article);
            }
        }

        private static string SearchTest(GartenlaubeService service, string query, string field)
        {
            List<GartenlaubeYear> years = service.GartenlaubeYears;

            string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("GartenlaubeIndex_{0}_{1}", startYear, endYear));
            var searcher = new TextSearcher(indexPath);


            Console.WriteLine("Search for " + query + " in field " + field);
            var res = searcher.SearchText(query, field);

            foreach (var item in res.SearchResultItems)
            {
                Console.WriteLine("Result with ID: {0}", item.Id);

                var q = (from y in years
                         from i in y.GartenlaubeIssues
                         from a in i.Articles
                         where a.Id == item.Id
                         select a).First();

                Console.WriteLine("Title: " + q.Title + "; Score: " + item.Score);
            }

            return string.Empty;
        }

        //private static void DownloadGartenlaubeFromWikisource()
        //{
        //    using (StreamWriter writer = new StreamWriter(fullFilename))
        //    {
        //        var wikisource = new Wiki("MartusBot/1.0, janningarne@gmail.com ", "https://de.wikisource.org");
        //        var categoryPages = (from members in wikisource.Query.categorymembers()
        //                     where members.title == "Kategorie:Die Gartenlaube"
        //                     select members.title)
        //        .ToEnumerable();

        //        Console.WriteLine("Found " + categoryPages.Count() + " categoryPages");

        //        foreach (var topLevelCategory in categoryPages)
        //        {
        //            if (topLevelCategory.StartsWith("Die Gartenlaube (18"))
        //            {
        //                var subPages = (from members in wikisource.Query.categorymembers()
        //                                where members.title == "Kategorie:" + topLevelCategory.Replace(' ', '_')
        //                                select members)
        //                    .Pages
        //                    .Select(
        //                        page =>
        //                            new
        //                            {
        //                                Title = page.info.title,
        //                                Text = page.revisions()
        //                                .Select(revision => revision.value)
        //                                .FirstOrDefault()
        //                            })
        //                    .ToEnumerable();

        //                foreach (var contentPage in subPages)
        //                {
        //                    Console.WriteLine("Writing: " + contentPage.Title);
        //                    writer.WriteLine(contentPage.Text);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
