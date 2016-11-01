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
                    GartenlaubeIssue currentIssue;
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("{{GartenlaubeHeft") && Regex.IsMatch(line, "\\d{1,2}"))
                        {
                            currentIssue = new GartenlaubeIssue();
                            currentIssue.IssueNumber = int.Parse(Regex.Match(line, "\\d{1,2}").Value);
                        }
                        else if (line.StartsWith("{{GartenlaubeEintrag"))
                        {
                            GartenlaubeArticle article = new GartenlaubeArticle();
                            var parts = line.Split('|');

                            //Page Numbers?
                            if (Regex.IsMatch(parts[1], "\\d+"))
                            {
                                article.StartPage = int.Parse(Regex.Match(parts[1], "\\d+").Value);
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

                                        article.PageName = split[0];
                                        article.Title = split[1];
                                    }
                                    else
                                    {
                                        article.PageName = titleAspirant;
                                        article.Title = titleAspirant;
                                    }
                                }
                                else
                                {
                                    for(int i = 1; i > -1; i--)
                                    {
                                        if (mc[i].Groups["group"].Value.Contains('|'))
                                        {
                                            var split = mc[i].Groups["group"].Value.Split('|');

                                            article.PageName = split[0];
                                            article.Title = split[1];
                                        }
                                        else
                                        {
                                            article.Author = mc[i].Groups["group"].Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
