using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus.BusinessObjects.Lucene
{
    public class GartenlaubeArticleSearcher : BaseSearcher
    {
        public GartenlaubeArticleSearcher(string dataFolder) : base(dataFolder)
        {

        }

        public SearchResult SearchGartenlaubeArticleText(string searchTerm, string field)
        {
            return Search<GartenlaubeArticleDocument>(field, searchTerm);
        }
    }
}
