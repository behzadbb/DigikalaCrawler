using DigikalaCrawler.Models;
using DigikalaCrawler.Models.Comments;
using MongoDB.Bson;

namespace DigikalaCrawler.Data.Mongo.DBModels;
public class DigikalaProduct : Page
{
    public ObjectId _id { get; set; }
    public CommentDetails CommentDetails { get; set; }
    public ProductData ProductData { get; set; }
}