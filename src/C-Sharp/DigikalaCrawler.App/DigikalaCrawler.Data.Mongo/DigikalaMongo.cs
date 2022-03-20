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

    public void InsertPages(int[] productIds)
    {
        List<DigikalaProduct> pages = new List<DigikalaProduct>();
        pages.AddRange(productIds.Select(x => new DigikalaProduct(x)));
        DigikalaProducts.InsertBatch(pages);
    }

    public IList<DigikalaProduct> GetFreeProduct(int userid, int count)
    {
        List<ObjectId> ids = DigikalaProducts.FindAll().Where(p => (p.Assign && p.UserId == userid && !p.Success)).Take(count).Select(x => x._id).ToList();
        if (ids.Count() < 1)
            ids = DigikalaProducts.FindAll().Where(p => !p.Success && !p.Assign && p.UserId == null).Take(count).Select(x => x._id).ToList();
        var query = Query<DigikalaProduct>.Where(p => p._id.Equals(ids));
        var update = Update<DigikalaProduct>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        DigikalaProducts.Update(query, update);

        return DigikalaProducts.FindAll().Where(p => p._id.Equals(ids)).ToList();
    }

    public void SetProduct(int userid, int productId, ProductData product, CommentDetails comments)
    {
        var query = Query<DigikalaProduct>.EQ(p => p.ProductId, productId);
        var update = Update<DigikalaProduct>
            .Set(p => p.Assign, true)
            .Set(p => p.Success, true)
            .Set(p => p.CrawleDate, DateTime.Now)
            .Set(p => p.ProductData, product)
            .Set(p => p.CommentDetails, comments);
        DigikalaProducts.Update(query, update);
    }
}