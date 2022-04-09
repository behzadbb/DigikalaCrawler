//using System;

//namespace DigikalaCrawler.Share.Models
//{
//    public class ProductObjV1
//    {
//        public int status { get; set; }
//        public ProductData data { get; set; }
//    }

//    public class ProductData
//    {
//        public Product product { get; set; }
//        public Recommendations recommendations { get; set; }
//        public object data_layer { get; set; }
//        public Seo seo { get; set; }
//        public object[] landing_touchpoint { get; set; }
//    }

//    public class Product
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//        public object url { get; set; }
//        public string status { get; set; }
//        public bool has_quick_view { get; set; }
//        public Digiplus digiplus { get; set; }
//        public object data_layer { get; set; }
//        public Images images { get; set; }
//        public Rating rating { get; set; }
//        public Color1[] colors { get; set; }
//        public Default_Variant default_variant { get; set; }
//        public Properties2 properties { get; set; }
//        public object[] badges { get; set; }
//        public Video[] videos { get; set; }
//        public Category category { get; set; }
//        public Brand brand { get; set; }
//        public Review review { get; set; }
//        public Pros_And_Cons pros_and_cons { get; set; }
//        public Suggestion suggestion { get; set; }
//        public Variant[] variants { get; set; }
//        public int concurrent_viewers { get; set; }
//        public int questions_count { get; set; }
//        public int comments_count { get; set; }
//        public Breadcrumb[] breadcrumb { get; set; }
//        public object[] size_guide { get; set; }
//        public Specification[] specifications { get; set; }
//        public Expert_Reviews expert_reviews { get; set; }
//        public Meta meta { get; set; }
//        public object[] fidibo { get; set; }
//        public Last_Comments[] last_comments { get; set; }
//        public Last_Questions[] last_questions { get; set; }
//    }

//    public class Digiplus
//    {
//        public string[] services { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public int cash_back { get; set; }
//        public bool is_general_location_jet_eligible { get; set; }
//    }
//    public class Images
//    {
//        public Main main { get; set; }
//        public List[] list { get; set; }
//    }

//    public class Main
//    {
//        public object[] storage_ids { get; set; }
//        public string[] url { get; set; }
//        public object thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//    }

//    public class List
//    {
//        public object[] storage_ids { get; set; }
//        public string[] url { get; set; }
//        public object thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//    }

//    public class Rating
//    {
//        public int rate { get; set; }
//        public int count { get; set; }
//    }

//    public class Default_Variant
//    {
//        public int id { get; set; }
//        public int lead_time { get; set; }
//        public float rank { get; set; }
//        public int rate { get; set; }
//        public Statistics statistics { get; set; }
//        public string status { get; set; }
//        public Properties properties { get; set; }
//        public Digiplus1 digiplus { get; set; }
//        public Color color { get; set; }
//        public Warranty warranty { get; set; }
//        public Seller seller { get; set; }
//        public Digiclub digiclub { get; set; }
//        public Price price { get; set; }
//        public Shipment_Methods shipment_methods { get; set; }
//    }

//    public class Statistics
//    {
//        public Totally_Satisfied totally_satisfied { get; set; }
//        public Satisfied satisfied { get; set; }
//        public Neutral neutral { get; set; }
//        public Dissatisfied dissatisfied { get; set; }
//        public Totally_Dissatisfied totally_dissatisfied { get; set; }
//        public int total_count { get; set; }
//        public int total_rate { get; set; }
//    }

//    public class Totally_Satisfied
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Satisfied
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Neutral
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Dissatisfied
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Totally_Dissatisfied
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Properties
//    {
//        public bool is_fast_shipping { get; set; }
//        public bool is_ship_by_seller { get; set; }
//        public bool is_multi_warehouse { get; set; }
//        public bool has_similar_variants { get; set; }
//        public bool in_digikala_warehouse { get; set; }
//    }

//    public class Digiplus1
//    {
//        public string[] services { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public int cash_back { get; set; }
//        public bool is_general_location_jet_eligible { get; set; }
//    }

//    public class Color
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string hex_code { get; set; }
//    }

//    public class Warranty
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//    }

//    public class Seller
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string code { get; set; }
//        public string url { get; set; }
//        public Rating1 rating { get; set; }
//        public Properties1 properties { get; set; }
//        public int stars { get; set; }
//        public string registration_date { get; set; }
//    }

