using System;
using System.IO;

namespace Faq
{
    class FaqFile
    {
        FileInfo file;
        FileSystemWatcher watcher;

        public FaqFile(FileInfo file)
        {
            this.file = file;

            if (!file.Exists)
            {
                file.Create();
            }

            watcher = new FileSystemWatcher(file.DirectoryName, file.Name);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }

        public FaqFile(string fullPath) : this(new FileInfo(fullPath))
        {
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string updatedFileContent;
            bool isSuccessRead = false;
            if (ContentChanged != null)
            {
                using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    updatedFileContent = streamReader.ReadToEnd();
                    isSuccessRead = true;
                }
                if (isSuccessRead)
                    ContentChanged(updatedFileContent);
            }
        }

        public event Action<string> ContentChanged;

        public string GetContent()
        {
            return File.ReadAllText(file.FullName);
        }

        public void SetContent(string content)
        {
            File.WriteAllText(file.FullName, content);
        }

    }
}
