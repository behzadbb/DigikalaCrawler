using System;
using System.Collections.Generic;
using System.Text;

namespace DigikalaCrawler.Share.Models
{
    public class SetProductDTO
    {
        public long ProductId { get; set; }
        public ProductData Product { get; set; }
        public CommentDetails Comments { get; set; }
    }
    public class SetProductsDTO
    {
        public List<SetProductDTO> Products { get; set; } = new List<SetProductDTO>();
    }
}
