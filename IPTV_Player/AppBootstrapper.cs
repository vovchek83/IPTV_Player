using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
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

            //catalog.Catalogs.Add(new AggregateCatalog(new DirectoryCatalog(
            //    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))));
           
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

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();

            assemblies.Add(Assembly.Load("IPTV.PlayerControl"));
            assemblies.Add(Assembly.Load("IPTV_Player"));
            assemblies.Add(Assembly.Load("M3U8Wrapper"));
            assemblies.Add(Assembly.Load("IPTV.Core.Presentation"));
            assemblies.Add(Assembly.Load("IPTV.Logger"));
            assemblies.Add(Assembly.Load("IPTV.Infrastructure"));
            assemblies.Add(Assembly.Load("IPTV.DataModel"));
            return assemblies;
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

                //IShell mainWindow = _container.GetExportedValue<IShell>();
                //IWindowManager windowManager = IoC.Get<IWindowManager>();
                //windowManager.ShowWindow(mainWindow);
                DisplayRootViewFor<IShell>();
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
            }
        }
    }
}