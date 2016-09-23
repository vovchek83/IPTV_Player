using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using IPTV.Infrastructure.Wrappers;
using IPTV.Logger;
using IPTV.PlayerControl.ViewModels;
using M3U8Wrapper;

namespace IPTV_Player.ViewModels
{
    [Export]
    public class PlayerViewModel : Screen
    {
        #region Data Members

        private readonly IChannelsWrapper _wrapper;

        private ChannelViewModel _selectedChannel;
        private ICollectionView _channelViewList;

        #endregion

        #region Properties

        public List<ChannelViewModel> ChannelsList { get; set; }

        public ICollectionView ChannelViewList
        {
            get { return _channelViewList; }
            set
            {
                _channelViewList = value;
                NotifyOfPropertyChange();
            }
        }

        public ChannelViewModel SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion
        [ImportingConstructor]
        public PlayerViewModel(ILogger logger, IChannelsWrapper wrapper)
        {

            logger.Info("Init PlayerViewModel");
            _wrapper = wrapper;

        }

        protected override void OnActivate()
        {
           // IPTV.Logger.Info("OnAvtivate PlayerViewModel");
            var chanels = _wrapper.WrappChannels();
        }
    }
}