using IPTV.Logger;
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
        private static ILogger _logger;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {

           // LogManager.GetLog = type => new FileLogger(type, CanDeleteLogFile);

            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IDecompressService, DecompressService>();
            _container.Singleton<IDownloadService, DownloadService>();
            _container.PerRequest<IShell, ShellViewModel>();
            _container.Singleton<ILogger, Logger>();
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
            try
            {
                _logger = IoC.Get<ILogger>();
                _logger.Info("Start Application");
                DisplayRootViewFor<IShell>();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
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

    }
}