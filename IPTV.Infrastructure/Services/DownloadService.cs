using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using Caliburn.Micro;
using IPTV_Player.Infrastructure.Services.Interfaces;

namespace IPTV.Infrastructure.Services
{
    [Export(typeof(IDownloadService))]
    public class DownloadService : IDownloadService
    {
        private static readonly ILog Logger = LogManager.GetLog(typeof(DownloadService));

        private string _appDataFolder;

        public string ApplicationDataFolder
        {
            get
            {
                if (_appDataFolder == null)
                {
                    _appDataFolder = CreateAppDataFolder();
                }
                return _appDataFolder;
            }
        }

        public string DownloadFile(string url, string fileName)
        {
            string path = null;
            try
            {
                path = Path.Combine(ApplicationDataFolder, fileName);
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, path);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return path;
        }

        public void DaownloadFileAsync(string url, string fileName)
        {
            string path = Path.Combine(ApplicationDataFolder, fileName);
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(url), @path);
        }

        #region Events Handlers

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Logger.Info(string.Format("Downlowded {0}%", e.ProgressPercentage));
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Logger.Info("Download Complate");
        }

        #endregion

        #region Private Methods

        private string CreateAppDataFolder()
        {
            string path = null;
            try
            {
                var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(appDataPath, "IPTV_Player");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return path;
        }

        #endregion

    }
}