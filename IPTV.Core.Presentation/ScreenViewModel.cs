using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using IPTV.Logger;

namespace IPTV.Core.Presentation
{
    [Export]
    public class ScreenViewModel : Screen, IDisposable
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        [Import]
        public ILogger Logger { get; set; }

        [Import]
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// you shouldnt override this method, instead override the <see cref="DisposeImp"/>
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            DisposeImp();
        }


        /// <summary>
        /// Add all your disposable logic here
        /// </summary>
        public virtual void DisposeImp()
        {
            //should beimplmented in derived classes
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~ScreenViewModel()
        {
            Dispose(false);
        }
    }
}