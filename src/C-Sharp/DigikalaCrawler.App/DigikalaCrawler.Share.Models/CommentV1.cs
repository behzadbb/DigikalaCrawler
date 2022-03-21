using System;
using System.Collections.Generic;
using System.Text;

namespace DigikalaCrawler.Share.Models
{
    public class CommentObjV1
    {
        public int status { get; set; }
        public CommentDetails data { get; set; }
    }

    public class CommentDetails
    {
        public List<Rate> ratings { get; set; }
        public List<CommentV1> comments { get; set; } = new List<CommentV1>();
        public Media_Comments[] media_comments { get; set; }
        public Pager pager { get; set; }
    }

    public class Pager
    {
        public int current_page { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
    }

    public class Rate
    {
        public int id { get; set; }
        public string title { get; set; }
        public float value { get; set; }
    }

    public class CommentV1
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string created_at { get; set; }
        public float rate { get; set; }
        public Reaction reactions { get; set; }
        public object[] files { get; set; }
        public string recommendation_status { get; set; }
        public bool is_buyer { get; set; }
        public string user_name { get; set; }
        public bool is_anonymous { get; set; }
        public object purchased_item { get; set; }
        public string[] advantages { get; set; }
        public string[] disadvantages { get; set; }
    }

    public class Reaction
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }

    public class Media_Comments
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string created_at { get; set; }
        public float rate { get; set; }
        public Reactions1 reactions { get; set; }
        public FileInfo[] files { get; set; }
        public string recommendation_status { get; set; }
        public bool is_buyer { get; set; }
        public string user_name { get; set; }
        public bool is_anonymous { get; set; }
        public object purchased_item { get; set; }
        public string[] advantages { get; set; }
        public string[] disadvantages { get; set; }
    }

    public class Reactions1
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }

    public class FileInfo
    {
        public string[] storage_ids { get; set; }
        public string[] url { get; set; }
        public string[] thumbnail_url { get; set; }
        public object temporary_id { get; set; }
    }
}
