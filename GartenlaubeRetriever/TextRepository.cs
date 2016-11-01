using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntoterOstgote.Martus.GartenlaubeKorpus
{
    public class TextRepository
    {
        public static List<Text> GetTexts()
        {
            return new List<Text>()
            {
                new Text()
                {
                    Author = "Arne Janning",
                    FullText = "Was ich erlebt und wie ich es angefangen habe, mir eine Existenz zu verschaffen, davon erzähle ich Dir ein ander Mal. Heute nur zur Beantwortung der übrigen Fragen Deines Briefes, des einzigen, den ich seit unserer Trennung empfangen. Du kündigst mir neue Auswanderer aus unserem engern Vaterlande an und fragst dabei nach unsern Landsleuten und wie sie leben hier, was sie treiben, nach ihrer Stellung den Eingebornen gegenüber u. a. m. Das sind viele Fragen auf ein Mal, die ich kaum in einem  Briefe werde beantworten können."
                },
                new Text()
                {
                    Author = "Steffen Martus",
                    FullText = "Meinst Du die Stellung der Deutschen den übrigen ''nicht'' eingeborenen Amerikanern gegenüber, so kann ich Dir mit Stolz berichten, daß sie bei weitem am meisten von allen eingewanderten Völkern gelten. Ich verstehe darunter den einzelnen Deutschen, nicht die Deutschen als Volk. In dem einzelnen Deutschen achtet man den geschickten Handwerker, den fleißigen unverdrossenen Ackerbauer, überhaupt den genügsamen und dabei ehrlichen Arbeiter. Die neuere Zeit mit ihren Kämpfen hat uns aus Deutschland viel Intelligenz herübergesandt, die sich in den meisten Fällen rasch Bahn gebrochen und den Amerikanern den Glauben genommen hat, als bestände das deutsche Volk nur aus armen Bauern und Handwerkern, die zu Hause kein Brod haben. Deutschland ist durch diese sehr in der Achtung gestiegen. Die Deutschen als Volk bespöttelt, ja verachtet der Amerikaner noch, und weil sie auch hier mit allen Ansprüchen auf Selbstständigkeit und Macht doch nur zerstreute Massen ohne innere Gestaltung und Zusammenhalt bilden, so gelten sie im Verhältniß zu dem, was sie durch Zahl und Bildung gelten könnten, eben doch am wenigsten."
                }
            };
        }
    }
}
