using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    public class GartenlaubeArticle
    {
        public string Sorting { get; set; }
        public List<string> Categories { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
        public int Year { get; set; }
        public string PagesString { get; set; }
        public int Issue { get; set; }
        public int StartPage { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public string MarkdownSource { get; set; }
        public string PageName { get; set; }
        public string PageQuality { get; set; }
        public string Notes { get; set; }
        public string ShortDescription { get; set; }
        public List<GartenlaubePage> GartenlaubePages { get; set; }

        public GartenlaubeArticle()
        {
            this.GartenlaubePages = new List<GartenlaubePage>();
            this.Categories = new List<string>();
        }
    }
}
