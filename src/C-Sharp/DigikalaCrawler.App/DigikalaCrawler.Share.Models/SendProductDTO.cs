using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models
{
    public class SetProductDTO
    {
        public long ProductId { get; set; }
        public ProductData Product { get; set; }
        public CommentDetails Comments { get; set; }
        public bool Error { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string JsonObject { get; set; }
        public bool ClientError { get; set; } = false;
    }
    public class SetProductsDTO
    {
        public List<SetProductDTO> Products { get; set; } = new List<SetProductDTO>();
        public int UserId { get; set; }
    }
}
