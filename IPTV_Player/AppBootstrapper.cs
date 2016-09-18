using System.IO;
using IPTV_Player.Infrastructure;
using IPTV_Player.Infrastructure.Services;
using IPTV_Player.Infrastructure.Services.Interfaces;
using IPTV_Player.ViewModels;

namespace IPTV_Player
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer _container;
        private static bool _logFileDeleted;
        private const string EPG_URL = "http://teleguide.info/download/new3/xmltv.xml.gz";

        private static ILog _logger;
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {

            LogManager.GetLog = type => new FileLogger(type, CanDeleteLogFile);
            _logger = LogManager.GetLog(typeof(AppBootstrapper));
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IDecompressService, DecompressService>();
            _container.Singleton<IDownloadService, DownloadService>();
            _container.PerRequest<IShell, ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var instance = _container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            string appFolder = CreateAppDataFolder();

            IDownloadService downloadService = IoC.Get<IDownloadService>();
            FileInfo file = downloadService.DownloadFile(EPG_URL, appFolder, "xmltv.xml.gz");

            if (file != null)
            {
                IDecompressService decompressService = IoC.Get<IDecompressService>();
                decompressService.Decompress(file);
            }

            DisplayRootViewFor<IShell>();
        }

        private static bool CanDeleteLogFile()
        {
            bool output;
            if (!_logFileDeleted)
            {
                output = true;
                _logFileDeleted = true;
            }
            else
            {
                output = false;
            }

            return output;
        }

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
                _logger.Error(e);
            }
            return path;
        }
    }
}