//    public class Rating1
//    {
//        public int total_rate { get; set; }
//        public int total_count { get; set; }
//        public int commitment { get; set; }
//        public float no_return { get; set; }
//        public int on_time_shipping { get; set; }
//    }

//    public class Properties1
//    {
//        public bool is_trusted { get; set; }
//        public bool is_official { get; set; }
//        public bool is_roosta { get; set; }
//        public bool is_new { get; set; }
//    }

//    public class Digiclub
//    {
//        public int point { get; set; }
//    }

//    public class Price
//    {
//        public int selling_price { get; set; }
//        public int rrp_price { get; set; }
//        public int order_limit { get; set; }
//        public bool is_incredible { get; set; }
//        public bool is_promotion { get; set; }
//        public bool is_locked_for_digiplus { get; set; }
//        public int discount_percent { get; set; }
//        public int timer { get; set; }
//        public int sold_percentage { get; set; }
//        public Badge badge { get; set; }
//        public bool is_digiplus_promotion { get; set; }
//        public bool is_digiplus_early_access { get; set; }
//        public bool is_application_incredible { get; set; }
//        public bool is_lightening_deal { get; set; }
//    }

//    public class Badge
//    {
//        public string title { get; set; }
//        public string color { get; set; }
//        public object icon { get; set; }
//    }

//    public class Shipment_Methods
//    {
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public Provider[] providers { get; set; }
//    }

//    public class Provider
//    {
//        public string title { get; set; }
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public string type { get; set; }
//    }

//    public class Properties2
//    {
//        public bool is_fast_shipping { get; set; }
//        public bool is_ship_by_seller { get; set; }
//        public bool is_multi_warehouse { get; set; }
//        public bool is_fake { get; set; }
//        public bool has_gift { get; set; }
//        public int min_price_in_last_month { get; set; }
//        public bool is_non_inventory { get; set; }
//        public bool is_ad { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public bool is_medical_supplement { get; set; }
//        public bool has_printed_price { get; set; }
//    }

//    public class Category
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//        public string code { get; set; }
//        public string content_description { get; set; }
//    }

//    public class Brand
//    {
//        public int id { get; set; }
//        public string code { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//        public Url1 url { get; set; }
//        public bool visibility { get; set; }
//        public Logo logo { get; set; }
//        public bool is_premium { get; set; }
//        public bool is_miscellaneous { get; set; }
//        public bool is_name_similar { get; set; }
//    }

//    public class Url1
//    {
//        public object _base { get; set; }
//        public string uri { get; set; }
//    }

//    public class Logo
//    {
//        public object[] storage_ids { get; set; }
//        public string[] url { get; set; }
//        public object thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//    }

//    public class Review
//    {
//        public string description { get; set; }
//        public Attribute[] attributes { get; set; }
//    }

//    public class Attribute
//    {
//        public string title { get; set; }
//        public string[] values { get; set; }
//    }

//    public class Pros_And_Cons
//    {
//        public object[] advantages { get; set; }
//        public object[] disadvantages { get; set; }
//    }

//    public class Suggestion
//    {
//        public int count { get; set; }
//        public int percentage { get; set; }
//    }

//    public class Expert_Reviews
//    {
//        public object[] attributes { get; set; }
//        public string description { get; set; }
//        public string short_review { get; set; }
//        public object[] admin_rates { get; set; }
//        public Review_Sections[] review_sections { get; set; }
//        public Technical_Properties technical_properties { get; set; }
//    }

//    public class Technical_Properties
//    {
//        public object[] advantages { get; set; }
//        public object[] disadvantages { get; set; }
//    }

//    public class Review_Sections
//    {
//        public string title { get; set; }
//        public Section[] sections { get; set; }
//    }

//    public class Section
//    {
//        public string template { get; set; }
//        public string text { get; set; }
//        public string image { get; set; }
//    }

//    public class Meta
//    {
//        public Brand_Category_Url brand_category_url { get; set; }
//    }

//    public class Brand_Category_Url
//    {
//        public object _base { get; set; }
//        public string uri { get; set; }
//    }

//    public class Color1
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string hex_code { get; set; }
//    }

//    public class Video
//    {
//        public string title { get; set; }
//        public string url { get; set; }
//        public string cover { get; set; }
//    }

