using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using IPTV.Logger;
using IPTV_Player.Infrastructure.Services.Interfaces;
using IPTV_Player.Interfaces;

namespace IPTV_Player.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {

        #region Data Members

        private ILogger _logger;
        private Dictionary<NavigationState, Action<NavigationState>> _navigationStates;
        private NavigationState _navigationState;

        #endregion

        #region Properties

        public NavigationState NavigationState
        {
            get { return _navigationState; }
            private set
            {
                if (value == _navigationState) return;

                _navigationState = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Constractor

        [ImportingConstructor]
        public ShellViewModel(ILogger logger, PlayerViewModel playerViewModel)
        {
            _logger = logger;

            _navigationStates = new Dictionary<NavigationState, Action<NavigationState>>
            {
               [NavigationState.Player] = x => InternalNavigateTo(x, playerViewModel),

            };
        }

        #endregion

        #region Nvigations

        protected override void OnActivate()
        {
            NavigateTo(NavigationState.Player);
        }

        public void NavigateTo(NavigationState state)
        {
            InternalNavigateTo(state);
        }


        private bool InternalNavigateTo(NavigationState state)
        {
            Action<NavigationState> navAction;
            if (!_navigationStates.TryGetValue(state, out navAction))
                return false;

            navAction(state);
            return true;
        }

        private void InternalNavigateTo(NavigationState state, IScreen screen)
        {
            if (NavigationState == state) return;
            _logger.Debug(string.Format("Navigate to {0}",state));
            NavigationState = state;
            ActivateItem(screen);

        }

        #endregion
    }
}