using System.Collections.Generic;
using System.Linq;

namespace Faq.Library.Extentions
{
    public static class FaqExtentions
    {
        public static IEnumerable<Faq> Filter(this List<Faq> AllFaqs, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return AllFaqs;
            }
            else
            {
                return AllFaqs.Where(i => i.Answer.Contains(pattern) || i.Question.Contains(pattern));
            }
        }
    }
}
