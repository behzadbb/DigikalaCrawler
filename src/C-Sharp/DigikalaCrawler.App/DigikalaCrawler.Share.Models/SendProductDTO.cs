using DigikalaCrawler.Share.Models.Question;
using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models
{
    public class SetProductDTO
    {
        public long ProductId { get; set; }
        public ProductData Product { get; set; }
        public CommentData CommentData { get; set; }
        public Questions Questions { get; set; }
        public bool Error { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string JsonObject { get; set; }
        public bool ClientError { get; set; } = false;
        public int CommentsCount { get; set; } = 0;
        public int SendCommentsCount { get; set; } = 0;
        public int QuestionsCount { get; set; } = 0;
    }
    public class SetProductsDTO
    {
        public List<SetProductDTO> Products { get; set; } = new List<SetProductDTO>();
        public int UserId { get; set; }
    }
}
