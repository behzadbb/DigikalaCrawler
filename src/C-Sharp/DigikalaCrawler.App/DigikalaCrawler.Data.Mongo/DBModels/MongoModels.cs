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