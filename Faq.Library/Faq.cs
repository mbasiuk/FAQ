namespace Faq.Library
{
    /// <summary>
    /// Represent a Faq
    /// </summary>
    public partial class Faq
    {
        public Faq()
        {

        }

        public Faq(string question, string answer)
        {
            Answer = answer;
            Question = question;
        }

        /// <summary>
        /// Summary
        /// </summary>
        public string Question
        {
            get; set;
        }

        /// <summary>
        /// Answer
        /// </summary>
        public string Answer
        {
            get; set;
        }
    }
}
