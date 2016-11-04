using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    [DataContract]
    public class GartenlaubeYear
    {
        [DataMember]
        public DateTime Year { get; set; }
        [DataMember]
        public List<GartenlaubeIssue> GartenlaubeIssues { get; set; }
        public string MarkdownSource { get; set; }

        public GartenlaubeYear()
        {
            this.GartenlaubeIssues = new List<GartenlaubeIssue>();
        }
    }
}
