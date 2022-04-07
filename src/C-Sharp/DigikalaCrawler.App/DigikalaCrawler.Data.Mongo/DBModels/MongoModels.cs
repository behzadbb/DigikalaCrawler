using DigikalaCrawler.Share.Models;
using MongoDB.Bson;

namespace DigikalaCrawler.Data.Mongo.DBModels;
public class DigikalaProduct : Page
{
    public ObjectId _id { get; set; }
    public CommentDetails CommentDetails { get; set; }
    public ProductData ProductData { get; set; }

    public DigikalaProduct()
    {

    }
    public DigikalaProduct(long productId)
    {
        this.ProductId = productId;
    }
}
public class DigikalaProductCrawl
{
    public ObjectId _id { get; set; }
    public long ProductId { get; set; }
    public int UserId { get; set; }
    public DateTime CrawleDate { get; set; }
    public bool Success { get; set; } = false;
    public bool Error { get; set; } = false;
    public string ErrorMessage { get; set; }
    public string JsonObject { get; set; }
    public bool ClientError { get; set; } = false;
    public bool ServerError { get; set; } = false;
    public CommentDetails CommentDetails { get; set; }
    public ProductData ProductData { get; set; }
}
public class DigikalaPage
{
    public ObjectId _id { get; set; }
    public long ProductId { get; set; }
    public int? UserId { get; set; } = null;
    public DateTime? AssignDate { get; set; } = null;
    public DateTime? CrawleDate { get; set; } = null;
    public bool Assign { get; set; } = false;
    public bool Success { get; set; } = false;
}