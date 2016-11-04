using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    [DataContract]
    public class GartenlaubeIssue
    {
        [DataMember]
        public int IssueNumber { get; set; }
        [DataMember]
        public List<GartenlaubeArticle> Articles { get; set; }

        public GartenlaubeIssue()
        {
            this.Articles = new List<Gartenlaube.GartenlaubeArticle>();
        }
    }
}
