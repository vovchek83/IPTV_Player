using IPTV.DataModel;
using IPTV.DataModel.Models;

namespace IPTV.PlayerControl.ViewModels
{
    public class ChannelViewModel 
    {
        public long Id
        {
            get { return _channel.Id; }
            set { _channel.Id = value; }
        }

        public string Name
        {
            get { return _channel.Name; }
            set { _channel.Name = value; }
        }

        public string Url
        {
            get { return _channel.Url; }
            set { _channel.Url = value; }
        }

        public string Icon
        {
            get { return _channel.Icon; }
            set { _channel.Icon = value; }
        }

        public EChannelGroup GroupName
        {
            get { return _channel.GroupName; }
            set { _channel.GroupName = value; }
        }

        private readonly ChannelModel _channel;

        public ChannelViewModel(ChannelModel channel)
        {
            _channel = channel;
        }
    }
}