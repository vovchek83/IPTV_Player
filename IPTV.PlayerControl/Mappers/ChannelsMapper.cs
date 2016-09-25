using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IPTV.DataModel.Models;
using IPTV.PlayerControl.ViewModels;

namespace IPTV.PlayerControl.Mappers
{
    public static class ChannelsMapper
    {

        public static async Task<List<ChannelViewModel>> Map(IEnumerable<ChannelModel> source)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new List<ChannelViewModel>(source.Select(channel => new ChannelViewModel(channel)));
            });
        }
    }
}