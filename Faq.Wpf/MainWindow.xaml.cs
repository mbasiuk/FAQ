using System.Windows;
using System.Reactive.Linq;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using LFaq = Faq.Library.Faq;
using Faq.Library.Extentions;
using System.Reactive;
using System.Windows.Input;

namespace WpfFaq
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
    }
}
