using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Models;
using DigikalaCrawler.Models.Comments;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DigikalaCrawler.Data.Mongo;
public class DigikalaMongo
{
    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;
    private MongoCollection<DigikalaProduct> DigikalaProducts;

    public DigikalaMongo()
    {
        client = new MongoClient("mongodb://localhost");
        server = client.GetServer();
        db = server.GetDatabase("DigikalaDB");
        DigikalaProducts = db.GetCollection<DigikalaProduct>("DigikalaProducts");
    }
}