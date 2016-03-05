using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FAQ = Faq.Library.Faq;

namespace WpfFaq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<FAQ> AllFaqs;
        TextBlock active;
        Style questionStyle = null;
        Style answerStyle = null;
        Dictionary<object, FAQ> QuestionByElement = null;
        Dictionary<object, FAQ> AnswerByElement = null;
        Dictionary<FAQ, TextBlock> QuestionElementByFaq = null;
        Dictionary<FAQ, TextBlock> AnswerElementByFaq = null;

        public MainWindow()
        {
            InitializeComponent();
            QuestionByElement = new Dictionary<object, FAQ>();
            AnswerByElement = new Dictionary<object, FAQ>();
            QuestionElementByFaq = new Dictionary<FAQ, TextBlock>();
            AnswerElementByFaq = new Dictionary<FAQ, TextBlock>();
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
            SizeHooks();
        }

        private void SizeHooks()
        {
            //  LayoutRoot.Height = FaqWindow.Height - 80;
        }

        private void CreateFaq(List<FAQ> faqs, int skip = 0)
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
                SetTop(i, question);
                QuestionByElement[question] = faqs[i];
                QuestionElementByFaq[faqs[i]] = question;

                TextBlock answer = new TextBlock();
                answer.Text = faqs[i].Answer;
                answer.Style = answerStyle;
                answer.PreviewMouseDown += textBlock_PreviewMouseDown;
                LayoutRoot.Children.Add(answer);
                Canvas.SetLeft(answer, 200);
                SetTop(i, answer);
                AnswerByElement[answer] = faqs[i];
                AnswerElementByFaq[faqs[i]] = answer;
            }

        }

        private static void SetTop(int i, UIElement block)
        {
            Canvas.SetTop(block, i * 50);
        }

        private void RecalculateHeight()
        {
            LayoutRoot.Height = Math.Max(270, AllFaqs.Count * 50 + 25);
        }

        private void RecalculatePositions()
        {
            for (int i = 0; i < AllFaqs.Count; i++)
            {
                FAQ faq = AllFaqs[i];
                TextBlock questions = QuestionElementByFaq[faq];
                SetTop(i, questions);
                TextBlock answer = AnswerElementByFaq[faq];
                SetTop(i, answer);
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
            
            active = sender as TextBlock;
            if (active != null)
            {
                editActiveTextBlock();
                e.Handled = true;
            }

        }

        private void editActiveTextBlock()
        {
            editorContainer.Visibility = Visibility.Visible;
            editor.Text = active.Text;
            Canvas.SetTop(editorContainer, Canvas.GetTop(active));
            editor.Focus();          
            Keyboard.Focus(editor);
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
            FAQ faq = new FAQ();
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

            if (QuestionByElement.ContainsKey(active))
            {
                FAQ activeFaq = QuestionByElement[active];
                RemoveActiveFaq(activeFaq);
            }

            if (AnswerByElement.ContainsKey(active))
            {
                FAQ activeFaq = AnswerByElement[active];
                RemoveActiveFaq(activeFaq);
            }

            RecalculatePositions();

            RecalculateHeight();

            active = null;
            editorContainer.Visibility = Visibility.Collapsed;

        }

        private void RemoveActiveFaq(FAQ activeFaq)
        {
            TextBlock questionElement = QuestionElementByFaq[activeFaq];
            if (questionElement != null)
            {
                LayoutRoot.Children.Remove(questionElement);
                QuestionElementByFaq.Remove(activeFaq);
                QuestionByElement.Remove(questionElement);
            }

            TextBlock answerElement = AnswerElementByFaq[activeFaq];
            if (answerElement != null)
            {
                LayoutRoot.Children.Remove(answerElement);
                AnswerElementByFaq.Remove(activeFaq);
                AnswerByElement.Remove(answerElement);
            }

            AllFaqs.Remove(activeFaq);
        }


        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (active != null)
            {
                active.Text = editor.Text;
                if (QuestionByElement.ContainsKey(active))
                {
                    QuestionByElement[active].Question = active.Text;
                }
                if (AnswerByElement.ContainsKey(active))
                {
                    AnswerByElement[active].Answer = active.Text;
                }
            }
            active = null;
            editorContainer.Visibility = Visibility.Collapsed;
        }

        private void FaqWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    CancelEdit_Click(sender, e);
                    break;
                case Key.Enter:
                    buttonOk_Click(sender, e);
                    break;
            }
        }

        private void FaqWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeHooks();
        }
    }
}