//    public class Variant
//    {
//        public int id { get; set; }
//        public int lead_time { get; set; }
//        public float rank { get; set; }
//        public int rate { get; set; }
//        public Statistics1 statistics { get; set; }
//        public string status { get; set; }
//        public Properties3 properties { get; set; }
//        public Digiplus2 digiplus { get; set; }
//        public Color2 color { get; set; }
//        public Warranty1 warranty { get; set; }
//        public Seller1 seller { get; set; }
//        public Digiclub1 digiclub { get; set; }
//        public Price1 price { get; set; }
//        public Shipment_Methods1 shipment_methods { get; set; }
//    }

//    public class Statistics1
//    {
//        public Totally_Satisfied1 totally_satisfied { get; set; }
//        public Satisfied1 satisfied { get; set; }
//        public Neutral1 neutral { get; set; }
//        public Dissatisfied1 dissatisfied { get; set; }
//        public Totally_Dissatisfied1 totally_dissatisfied { get; set; }
//        public int total_count { get; set; }
//        public int total_rate { get; set; }
//    }

//    public class Totally_Satisfied1
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Satisfied1
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Neutral1
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Dissatisfied1
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Totally_Dissatisfied1
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Properties3
//    {
//        public bool is_fast_shipping { get; set; }
//        public bool is_ship_by_seller { get; set; }
//        public bool is_multi_warehouse { get; set; }
//        public bool has_similar_variants { get; set; }
//        public bool in_digikala_warehouse { get; set; }
//    }

//    public class Digiplus2
//    {
//        public string[] services { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public int cash_back { get; set; }
//        public bool is_general_location_jet_eligible { get; set; }
//    }

//    public class Color2
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string hex_code { get; set; }
//    }

//    public class Warranty1
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//    }

//    public class Seller1
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string code { get; set; }
//        public string url { get; set; }
//        public Rating2 rating { get; set; }
//        public Properties4 properties { get; set; }
//        public float stars { get; set; }
//        public string registration_date { get; set; }
//    }

//    public class Rating2
//    {
//        public int total_rate { get; set; }
//        public int total_count { get; set; }
//        public int commitment { get; set; }
//        public float no_return { get; set; }
//        public int on_time_shipping { get; set; }
//    }


//    public class Properties4
//    {
//        public bool is_trusted { get; set; }
//        public bool is_official { get; set; }
//        public bool is_roosta { get; set; }
//        public bool is_new { get; set; }
//    }

//    public class Digiclub1
//    {
//        public int point { get; set; }
//    }

//    public class Price1
//    {
//        public int selling_price { get; set; }
//        public int rrp_price { get; set; }
//        public int order_limit { get; set; }
//        public bool is_incredible { get; set; }
//        public bool is_promotion { get; set; }
//        public bool is_locked_for_digiplus { get; set; }
//        public int discount_percent { get; set; }
//        public int timer { get; set; }
//        public Badge1 badge { get; set; }
//        public bool is_digiplus_promotion { get; set; }
//        public bool is_digiplus_early_access { get; set; }
//        public bool is_application_incredible { get; set; }
//        public bool is_lightening_deal { get; set; }
//        public int sold_percentage { get; set; }
//        public int marketable_stock { get; set; }
//    }

//    public class Badge1
//    {
//        public string title { get; set; }
//        public string color { get; set; }
//        public object icon { get; set; }
//    }

//    public class Shipment_Methods1
//    {
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public Provider1[] providers { get; set; }
//    }

//    public class Provider1
//    {
//        public string title { get; set; }
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public string type { get; set; }
//    }

//    public class Breadcrumb
//    {
//        public string title { get; set; }
//        public Url2 url { get; set; }
//    }

//    public class Url2
//    {
//        public object _base { get; set; }
//        public string uri { get; set; }
//    }

//    public class Specification
//    {
//        public string title { get; set; }
//        public Attribute1[] attributes { get; set; }
//    }

//    public class Attribute1
//    {
//        public string title { get; set; }
//        public string[] values { get; set; }
//    }

//    public class Last_Comments
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string body { get; set; }
//        public string created_at { get; set; }
//        public float rate { get; set; }
//        public Reactions reactions { get; set; }
//        public object[] files { get; set; }
//        public string recommendation_status { get; set; }
//        public bool is_buyer { get; set; }
//        public string user_name { get; set; }
//        public bool is_anonymous { get; set; }
//        public object[] purchased_item { get; set; }
//        public string[] advantages { get; set; }
//        public string[] disadvantages { get; set; }
//    }

