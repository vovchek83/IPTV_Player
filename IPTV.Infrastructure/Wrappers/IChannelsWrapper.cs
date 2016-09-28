using System.Collections.Generic;
using System.Threading.Tasks;
using IPTV.DataModel.Models;

namespace IPTV.Infrastructure.Wrappers
{
    public interface IChannelsWrapper
    {
        IEnumerable<ChannelModel> WrappChannels();
        Task<IEnumerable<ChannelModel>> WrappChannelsAsync();
    }
}