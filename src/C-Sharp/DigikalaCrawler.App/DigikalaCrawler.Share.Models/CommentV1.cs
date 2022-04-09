using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models
{
    public class CommentObjV1
    {
        public int status { get; set; }
        public CommentDetails data { get; set; }
    }

    public class CommentDetails
    {
        public List<CommentV1> comments { get; set; }
        public Pager pager { get; set; }
    }

    public class Pager
    {
        public int current_page { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
    }

    public class CommentV1
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string created_at { get; set; }
        public float rate { get; set; }
        public Reaction reactions { get; set; }
        public string recommendation_status { get; set; }
        public bool is_buyer { get; set; }
        public string user_name { get; set; }
        public bool is_anonymous { get; set; }
        public List<string> advantages { get; set; }
        public List<string> disadvantages { get; set; }
    }

    public class Reaction
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }
}
