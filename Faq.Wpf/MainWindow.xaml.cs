using System.Windows;
using System.Reactive.Linq;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using LFaq = Faq.Library.Faq;
using Faq.Library.Extentions;
using System.Reactive;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<LFaq> AllFaqs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllFaqs = LFaq.GetAllFaq();
            DataContext = AllFaqs;
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

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
