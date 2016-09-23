namespace IPTV.DataModel.Models
{
    public class ChannelModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public EChannelGroup GroupName { get; set; }
    }
}