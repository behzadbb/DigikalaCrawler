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
        public List<Intent_Data> intent_data { get; set; }
        public Sort sort { get; set; }
        public List<Sort_Options> sort_options { get; set; }
        public List<Media_Comments> media_comments { get; set; }
        public Pager pager { get; set; }
    }

    public class Sort
    {
        public string _default { get; set; }
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
        public int rate { get; set; }
        public Reaction reactions { get; set; }
        public List<object> files { get; set; }
        public string recommendation_status { get; set; }
        public bool is_buyer { get; set; }
        public string user_name { get; set; }
        public bool is_anonymous { get; set; }
        public object purchased_item { get; set; }
        public List<string> advantages { get; set; }
        public List<string> disadvantages { get; set; }
    }

    public class Reaction
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }

    public class Intent_Data
    {
        public string title { get; set; }
        public int number_of_comments { get; set; }
        public Tag_Data tag_data { get; set; }
        public Tag_Percentage tag_percentage { get; set; }
        public int productId { get; set; }
        public int intentId { get; set; }
    }

    public class Tag_Data
    {
        public int positive { get; set; }
        public int negative { get; set; }
        public int neutral { get; set; }
    }

    public class Tag_Percentage
    {
        public int positive { get; set; }
        public int negative { get; set; }
        public int neutral { get; set; }
    }

    public class Sort_Options
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Media_Comments
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string created_at { get; set; }
        public int rate { get; set; }
        public Reactions1 reactions { get; set; }
        public List<MediaFile> files { get; set; }
        public string recommendation_status { get; set; }
        public bool is_buyer { get; set; }
        public string user_name { get; set; }
        public bool is_anonymous { get; set; }
        public Purchased_Item purchased_item { get; set; }
        public List<string> advantages { get; set; }
        public List<string> disadvantages { get; set; }
    }

    public class Reactions1
    {
        public int likes { get; set; }
        public int dislikes { get; set; }
    }

    public class Purchased_Item
    {
        public Seller seller { get; set; }
        public Color color { get; set; }
    }

    public class Seller
    {
        public int id { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string url { get; set; }
        public Rating rating { get; set; }
        public Properties properties { get; set; }
        public object stars { get; set; }
        public Grade grade { get; set; }
    }

    public class Rating
    {
        public object total_rate { get; set; }
        public int total_count { get; set; }
        public object commitment { get; set; }
        public object no_return { get; set; }
        public object on_time_shipping { get; set; }
    }

    public class Properties
    {
        public bool is_trusted { get; set; }
        public bool is_official { get; set; }
        public bool is_roosta { get; set; }
        public bool is_new { get; set; }
    }

    public class Grade
    {
        public string label { get; set; }
        public string color { get; set; }
    }

    public class Color
    {
        public int id { get; set; }
        public string title { get; set; }
        public string hex_code { get; set; }
    }

    public class MediaFile
    {
        public List<string> storage_ids { get; set; }
        public List<string> url { get; set; }
        public List<string> thumbnail_url { get; set; }
        public object temporary_id { get; set; }
        public object webp_url { get; set; }
    }
}
