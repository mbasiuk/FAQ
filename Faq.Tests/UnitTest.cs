using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Faq.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void GetAllFaqTest()
        {
            var faqs = Library.FaqManager.Load();
            Assert.IsNotNull(faqs);
          
        }

        [TestMethod]
        public void SaveTest()
        {
            List<Library.Faq> faqs = new List<Library.Faq>();
            faqs.Add(new Library.Faq("q1", "a1"));
            faqs.Add(new Library.Faq("q2", "a2"));
            Library.FaqManager.Save(faqs);
        }

    }
}
