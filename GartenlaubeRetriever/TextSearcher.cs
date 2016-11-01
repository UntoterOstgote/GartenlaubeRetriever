using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class TextSearcher : BaseSearcher
    {
        public TextSearcher(string dataFolder) : base(dataFolder)
        {
        }

        public SearchResult SearchText(string searchTerm, string field)
        {
            return Search<TextDocument>(field, searchTerm);
        }
    }
}
