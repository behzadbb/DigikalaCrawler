//using System.Collections.Generic;

//namespace DigikalaCrawler.Share.Models
//{
//    public class CommentObjV12
//    {
//        public int status { get; set; }
//        public CommentDetails data { get; set; }
//    }

//    public class CommentDetails2
//    {
//        public List<CommentV1> comments { get; set; }
//        public List<Intent_Data> intent_data { get; set; }
//        public Sort sort { get; set; }
//        public List<Sort_Options> sort_options { get; set; }
//        public List<Media_Comments> media_comments { get; set; }
//        public Pager pager { get; set; }
//    }

//    public class Sort
//    {
//        public string _default { get; set; }
//    }

//    public class Pager
//    {
//        public int current_page { get; set; }
//        public int total_pages { get; set; }
//        public int total_items { get; set; }
//    }

//    public class CommentV1
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string body { get; set; }
//        public string created_at { get; set; }
//        public float rate { get; set; }
//        public Reaction reactions { get; set; }
//        public string recommendation_status { get; set; }
//        public bool is_buyer { get; set; }
//        public string user_name { get; set; }
//        public bool is_anonymous { get; set; }
//        //public Purchased_Item purchased_item { get; set; }
//        //[JsonProperty("advantages")] // and specify possible list of types as string or CompanyModel
//        //[JsonConverter(typeof(AdvantagesJsonConverter))] // add this line
//        public List<string> Advantages { get; set; }
//        public List<string> Disadvantages { get; set; }
//    }
//    //public class AdvantagesJsonConverter : JsonConverter
//    //{

//    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    //    {}

//    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    //    {
//    //        JToken token = JToken.Load(reader);
//    //        if (token.Type == JTokenType.Null || token.Count() == 0)
//    //        {
//    //            return new List<string>();
//    //        }
//    //        if (token.Type == JTokenType.Array)
//    //        {
//    //            return token.ToObject<List<string>>();
//    //        }
//    //        else
//    //        {
//    //            return token.ToObject<List<string>>();
//    //        }
//    //    }

//    //    public override bool CanConvert(Type objectType)
//    //    {return true;}
//    //}

//    public class Reaction
//    {
//        public int likes { get; set; }
//        public int dislikes { get; set; }
//    }

//    public class Intent_Data
//    {
//        public string title { get; set; }
//        public int number_of_comments { get; set; }
//        public Tag_Data tag_data { get; set; }
//        public Tag_Percentage tag_percentage { get; set; }
//        public int productId { get; set; }
//        public int intentId { get; set; }
//    }

//    public class Tag_Data
//    {
//        public int positive { get; set; }
//        public int negative { get; set; }
//        public int neutral { get; set; }
//    }

//    public class Tag_Percentage
//    {
//        public int positive { get; set; }
//        public int negative { get; set; }
//        public int neutral { get; set; }
//    }

//    public class Sort_Options
//    {
//        public string id { get; set; }
//        public string title { get; set; }
//    }

//    public class Media_Comments
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string body { get; set; }
//        public string created_at { get; set; }
//        public float rate { get; set; }
//        public Reactions1 reactions { get; set; }
//        public List<MediaFile> files { get; set; }
//        public string recommendation_status { get; set; }
//        public bool is_buyer { get; set; }
//        public string user_name { get; set; }
//        public bool is_anonymous { get; set; }
//        public object purchased_item { get; set; }
//        public List<string> advantages { get; set; }
//        public List<string> disadvantages { get; set; }
//    }

//    public class Reactions1
//    {
//        public int likes { get; set; }
//        public int dislikes { get; set; }
//    }

//    public class MediaFile
//    {
//        public List<string> storage_ids { get; set; }
//        public List<string> url { get; set; }
//        public List<string> thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//        public object webp_url { get; set; }
//    }
//}
