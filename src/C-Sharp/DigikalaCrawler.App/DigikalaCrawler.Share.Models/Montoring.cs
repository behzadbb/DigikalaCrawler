using System;
using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models
{
    public class Montoring
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public double AvrageCrawling { set; get; } = 0;
        public int LastCommentCount { set; get; } = 0;
        public int TotalCommentCount { set; get; } = 0;
        public int TotalProductCount { set; get; } = 0;
        public double HoursDurration { set; get; } = 0;
        public int CountPerHours { set; get; } = 0;
        public double LoadTimeProducts { set; get; } = 0;
        public double LastCrawlTimeProducts { set; get; } = 0;
        public double LastSendToServerTimeProducts { set; get; } = 0;
        public double Last { set; get; } = 0;
        public long K { set; get; } = 0;
        public List<double> TimeSheet = new List<double>();
    }
}
