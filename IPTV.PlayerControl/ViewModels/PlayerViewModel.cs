using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Data;
using IPTV.Core.Presentation;
using IPTV.Infrastructure.Wrappers;
using IPTV.Logger;
using IPTV.DataModel.Models;

namespace IPTV.PlayerControl.ViewModels
{
    [Export]
    public class PlayerViewModel : ScreenViewModel
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
        public PlayerViewModel(ILogger logger,IChannelsWrapper wrapper)
        {
            Logger = logger;
            Logger.Info("Init PlayerViewModel");
            _wrapper = wrapper;
        }

        protected override void OnActivate()
        {
            Logger.Info("OnAvtivate PlayerViewModel");
            IEnumerable<ChannelModel> chanels = _wrapper.WrappChannels();
            ChannelViewList = CollectionViewSource.GetDefaultView(chanels.Select(channel => new ChannelViewModel(channel)));
        }
    }
}