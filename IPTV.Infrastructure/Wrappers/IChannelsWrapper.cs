using System.Collections.Generic;
using IPTV.DataModel.Models;

namespace IPTV.Infrastructure.Wrappers
{
    public interface IChannelsWrapper
    {
        IEnumerable<ChannelModel> WrappChannels();
    }
}