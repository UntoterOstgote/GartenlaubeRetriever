using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToWiki;
using UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube;
using LinqToWiki.Generated;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using CommonMark;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.Services
{
    public class GartenlaubeService
    {
        private Wiki wiki;
        private List<string> yearPagesNames;
        private List<GartenlaubeYear> gartenlaubeYears;
        private List<GartenlaubeIssue> gartenlaubeIssues;

        public GartenlaubeService(string path)
        {
            wiki = new Wiki("MartusBot/1.0, janningarne@gmail.com ", "https://de.wikisource.org");
            //Build Entry Level Pages
            yearPagesNames = new List<string>();
            for (int i = 53; i < 100; i++)
            {
                yearPagesNames.Add(string.Format("Die Gartenlaube (18{0})", i.ToString()));
            }

            gartenlaubeYears = new List<GartenlaubeYear>();
            gartenlaubeIssues = new List<GartenlaubeIssue>();
        }

        public void Run()
        {
            GetGartenlaubeYears();
            GetGartenlaubeIssuesAndArticles();
            GetGartenlaubePages();
        }

        private void GetGartenlaubeYears()
        {
            foreach (var yearPageName in yearPagesNames)
            {
                string year = Regex.Match(yearPageName, "\\d{4}").Value;
                Debug.WriteLine("Processing year " + year);

                PagesSource<Page> yearPage = wiki.CreateTitlesSource(yearPageName);
                var pageSource = yearPage.Select(
                    p =>
                    new
                    {
                        Text = p.revisions()
                        .Select(revision => revision.value)
                        .FirstOrDefault()
                    }).ToEnumerable()
                    .First();

                gartenlaubeYears.Add(
                    new GartenlaubeYear()
                    {
                        Year = new DateTime(int.Parse(year), 1, 1),
                        MarkdownSource = pageSource.Text
                    }
                    );
            }
        }

        private void GetGartenlaubeIssuesAndArticles()
        {
            foreach (var gartenlaubeYear in this.gartenlaubeYears)
            {
                using (StringReader reader = new StringReader(gartenlaubeYear.MarkdownSource))
                {
                    GartenlaubeIssue currentIssue = null;
                    GartenlaubeArticle currentArticle = null;
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("{{GartenlaubeHeft") && Regex.IsMatch(line, "\\d{1,2}"))
                        {
                            currentIssue = new GartenlaubeIssue();
                            currentIssue.IssueNumber = int.Parse(Regex.Match(line, "\\d{1,2}").Value);
                            gartenlaubeYear.GartenlaubeIssues.Add(currentIssue);
                        }
                        else if (line.StartsWith("{{GartenlaubeEintrag"))
                        {
                            currentArticle = new GartenlaubeArticle();
                            var parts = line.Split('|');

                            //Page Numbers?
                            if (Regex.IsMatch(parts[1], "\\d+"))
                            {
                                currentArticle.StartPage = int.Parse(Regex.Match(parts[1], "\\d+").Value);
                            }

                            if (Regex.IsMatch(line, @"\[\[.*?\]\]"))
                            {
                                var mc = Regex.Matches(line, @"\[\[(?<group>.*?)\]\]");

                                int matchCount = mc.Count;

                                if (matchCount == 1)
                                {
                                    string titleAspirant = mc[0].Groups["group"].Value;
                                    if (titleAspirant.Contains('|'))
                                    {
                                        var split = titleAspirant.Split('|');

                                        currentArticle.PageName = split[0];
                                        currentArticle.Title = split[1];
                                    }
                                    else
                                    {
                                        currentArticle.PageName = titleAspirant;
                                        currentArticle.Title = titleAspirant;
                                    }
                                }
                                else if (matchCount == 2)
                                {
                                    //{{GartenlaubeEintrag|7|Aus der Menschenheimath<br /><small>Briefe des Schulmeisters Johannes Frisch an seinen ehemaligen Schüler</small>|[[Emil Adolf Roßmäßler]]|[[Aus der Menschenheimath/Erster Brief|Erster Brief]]||JAHR=1853}}
                                    if (mc[1].Groups["group"].Value.Contains('|'))
                                    {
                                        var split = mc[1].Groups["group"].Value.Split('|');

                                        currentArticle.PageName = split[0];
                                        currentArticle.Title = split[1];
                                        currentArticle.Author = mc[0].Groups["group"].Value;
                                    }
                                    else if (!mc[1].Groups["group"].Value.Contains('|') && !mc[0].Groups["group"].Value.Contains('|'))
                                    {
                                        currentArticle.Author = mc[1].Groups["group"].Value;
                                        currentArticle.PageName = mc[0].Groups["group"].Value;
                                        currentArticle.Title = mc[0].Groups["group"].Value;
                                    }
                                    else if (mc[0].Groups["group"].Value.Contains('|'))
                                    {
                                        var split = mc[0].Groups["group"].Value.Split('|');

                                        currentArticle.PageName = split[0];
                                        currentArticle.Title = split[1];
                                        currentArticle.Author = mc[1].Groups["group"].Value;
                                    }
                                }
                                else if (matchCount == 3)
                                {
                                    if (mc[2].Groups["group"].Value.Contains('|'))
                                    {
                                        var split = mc[2].Groups["group"].Value.Split('|');

                                        currentArticle.PageName = split[0];
                                        currentArticle.Title = split[1];
                                        currentArticle.Author = mc[1].Groups["group"].Value;
                                    }
                                }

                                currentIssue.Articles.Add(currentArticle);
                            }
                        }
                    }
                }
            }
        }

        private void GetGartenlaubePages()
        {
            var query = from p in gartenlaubeYears
                        from i in p.GartenlaubeIssues
                        from a in i.Articles
                        select a;

            foreach (var article in query)
            {
                PagesSource<Page> articlePages = wiki.CreateTitlesSource(article.PageName);
                var pageSource = articlePages.Select(
                    p =>
                    new
                    {
                        Text = p.revisions()
                        .Select(revision => revision.value)
                        .FirstOrDefault()
                    }).ToEnumerable()
                    .First();
                article.MarkdownSource = pageSource.Text;
                //Debug.WriteLine(article.MarkdownSource);

                Debug.WriteLine(article.PageName);

                using (StringReader reader = new StringReader(pageSource.Text))
                {
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("|VORIGER="))
                        {
                            article.Previous = line.Replace("|VORIGER=", string.Empty);
                        }
                        if (line.StartsWith("|TITEL="))
                        {
                            article.Title = line.Replace("|TITEL=", string.Empty);
                        }
                        if (line.StartsWith("|NÄCHSTER="))
                        {
                            article.Next = line.Replace("|NÄCHSTER=", string.Empty);
                        }
                        if (line.StartsWith("|AUTOR"))
                        {
                            article.Author = 
                                line.Replace("|AUTOR", string.Empty).Replace("[[", string.Empty).Replace("]]", string.Empty);
                        }
                        if (line.StartsWith("|JAHR="))
                        {
                            article.Year = int.Parse(line.Replace("|JAHR=", string.Empty));
                        }
                        if (line.StartsWith("|KURZBESCHREIBUNG="))
                        {
                            article.ShortDescription = line.Replace("|KURZBESCHREIBUNG=", string.Empty);
                        }
                        if (line.StartsWith("|SONSTIGES="))
                        {
                            article.Notes = line.Replace("|SONSTIGES=", string.Empty);
                        }
                        if (line.StartsWith("|BEARBEITUNGSSTAND="))
                        {
                            article.PageQuality = line.Replace("|BEARBEITUNGSSTAND=", string.Empty);
                        }
                        if (line.StartsWith("{{SeitePR|"))
                        {
                            string pageLocation = Regex.Match(line, @"Die Gartenlaube \(18\d\d\).*\.jpg").Value;
                            article.GartenlaubePages.Add(
                                new GartenlaubePage()
                                {
                                    Location = pageLocation
                                });
                        }
                        if (line.StartsWith("[[Kategorie:"))
                        {
                            article.Categories.Add(line.Replace("[[Kategorie:", string.Empty).Replace("]]", string.Empty));
                        }
                    }
                }

                
            }
        }
    }
}
