using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    public class GartenlaubeIssue
    {
        public int IssueNumber { get; set; }
        public List<GartenlaubeArticle> Articles { get; set; }

        public GartenlaubeIssue()
        {
            this.Articles = new List<Gartenlaube.GartenlaubeArticle>();
        }
    }
}