//    public class Reactions
//    {
//        public int likes { get; set; }
//        public int dislikes { get; set; }
//    }

//    public class Last_Questions
//    {
//        public int id { get; set; }
//        public string text { get; set; }
//        public int answer_count { get; set; }
//        public string sender { get; set; }
//        public string created_at { get; set; }
//    }

//    public class Recommendations
//    {
//        public Also_Bought_Products also_bought_products { get; set; }
//        public Related_Products related_products { get; set; }
//    }

//    public class Also_Bought_Products
//    {
//        public string title { get; set; }
//        public object discount_percent { get; set; }
//        public object see_more_url { get; set; }
//        public Product1[] products { get; set; }
//        public object background { get; set; }
//        public object icon { get; set; }
//        public object products_count { get; set; }
//        public object data_layer { get; set; }
//    }

//    public class Product1
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//        public Url3 url { get; set; }
//        public string status { get; set; }
//        public bool has_quick_view { get; set; }
//        public Digiplus3 digiplus { get; set; }
//        public object data_layer { get; set; }
//        public Images1 images { get; set; }
//        public Default_Variant1 default_variant { get; set; }
//    }

//    public class Url3
//    {
//        public object _base { get; set; }
//        public string uri { get; set; }
//    }

//    public class Digiplus3
//    {
//        public string[] services { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public int cash_back { get; set; }
//        public bool is_general_location_jet_eligible { get; set; }
//    }

//    public class Images1
//    {
//        public Main1 main { get; set; }
//    }

//    public class Main1
//    {
//        public object[] storage_ids { get; set; }
//        public string[] url { get; set; }
//        public object thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//    }

//    public class Default_Variant1
//    {
//        public int id { get; set; }
//        public int lead_time { get; set; }
//        public float rank { get; set; }
//        public float rate { get; set; }
//        public object statistics { get; set; }
//        public string status { get; set; }
//        public object properties { get; set; }
//        public object digiplus { get; set; }
//        public object color { get; set; }
//        public object warranty { get; set; }
//        public Seller2 seller { get; set; }
//        public Digiclub2 digiclub { get; set; }
//        public Price2 price { get; set; }
//        public Shipment_Methods2 shipment_methods { get; set; }
//    }

//    public class Totally_Satisfied2
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Satisfied2
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Neutral2
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Dissatisfied2
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Totally_Dissatisfied2
//    {
//        public int rate_count { get; set; }
//        public float rate { get; set; }
//    }

//    public class Seller2
//    {
//        public int id { get; set; }
//        public string title { get; set; }
//        public string code { get; set; }
//        public string url { get; set; }
//        public Rating3 rating { get; set; }
//        public Properties6 properties { get; set; }
//        public float stars { get; set; }
//        public string registration_date { get; set; }
//    }

//    public class Rating3
//    {
//        public int total_rate { get; set; }
//        public int total_count { get; set; }
//        public float commitment { get; set; }
//        public float no_return { get; set; }
//        public int on_time_shipping { get; set; }
//    }

//    public class Properties6
//    {
//        public bool is_trusted { get; set; }
//        public bool is_official { get; set; }
//        public bool is_roosta { get; set; }
//        public bool is_new { get; set; }
//    }

//    public class Digiclub2
//    {
//        public int point { get; set; }
//    }

//    public class Price2
//    {
//        public int selling_price { get; set; }
//        public int rrp_price { get; set; }
//        public int order_limit { get; set; }
//        public bool is_incredible { get; set; }
//        public bool is_promotion { get; set; }
//        public bool is_locked_for_digiplus { get; set; }
//        public int marketable_stock { get; set; }
//        public int discount_percent { get; set; }
//        public Badge2 badge { get; set; }
//        public bool is_digiplus_promotion { get; set; }
//        public bool is_digiplus_early_access { get; set; }
//        public bool is_application_incredible { get; set; }
//        public bool is_lightening_deal { get; set; }
//        public int sold_percentage { get; set; }
//    }

//    public class Badge2
//    {
//        public string title { get; set; }
//        public string color { get; set; }
//        public Icon icon { get; set; }
//    }

//    public class Icon
//    {
//        public object[] storage_ids { get; set; }
//        public string[] url { get; set; }
//        public object thumbnail_url { get; set; }
//        public object temporary_id { get; set; }
//    }

//    public class Shipment_Methods2
//    {
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public Provider2[] providers { get; set; }
//    }

