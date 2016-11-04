using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube
{
    [DataContract]
    public class GartenlaubeArticle
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Sorting { get; set; }
        [DataMember]
        public List<string> Categories { get; set; }
        [DataMember]
        public string Previous { get; set; }
        [DataMember]
        public string Next { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string PagesString { get; set; }
        [DataMember]
        public int Issue { get; set; }
        [DataMember]
        public int StartPage { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Type { get; set; }
        public string MarkdownSource { get; set; }

        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageQuality { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }

        private string fullText;
        public string FullText
        {
            get
            {
                fullText = string.Empty;
                foreach (var item in GartenlaubePages)
                {
                    fullText += item.MarkdownSource;
                }

                return fullText;
            }
            set
            {
                fullText = value;
            }
        }

        [DataMember]
        public List<GartenlaubePage> GartenlaubePages { get; set; }

        public GartenlaubeArticle()
        {
            this.GartenlaubePages = new List<GartenlaubePage>();
            this.Categories = new List<string>();
        }
    }
}
