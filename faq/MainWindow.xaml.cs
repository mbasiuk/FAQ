using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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
        string FaqFullPath = null;
        const string faqsFileName = "faq.md";
        bool IsUpdated;

        string ExternalValue = null;
        bool IsUpdateExternal = false;

        DispatcherTimer timer;
        FileSystemWatcher watcher;

        public enum ViewEditMode
        {
            View, Edit, ViewAndEdit
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
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            FaqFullPath = Path.Combine(currentDirectory, faqsFileName);
            if (File.Exists(FaqFullPath))
            {
                EditFaq.Text = File.ReadAllText(FaqFullPath);
            }
            else
            {
                File.WriteAllText(FaqFullPath, EditFaq.Text);
            }

            InvalidateView();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            watcher = new FileSystemWatcher(currentDirectory, faqsFileName);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;

        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            using (FileStream fileStream = new FileStream(FaqFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                ExternalValue = streamReader.ReadToEnd();
                IsUpdateExternal = true;
                IsUpdated = true;
            }
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
            if (FaqFullPath != null)
            {
                File.WriteAllText(FaqFullPath, EditFaq.Text);
            }
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
