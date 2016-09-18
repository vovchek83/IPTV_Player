using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Caliburn.Micro;
using IPTV_Player.Infrastructure.Services.Interfaces;
using System.ComponentModel;

namespace IPTV_Player.Infrastructure.Services
{
    public class DownloadService : IDownloadService
    {
        private static readonly ILog Logger = LogManager.GetLog(typeof(DownloadService));

        public void DaownloadFileAsync(string url, string directory, string fileName)
        {
            string path = Path.Combine(directory, fileName);
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(url), @path);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Logger.Info(string.Format("Downlowded {0}%",e.ProgressPercentage));
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Logger.Info("Download Complate");
        }

        public FileInfo DownloadFile(string url, string directory, string fileName)
        {
            FileInfo fileInfo = null;
            try
            {
                WebClient client = new WebClient();
                string path = Path.Combine(directory, fileName);
                client.DownloadFile(url,path);
                fileInfo = new FileInfo(path);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return fileInfo;
        }
    }
}