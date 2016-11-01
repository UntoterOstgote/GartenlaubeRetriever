using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class TextDocument : ADocument
    {
        private string author;
        private string fullText;
        private PageQuality pageQuality;

        [SearchField]
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
                this.AddParameterToDocumentNoStoreParameter("Author", author);
            }
        }

        [SearchField]
        public string FullText
        {
            get
            {
                return fullText;
            }
            set
            {
                fullText = value;
                this.AddParameterToDocumentNoStoreParameter("FullText", fullText);
            }
        }

        [SearchField]
        public PageQuality PageQuality
        {
            get
            {
                return pageQuality;
            }
            set
            {
                pageQuality = value;
                this.AddParameterToDocumentNoStoreParameter("PageQuality", pageQuality);
            }
        }

        public static explicit operator TextDocument(Text text)
        {
            return new TextDocument()
            {
                Author = text.Author,
                FullText = text.FullText,
                PageQuality = text.PageQuality
            };
        }
    }
}
