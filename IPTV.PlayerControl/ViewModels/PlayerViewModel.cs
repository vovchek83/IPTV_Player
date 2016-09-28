using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Data;
using IPTV.Core.Presentation;
using IPTV.Infrastructure.Wrappers;
using IPTV.Logger;
using IPTV.DataModel.Models;
using IPTV.PlayerControl.Mappers;

namespace IPTV.PlayerControl.ViewModels
{
    [Export]
    public class PlayerViewModel : ScreenViewModel
    {
        #region Data Members

        private readonly IChannelsWrapper _wrapper;
        private ChannelViewModel _selectedChannel;
        private ICollectionView _channelViewList;
        private Uri _currentChannelSource;

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
                if (_selectedChannel != value)
                {
                    _selectedChannel = value;
                    CurrentChannelSource = new Uri(_selectedChannel.Url, UriKind.RelativeOrAbsolute);
                }
                NotifyOfPropertyChange();
            }
        }

        public Uri CurrentChannelSource
        {
            get { return _currentChannelSource; }
            set
            {
                _currentChannelSource = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Ctor

        [ImportingConstructor]
        public PlayerViewModel(ILogger logger, IChannelsWrapper wrapper)
        {
            Logger = logger;
            Logger.Info("Init PlayerViewModel");
            _wrapper = wrapper;
        }

        protected override void OnActivate()
        {
            Logger.Info("OnAvtivate PlayerViewModel");
            LoadChannel();

        }

        #endregion

        #region Private Methodes

        private async void LoadChannel()
        {
            IsBusy = true;
            try
            {
                IEnumerable<ChannelModel> chanels = await _wrapper.WrappChannelsAsync();
                ChannelsList = await ChannelsMapper.Map(chanels);
                ChannelViewList = CollectionViewSource.GetDefaultView(ChannelsList);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

    }
}