//    public class Provider2
//    {
//        public string title { get; set; }
//        public string description { get; set; }
//        public bool has_lead_time { get; set; }
//        public string type { get; set; }
//    }

//    public class Related_Products
//    {
//        public string title { get; set; }
//        public object discount_percent { get; set; }
//        public object see_more_url { get; set; }
//        public Product2[] products { get; set; }
//        public object background { get; set; }
//        public object icon { get; set; }
//        public object products_count { get; set; }
//        public object data_layer { get; set; }
//    }

//    public class Product2
//    {
//        public int id { get; set; }
//        public string title_fa { get; set; }
//        public string title_en { get; set; }
//        public object url { get; set; }
//        public string status { get; set; }
//        public bool has_quick_view { get; set; }
//        public Digiplus5 digiplus { get; set; }
//        public Data_Layer2 data_layer { get; set; }
//        public object images { get; set; }
//        public object default_variant { get; set; }
//    }

//    public class Digiplus5
//    {
//        public string[] services { get; set; }
//        public bool is_jet_eligible { get; set; }
//        public int cash_back { get; set; }
//        public bool is_general_location_jet_eligible { get; set; }
//    }

//    public class Data_Layer2
//    {
//        public string brand { get; set; }
//        public string category { get; set; }
//        public int metric6 { get; set; }
//        public int dimension2 { get; set; }
//        public int dimension6 { get; set; }
//        public string dimension7 { get; set; }
//        public float dimension9 { get; set; }
//        public int dimension11 { get; set; }
//        public string dimension20 { get; set; }
//    }
//    public class Seo
//    {
//        public string title { get; set; }
//        public string description { get; set; }
//        public Twitter_Card twitter_card { get; set; }
//        public Open_Graph open_graph { get; set; }
//        public Header header { get; set; }
//        public Markup_Schema markup_schema { get; set; }
//        public Category_Breadcrumb[] category_breadcrumb { get; set; }
//        public Brand_Breadcrumb[] brand_breadcrumb { get; set; }
//    }

//    public class Twitter_Card
//    {
//        public string title { get; set; }
//        public string image { get; set; }
//        public int price { get; set; }
//        public string description { get; set; }
//    }

//    public class Open_Graph
//    {
//        public string title { get; set; }
//        public string url { get; set; }
//        public string image { get; set; }
//        public string availability { get; set; }
//        public string type { get; set; }
//        public string site { get; set; }
//        public int price { get; set; }
//        public object description { get; set; }
//    }

//    public class Header
//    {
//        public string title { get; set; }
//        public string description { get; set; }
//        public string canonical_url { get; set; }
//    }

//    public class Markup_Schema
//    {
//        public string context { get; set; }
//        public string type { get; set; }
//        public string name { get; set; }
//        public string alternateName { get; set; }
//        public string[] image { get; set; }
//        public string description { get; set; }
//        public int mpn { get; set; }
//        public int sku { get; set; }
//        public string category { get; set; }
//        public Brand1 brand { get; set; }
//        public Aggregaterating aggregateRating { get; set; }
//        public Offers offers { get; set; }
//        public Review1 review { get; set; }
//    }

//    public class Brand1
//    {
//        public string type { get; set; }
//        public string name { get; set; }
//        public string url { get; set; }
//        public string id { get; set; }
//    }

//    public class Aggregaterating
//    {
//        public string type { get; set; }
//        public float ratingValue { get; set; }
//        public int reviewCount { get; set; }
//    }

//    public class Offers
//    {
//        public string type { get; set; }
//        public string priceCurrency { get; set; }
//        public int price { get; set; }
//        public string itemCondition { get; set; }
//        public string availability { get; set; }
//    }

//    public class Review1
//    {
//        public string type { get; set; }
//        public string author { get; set; }
//        public string datePublished { get; set; }
//        public string reviewBody { get; set; }
//        public string name { get; set; }
//        public Reviewrating reviewRating { get; set; }
//    }

//    public class Reviewrating
//    {
//        public string type { get; set; }
//        public int bestRating { get; set; }
//        public float ratingValue { get; set; }
//        public int worstRating { get; set; }
//    }

//    public class Category_Breadcrumb
//    {
//        public string title { get; set; }
//        public string url { get; set; }
//    }

//    public class Brand_Breadcrumb
//    {
//        public string title { get; set; }
//        public string url { get; set; }
//    }
//}
