using LuceneWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class TextWriter : BaseWriter
    {
        public TextWriter(string dataFolder) : base(dataFolder)
        {

        }

        public void AddUpdateTextToIndex(Text text)
        {
            AddUpdateItemsToIndex(new List<TextDocument> { (TextDocument)text });
        }

        public void AddUpdateTextToIndex(List<Text> text)
        {
            AddUpdateItemsToIndex(text.Select(t => (TextDocument)t).ToList());
        }


        public void DeleteTextFromIndex(Text text)
        {
            DeleteItemsFromIndex(new List<TextDocument> { (TextDocument)text });
        }

        public void DeleteTextFromIndex(int id)
        {
            DeleteItemsFromIndex(new List<TextDocument> { new TextDocument { Id = id } });
        }
    }
}
