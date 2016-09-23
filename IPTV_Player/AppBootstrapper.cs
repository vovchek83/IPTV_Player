using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using IPTV.Logger;
using IPTV_Player.Infrastructure.Services;
using IPTV_Player.Infrastructure.Services.Interfaces;
using IPTV_Player.Interfaces;
using IPTV_Player.ViewModels;

namespace IPTV_Player
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using System.IO;

    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;
       
        private static ILogger _logger;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            catalog.Catalogs.Add(new AggregateCatalog(new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))));
            // LogManager.GetLog = type => new FileLogger(type, CanDeleteLogFile);
            _container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();


            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<ILogger>(new Logger());

            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }


        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            try
            {
                _logger = IoC.Get<ILogger>();
                _logger.Info("Start Application");

                IShell mainWindow = _container.GetExportedValue<IShell>();
                IWindowManager windowManager = IoC.Get<IWindowManager>();
                windowManager.ShowWindow(mainWindow);
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
            }
        }
    }
}