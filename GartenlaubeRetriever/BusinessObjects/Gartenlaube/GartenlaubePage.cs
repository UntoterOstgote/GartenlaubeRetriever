using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    [DataContract]
    public class GartenlaubePage
    {
        [DataMember]
        public string MarkdownSource { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public PageQuality PageQuality { get; set; }
    }
}
