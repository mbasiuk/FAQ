namespace WpfFaq
{
    public interface IFaqInerface
    {
        void AppendNewFaq();
        void AppendNewFaq(Faq.Library.Faq faq);
        /// <param name="element">Framefork event source</param>
        void ChooseFaqElement(object element);
        /// <param name="element">Framefork event source</param>
        void RemoveChosenFaq(object element);
        void ConfirmChosenElementUpdate();
        void DisgardChosenElementUpdate();
        void SaveAll();

    }
}
