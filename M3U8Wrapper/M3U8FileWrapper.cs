using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Serialization;
using IPTV.DataModel;
using IPTV.DataModel.Models;
using IPTV.Infrastructure.Services;
using IPTV.Infrastructure.Wrappers;
using IPTV.Logger;
using IPTV_Player.Infrastructure.Services.Interfaces;

namespace M3U8Wrapper
{
    [Export(typeof(IChannelsWrapper))]
    public class M3U8FileWrapper : IChannelsWrapper
    {

        private readonly ILogger _logger;
        private readonly IDownloadService _downloadService;

        [ImportingConstructor]
        public M3U8FileWrapper(ILogger logger)
        {
            _logger = logger;
            _logger.Info("Init M3u8 wrapping...");
            _downloadService = new DownloadService();
        }

        public IEnumerable<ChannelModel> WrappChannels()
        {
            IEnumerable<ChannelModel> retModels = null;
            try
            {
                var file = _downloadService.DownloadFile(
                    "https://edem.tv/playlists/uplist/1c2da6c7d34477003a5b8e8acc9903b7/edem_pl.m3u8", "edem.m3u8");

                if (string.IsNullOrEmpty(file) == false)
                {
                    retModels = ExtractChanlesFromFile(file);
                }

                //TODO message to user
                _logger.Error("failed to get file");

                return retModels;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return null;
            }
        }


        #region Private Methods

        private IEnumerable<ChannelModel> ExtractChanlesFromFile(string filePath)
        {
            IEnumerable<ChannelModel> chanelModelList = null;
            try
            {
                List<string> lines = new List<string>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line) == false)
                        {
                            lines.Add(line);
                        }
                    }
                }

                chanelModelList = ExtractChanels(lines);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return chanelModelList;
        }

        private List<ChannelModel> ExtractChanels(List<string> lines)
        {
            List<ChannelModel> chanelList = new List<ChannelModel>();
            try
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Trim().StartsWith("#EXTINF"))
                    {
                        ChannelModel chanel = new ChannelModel
                        {
                            Name = ExtractName(lines[i++]),
                            GroupName = ExtractGroup(lines[i++]),
                            Url = ExtractUrl(lines[i++]),
                        };

                        chanelList.Add(chanel);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return chanelList;
        }

        private string ExtractIcon(string p)
        {
            string icon = string.Empty;

            try
            {
                int start;
                if ((start = p.IndexOf("tvg-logo=\"", StringComparison.Ordinal)) != -1)
                {

                    string substring = p.Substring(start);

                    start = substring.IndexOf("\"", StringComparison.Ordinal);

                    int end = substring.IndexOf("\"", (start + 1), StringComparison.Ordinal);

                    icon = substring.Substring(start + 1, (end - start - 1));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return icon;
        }

        private string ExtractName(string p)
        {

            string retString = string.Empty;

            try
            {
                int strat;

                if ((strat = p.IndexOf("[COLOR", StringComparison.Ordinal)) != -1)
                {
                    strat = p.IndexOf("]", StringComparison.Ordinal);
                    int end = p.IndexOf("[/COLOR]", StringComparison.Ordinal);

                    retString = p.Substring(strat + 1, end - strat - 1);
                }
                else
                {
                    int firstLocation = p.IndexOf(",", StringComparison.Ordinal);
                    retString = p.Substring(firstLocation + 1).TrimStart();

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }


            return retString.TrimStart().TrimEnd();
        }

        private EChannelGroup ExtractGroup(string p)
        {
            EChannelGroup group = EChannelGroup.None;
            string groupString = string.Empty;
            try
            {
                int start;
                if ((start = p.IndexOf("#EXTGRP:", StringComparison.Ordinal)) != -1)
                {

                    string substring = p.Substring(start);

                    start = substring.IndexOf(":", StringComparison.Ordinal);

                    groupString = substring.Substring(start + 1);
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            if (string.IsNullOrEmpty(groupString) == false)
            {
                if (Enum.TryParse(groupString, out group) == false)
                {
                    group = EChannelGroup.None;
                }
            }

            return group;
        }

        private string ExtractUrl(string p)
        {
            string url = string.Empty;
            try
            {
                if (p.Trim().StartsWith("http") == true)
                {
                    url = p;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            return url;
        }

        #endregion
    }
}
