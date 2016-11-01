using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    public class GartenlaubeYear
    {
        public DateTime Year { get; set; }
        public List<GartenlaubeIssue> GartenlaubeIssues { get; set; }
        public string MarkdownSource { get; set; }
    }
}
