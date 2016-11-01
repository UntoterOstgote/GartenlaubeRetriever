using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UntoterOstgote.Martus.Ocr.Postprocessing
{
    public class Utils
    {
        public enum Language
        {
            English,
            Portuguese,
            German
        }

        // DE regular expressions pre-compilation.

        // Regular expression to detect words in the German language:
        // [a-zãõàáéíóúâêôç]+([-][a-zãõàáéíóúâêôç]+)+|[a-zãõàáéíóúâêôç]+
        private static readonly string lettersClassForWordPatternDE = @"[a-zäöüß]";
        private static string regExStrWordPatternDE = String.Format(@"{0}+([-]{0}+)+|{0}+", lettersClassForWordPatternDE);
        private static Regex regExCompWordPatternDE = new Regex(regExStrWordPatternDE, RegexOptions.Compiled | RegexOptions.IgnoreCase);


        // PT regular expressions pre-compilation.

        // Regular expression to detect words in the Portuguese language:
        // [a-zãõàáéíóúâêôç]+([-][a-zãõàáéíóúâêôç]+)+|[a-zãõàáéíóúâêôç]+
        private static readonly string lettersClassForWordPatternPT = @"[a-zãõàáéíóúâêôç]";
        private static string regExStrWordPatternPT = String.Format(@"{0}+([-]{0}+)+|{0}+", lettersClassForWordPatternPT);
        private static Regex regExCompWordPatternPT = new Regex(regExStrWordPatternPT, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // EN regular expressions pre-compilation.
        // Regular expression to detect words in the Portuguese language:
        // [a-z]+['][a-z]+|[a-z]+
        private static readonly string lettersClassForWordPatternEN = @"[a-z]";
        private static string regExStrWordPatternEN = String.Format(@"{0}+[']{0}+|{0}+", lettersClassForWordPatternEN);
        private static Regex regExCompWordPatternEN = new Regex(regExStrWordPatternEN, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Regex RegExCompWordDE
        {
            get
            {
                return regExCompWordPatternDE;
            }
        }

        public static Regex RegExCompWordPT
        {
            get
            {
                return regExCompWordPatternPT;
            }
        }
        public static Regex RegExCompWordEN
        {
            get
            {
                return regExCompWordPatternEN;
            }
        }
        public class MyTupleStr
        {
            public string Item1 { get; set; }
            public string Item2 { get; set; }
            public MyTupleStr(string item1, string item2)
            {
                this.Item1 = item1;
                this.Item2 = item2;
            }
        }
    }
}