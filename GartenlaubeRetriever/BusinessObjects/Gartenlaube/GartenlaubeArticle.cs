using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    public class GartenlaubeArticle
    {
        public int StartPage { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public string MarkdownSource { get; set; }
        public string PageName { get; set; }
        public List<GartenlaubePage> GartenlaubePages { get; set; }
    }
}
