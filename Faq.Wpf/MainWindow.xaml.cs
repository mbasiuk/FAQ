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

        IFaqInerface operationContaner = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            questionStyle = FindResource("QuestionStyle") as Style;
            answerStyle = FindResource("AnswerStyle") as Style;

            AllFaqs = Faq.Library.FaqManager.Load();
            if (AllFaqs.Count == 0)
            {
                noFaqsContainer.Visibility = Visibility.Visible;
            }
            else
            {
                CreateFaq(AllFaqs);
                RecalculateHeight();
            }
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
                question.Tag = faqs[i];
                question.Text = faqs[i].Question;
                question.Style = questionStyle;
                question.PreviewMouseDown += textBlock_PreviewMouseDown;
                LayoutRoot.Children.Add(question);
                Canvas.SetLeft(question, 20);
                Canvas.SetTop(question, i * 50);

                TextBlock answer = new TextBlock();
                answer.Tag = faqs[i];
                answer.Text = faqs[i].Answer;
                answer.Style = answerStyle;
                answer.PreviewMouseDown += textBlock_PreviewMouseDown;
                LayoutRoot.Children.Add(answer);
                Canvas.SetLeft(answer, 200);
                Canvas.SetTop(answer, i * 50);
            }



        }

        private void RecalculateHeight()
        {
            LayoutRoot.Height = System.Math.Max(280, AllFaqs.Count * 50 + 25);
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
                editor.Tag = active.Tag;
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
            active = null;
            editorContainer.Visibility = Visibility.Collapsed;
            Faq.Library.FaqManager.Save(AllFaqs);
        }

        private void addFaq_Click(object sender, RoutedEventArgs e)
        {
            noFaqsContainer.Visibility = Visibility.Hidden;
            LFaq faq = new LFaq();
            AllFaqs.Add(faq);
            CreateFaq(AllFaqs, AllFaqs.Count - 1);
            RecalculateHeight();
        }

        private void removeFaq_Click(object sender, RoutedEventArgs e)
        {
            if (active == null)
            {
                return;
            }
            RecalculateHeight();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (active != null)
            {
                active.Text = editor.Text;
            }
            active = null;
            editorContainer.Visibility = Visibility.Collapsed;
        }
    }
}
