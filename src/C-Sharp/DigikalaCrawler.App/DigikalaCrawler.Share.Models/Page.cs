using System;

namespace DigikalaCrawler.Share.Models
{
    public class Page
    {
        public long ProductId { get; set; }
        public int? UserId { get; set; } = null;
        public DateTime? AssignDate { get; set; } = null;
        public DateTime? CrawleDate { get; set; } = null;
        public bool Assign { get; set; } = false;
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string JsonObject { get; set; }
        public bool ClientError { get; set; } = false;
        public bool ServerError { get; set; } = false;
    }
}
