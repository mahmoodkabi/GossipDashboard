using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace GossipDashboard.Helper
{
    public static class VideoHelper
    {

        public static MediaInfo GetMediaInfo(string url)
        {
            try
            {
                var ffProbe = new NReco.VideoInfo.FFProbe();
                var videoInfo = ffProbe.GetMediaInfo(url);
                var res = new MediaInfo();
                res.DurationSecond = videoInfo.Duration.TotalSeconds;
                res.FormatName = videoInfo.FormatName;
                if (videoInfo.Streams != null && videoInfo.Streams.Length > 0)
                {
                    res.Height = videoInfo.Streams[0].Height;
                    res.Width = videoInfo.Streams[0].Width;
                }

                return res;
            }
            catch (Exception)
            {

                return new MediaInfo()
                {
                    DurationSecond = 0,
                    FormatName = "",
                    Width = 0,
                    Height = 0
                };
            }
        }
    }

    public class MediaInfo
    {
        public double DurationSecond { get; set; }
        public string FormatName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}