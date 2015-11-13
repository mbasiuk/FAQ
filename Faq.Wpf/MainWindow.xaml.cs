using Faq.Library.Extentions;
using System;
using System.Reactive;
using System.Reactive.Linq;
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
        System.Collections.ObjectModel.ObservableCollection<LFaq> AllFaqs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllFaqs = new System.Collections.ObjectModel.ObservableCollection<LFaq>(LFaq.GetAllFaq());
            Hande(null);
            Observable.FromEventPattern<TextChangedEventArgs>(searchText, "TextChanged")
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(messages => Hande(messages));
        }

        private object Hande(EventPattern<TextChangedEventArgs> messages)
        {
            listBox.ItemsSource = AllFaqs.Filter(searchText.Text);
            return messages;
        }

        /// <summary>
        /// Enable scrolling fix on touch screen;
        /// </summary>
        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(questionText.Text) || string.IsNullOrWhiteSpace(answerText.Text))
            {
                return;
            }
            LFaq faq = new LFaq(questionText.Text, answerText.Text);
            AllFaqs.Add(faq);
            questionText.Text = string.Empty;
            answerText.Text = string.Empty;
            Hande(null);
        }
    }
}
