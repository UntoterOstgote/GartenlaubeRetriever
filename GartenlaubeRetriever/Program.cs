using LinqToWiki;
using LinqToWiki.Generated;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GartenlaubeRetriever
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("results.txt"))
            {
                var wiki = new Wiki("MartusBot/1.0", "https://de.wikisource.org");
                var pages = (from cm in wiki.Query.categorymembers()
                             where cm.title == "Kategorie:Die Gartenlaube"
                             select cm.title)
                .ToEnumerable();

                foreach (var topLevelCategory in pages)
                {
                    if (topLevelCategory.StartsWith("Die Gartenlaube (18"))
                    {
                        var subPages = (from cm in wiki.Query.categorymembers()
                                        where cm.title == "Kategorie:" + topLevelCategory.Replace(' ', '_')
                                        select cm)
                            .Pages
                            .Select(
                                page =>
                                    new
                                    {
                                        title = page.info.title,
                                        text = page.revisions()
                                        .Select(r => r.value)
                                        .FirstOrDefault()
                                    })
                            .ToEnumerable();

                        foreach (var subLevelPage in subPages)
                        {
                            writer.WriteLine(subLevelPage.title + Environment.NewLine + subLevelPage.text);
                        }
                    }
                }
            }

            Console.WriteLine("Ready");
            Console.ReadLine();
        }
    }
}
