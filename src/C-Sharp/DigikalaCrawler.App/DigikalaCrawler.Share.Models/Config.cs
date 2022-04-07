namespace DigikalaCrawler.Share.Models
{
    public struct Config
    {
        public int UserId { get; set; }
        public string Server { get; set; }
        public int Count { get; set; }
        public bool UseProxy { get; set; }
        public string ProxyHost { get; set; }
        public short ProxyPort { get; set; }
        public bool LocalDatabase { get; set; }
    }
}