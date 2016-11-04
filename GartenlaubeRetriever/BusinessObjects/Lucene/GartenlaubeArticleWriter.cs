using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Gartenlaube;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Lucene
{
    public class GartenlaubeArticleWriter : BaseWriter
    {
        public GartenlaubeArticleWriter(string dataFolder) : base(dataFolder)
        {
        }

        public void AddUpdateGartenlaubeArticleToIndex(GartenlaubeArticle gartenlaubeArticle)
        {
            AddUpdateItemsToIndex(new List<GartenlaubeArticleDocument> { (GartenlaubeArticleDocument)gartenlaubeArticle });
        }

        public void AddUpdateGartenlaubeArticleToIndex(List<GartenlaubeArticle> gartenlaubeArticle)
        {
            AddUpdateItemsToIndex(gartenlaubeArticle.Select(g => (GartenlaubeArticleDocument)g).ToList());
        }

        public void DeleteGartenlaubeArticleFromIndex(GartenlaubeArticle gartenlaubeArticle)
        {
            DeleteItemsFromIndex(new List<GartenlaubeArticleDocument> { (GartenlaubeArticleDocument) gartenlaubeArticle});
        }

        public void DeleteGartenlaubeArticleFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<GartenlaubeArticleDocument> { new GartenlaubeArticleDocument { Id = id } });
        }
    }
}
