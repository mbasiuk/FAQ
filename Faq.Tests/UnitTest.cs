using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Faq.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void GetAllFaqTest()
        {
            var faqs = Faq.Library.Faq.GetAllFaq();
        }

        [TestMethod]
        public void FindTest()
        {
            var faqs = Library.Faq.FindFaq("%how much%");
        }

        [TestMethod]
        public void InsertAndDeleteTest()
        {
            Library.Faq faq = new Library.Faq("questions", "answer");
            faq.InsertFaq();
            faq.Delete();
        }

    }
}
