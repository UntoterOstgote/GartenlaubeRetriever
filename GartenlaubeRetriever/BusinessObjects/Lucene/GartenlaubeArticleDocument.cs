using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Lucene
{
    public class GartenlaubeArticleDocument : ADocument
    {
        private string title;
        private string author;
        private string fullText;

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
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                this.AddParameterToDocumentNoStoreParameter("Title", title);
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

        public static explicit operator GartenlaubeArticleDocument(GartenlaubeArticle gartenlaubeArticle)
        {
            return new GartenlaubeArticleDocument()
            {
                Id = gartenlaubeArticle.Id,
                Author = gartenlaubeArticle.Author,
                Title = gartenlaubeArticle.Title,
                FullText = gartenlaubeArticle.FullText
            };
        }
    }
}
