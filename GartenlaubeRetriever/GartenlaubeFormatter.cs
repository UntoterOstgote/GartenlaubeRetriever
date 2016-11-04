using CommonMark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommonMark.Syntax;
using System.Runtime.Serialization;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class GartenlaubeFormatter : CommonMark.Formatters.HtmlFormatter
    {
        private int counter = 0;

        public GartenlaubeFormatter(System.IO.TextWriter target, CommonMarkSettings settings) : base (target, settings)
        {

        }

        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            
            Console.WriteLine(string.Format("Block: {0}, isOpening: {1}, isClosing: {2}, block.Tag: {3}", block.StringContent, isOpening, isClosing, block.Tag));
            base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
        }

        protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (inline.LiteralContent != null)
            {
                if (inline.LiteralContent.Contains("section begin"))
                {

                } 
            }
            if (inline.LiteralContent == "<noinclude>")
            {
                if (inline.NextSibling.LiteralContent != null)
                {
                    if (Regex.IsMatch(inline.NextSibling.LiteralContent, "<pagequality level=\"\\d\""))
                    {
                        int pageQuality = int.Parse(Regex.Match(inline.NextSibling.LiteralContent, 
                            "<pagequality level=\"(?<pagequality>\\d)\"").Groups["pagequality"].Value);
                        
                    } 
                }
                
            }
            //if (inline.Tag == InlineTag.String)
            //{
                base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
            //}
            //else
            //{
            //    ignoreChildNodes = false;
            //}
            //Console.WriteLine(string.Format("Inline: {0}, isOpening: {1}, isClosing: {2}, inline.Tag: {3}", inline.LiteralContent, isOpening, isClosing, inline.Tag));

            Console.WriteLine(counter);
        }
    }

    [DataContract]
    public enum PageQuality
    {
        [EnumMember]
        WithoutText = 0,
        [EnumMember]
        NotProofread = 1,
        [EnumMember]
        Problematic = 2,
        [EnumMember]
        Proofread = 3,
        [EnumMember]
        Validated = 4
    }
}
