using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<string> _channelsGroup;

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

        public List<string> ChannelsGroup
        {
            get { return _channelsGroup; }
            set
            {
                _channelsGroup = value;
                NotifyOfPropertyChange();
            }
        }

        public int SelectedGroupIndex { get; set; }

        #endregion

        #region Ctor

        [ImportingConstructor]
        public PlayerViewModel(ILogger logger, IChannelsWrapper wrapper)
        {
            Logger = logger;
            Logger.Info("Init PlayerViewModel");
            ChannelsGroup = new List<string>();
            ChannelsGroup.Add("All Channel");
            SelectedGroupIndex = 0;
            _wrapper = wrapper;
        }

        protected override void OnActivate()
        {
            Logger.Info("OnAvtivate PlayerViewModel");
            LoadChannel();

        }

        #endregion

        public void ChannelGroupChanged(object selectedItem)
        {
            ChannelViewList.Filter = item =>
             {
                 ChannelViewModel vitem = item as ChannelViewModel;
                 return (vitem != null) && (vitem.GroupName.ToString() == selectedItem.ToString());
             };
        }

        #region Private Methodes

        private async void LoadChannel()
        {
            IsBusy = true;
            try
            {
                IEnumerable<ChannelModel> chanels = await _wrapper.WrappChannelsAsync();
                if (chanels != null)
                {
                    ChannelsList = await ChannelsMapper.Map(chanels);
                    ChannelViewList = CollectionViewSource.GetDefaultView(ChannelsList);
                    var groups = chanels.GroupBy(x => x.GroupName).Select(y => y.Key.ToString()).ToList();
                    foreach (var group in groups)
                    {
                        ChannelsGroup.Add(group);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

    }
}