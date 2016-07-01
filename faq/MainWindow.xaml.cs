using Faq.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Faq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsUpdated;

        string ExternalValue = null;
        bool IsUpdateExternal = false;
        FaqFile file;

        DispatcherTimer timer;

        public enum ViewEditMode
        {
            ViewAndEdit, View, Edit
        }

        ViewEditMode CurrentMode = ViewEditMode.ViewAndEdit;

        public MainWindow()
        {
            InitializeComponent();
            IsUpdated = false;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            string faqPath = ConfigurationManager.AppSettings["faqPath"];
            try
            {
                CurrentMode = (ViewEditMode)Settings.Default.viewMode;
                InvalidateViewMode();
            }
            catch { }

            file = new FaqFile(faqPath);
            file.ContentChanged += File_ContentChanged;
            EditFaq.Text = file.GetContent();

            InvalidateView();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void File_ContentChanged(string newFileContent)
        {
            ExternalValue = newFileContent;
            IsUpdateExternal = true;
            IsUpdated = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!IsUpdated)
            {
                return;
            }

            IsUpdated = false;

            InvalidateView();

        }

        void InvalidateView()
        {
            if (IsUpdateExternal)
            {
                IsUpdateExternal = false;
                EditFaq.Text = ExternalValue;
            }


            string text = EditFaq.Text;
            ViewFaq.Inlines.Clear();
            using (var reader = new StringReader(text))
            {
                string readerText = string.Empty;
                while ((readerText = reader.ReadLine()) != null)
                {
                    if (readerText.StartsWith("#"))
                    {
                        ViewFaq.Inlines.Add(new LineBreak());
                        ViewFaq.Inlines.Add(new Bold(new Run(readerText.Replace("# ", string.Empty).Replace("#", string.Empty))));
                        ViewFaq.Inlines.Add(new LineBreak());
                    }
                    else
                    {
                        ViewFaq.Inlines.Add(readerText);
                        ViewFaq.Inlines.Add(new LineBreak());
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (file != null)
            {
                file.SetContent(EditFaq.Text);
            }
            Settings.Default.Save();
            base.OnClosing(e);
        }

        private void EditFaq_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            IsUpdated = true;
        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, System.Windows.Input.ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            // Toogle Mode
            switch (CurrentMode)
            {
                case ViewEditMode.View:
                    CurrentMode = ViewEditMode.ViewAndEdit;
                    break;
                case ViewEditMode.ViewAndEdit:
                    CurrentMode = ViewEditMode.Edit;
                    break;
                case ViewEditMode.Edit:
                    CurrentMode = ViewEditMode.View;
                    break;
            }
            InvalidateViewMode();
        }

        private void InvalidateViewMode()
        {
            // Update Loyout

            switch (CurrentMode)
            {
                case ViewEditMode.Edit:
                    editColumn.MaxWidth = 2000;
                    viewColumn.MaxWidth = 0;
                    break;
                case ViewEditMode.View:
                    editColumn.MaxWidth = 0;
                    viewColumn.MaxWidth = 2000;
                    break;
                case ViewEditMode.ViewAndEdit:
                    editColumn.MaxWidth = 2000;
                    editColumn.MaxWidth = 2000;
                    break;
            }

            Settings.Default.viewMode = (int)CurrentMode;
        }

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                rowCopyright.MaxHeight = 0;
            }
        }
    }
}
