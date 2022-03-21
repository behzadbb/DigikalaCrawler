using System;
using System.Collections.Generic;
using System.Text;

namespace DigikalaCrawler.Share.Models
{
    public class Page
    {
        public int ProductId { get; set; }
        public int? UserId { get; set; } = null;
        public DateTime? AssignDate { get; set; } = null;
        public DateTime? CrawleDate { get; set; } = null;
        public bool Assign { get; set; } = false;
        public bool Success { get; set; } = false;
    }
}
