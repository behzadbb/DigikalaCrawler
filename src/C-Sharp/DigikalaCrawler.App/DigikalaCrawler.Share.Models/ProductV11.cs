using Newtonsoft.Json;
using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models
{

    public class ProductObjV1
    {
        public int status { get; set; }
        public ProductData data { get; set; }
    }
    public class ProductData
    {
        public Product product { get; set; }
        public Seo seo { get; set; }
    }
    public class Product
    {
        public int id { get; set; }
        public string title_fa { get; set; }
        public string title_en { get; set; }
        public Category category { get; set; }
        public Brand brand { get; set; }
        public int questions_count { get; set; }
        public int comments_count { get; set; }
    }
    public class Category
    {
        public int id { get; set; }
        public string title_fa { get; set; }
        public string title_en { get; set; }
        public string code { get; set; }
        public string content_description { get; set; }
    }
    public class Brand
    {
        public int id { get; set; }
        public string code { get; set; }
        public string title_fa { get; set; }
        public string title_en { get; set; }
        public bool visibility { get; set; }
        public bool is_premium { get; set; }
        public bool is_miscellaneous { get; set; }
        public bool is_name_similar { get; set; }
    }
    public class Url1
    {
        public object _base { get; set; }
        public string uri { get; set; }
    }
    public class Seo
    {
        public string title { get; set; }
        public string description { get; set; }
    }
    public class Reviewrating
    {
        public string type { get; set; }
        public int bestRating { get; set; }
        public float ratingValue { get; set; }
        public int worstRating { get; set; }
    }
}