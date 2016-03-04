using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LFaq = Faq.Library.Faq;

namespace WpfFaq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<LFaq> AllFaqs;
        TextBlock active;
        Style questionStyle = null;
        Style answerStyle = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            questionStyle = FindResource("QuestionStyle") as Style;
            answerStyle = FindResource("AnswerStyle") as Style;

            AllFaqs = Faq.Library.FaqManager.Load();

            CreateFaq(AllFaqs);
        }

        private void CreateFaq(List<LFaq> faqs, int skip = 0)
        {
            for (int i = 0; i < faqs.Count; i++)
            {
                if (i < skip)
                {
                    continue;
                }

                TextBlock question = new TextBlock();
                question.Text = faqs[i].Question;
                question.Style = questionStyle;
                question.PreviewMouseDown += textBlock_PreviewMouseDown;
                LayoutRoot.Children.Add(question);
                Canvas.SetLeft(question, 20);
                Canvas.SetTop(question, i * 50);

                TextBlock answer = new TextBlock();
                answer.Text = faqs[i].Answer;
                answer.Style = answerStyle;
                answer.PreviewMouseDown += textBlock_PreviewMouseDown;
                LayoutRoot.Children.Add(answer);
                Canvas.SetLeft(answer, 200);
                Canvas.SetTop(answer, i * 50);
            }
        }


        /// <summary>
        /// Enable scrolling fix on touch screen;
        /// </summary>
        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void textBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            editorContainer.Visibility = Visibility.Visible;
            active = sender as TextBlock;
            if (active != null)
            {
                editor.Text = active.Text;
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (active != null)
            {
                active.Text = editor.Text;
            }
        }

        private void CancelEdit_Click(object sender, RoutedEventArgs e)
        {
            active = null;
            editorContainer.Visibility = Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (active != null)
            {
                active.Text = editor.Text;
            }
            Faq.Library.FaqManager.Save(AllFaqs);
        }

        private void addFaq_Click(object sender, RoutedEventArgs e)
        {
            LFaq faq = new LFaq();
            AllFaqs.Add(faq);
            CreateFaq(AllFaqs, AllFaqs.Count - 1);
        }

        private void removeFaq_Click(object sender, RoutedEventArgs e)
        {
            if (active == null)
            {
                return;
            }
          
        }
    }
}
