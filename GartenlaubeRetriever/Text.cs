using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class Text
    {
        public int Id { get; set; }
        public PageQuality PageQuality { get; set; }
        public string Author { get; set; }
        public string FullText { get; set; }
    }
}
