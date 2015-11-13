using System.Windows;
using Expression.Blend.SampleData.SampleDataSource;
using System.Reactive.Linq;
using System.Windows.Controls;
using System;
using System.Linq;
using Microsoft.Expression.Interactivity.Core;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                SampleDataSource source = (SampleDataSource)DataContext;
                foreach (var domainFaq in source.Faq)
                {
                    Faq.Library.Faq faq = new Faq.Library.Faq(domainFaq.Question, domainFaq.Answer);
                    faq.InsertFaq();
                }
            }
            else
            {
                DataContext = Faq.Library.Faq.GetAllFaq();
                Observable.FromEventPattern<TextChangedEventArgs>(searchText, "TextChanged")                    
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .ObserveOn(System.Threading.SynchronizationContext.Current)
                    .Subscribe(messages => Hande(messages));
            }
        }

        private object Hande(System.Reactive.EventPattern<TextChangedEventArgs> messages)
        {
            if (string.IsNullOrWhiteSpace(searchText.Text))
            {
                listBox.ItemsSource = Faq.Library.Faq.GetAllFaq();
            }
            else
            {                
                listBox.ItemsSource = Faq.Library.Faq.FindFaq(string.Format("%{0}%", searchText.Text));
            }
            return messages;
        }

        public void FocusTextBoxSearch()
        {
            searchText.Focus();
        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void searchText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true;
            }
        }
    }
}
