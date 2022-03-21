using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Share.Models;
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

    public void InsertPages(List<int> productIds)
    {
        List<DigikalaProduct> pages = new List<DigikalaProduct>();
        pages.AddRange(productIds.Select(x => new DigikalaProduct(x)));
        DigikalaProducts.InsertBatch(pages);
    }

    public IList<int> GetFreeProduct(int userid, int count)
    {
        List<int> ids = DigikalaProducts.FindAll().Where(p => (p.Assign && p.UserId == userid && !p.Success)).Take(count).Select(x => x.ProductId).ToList();
        if (ids.Count() < count)
            ids.AddRange(DigikalaProducts.FindAll().Where(p => !p.Success && !p.Assign && p.UserId == null).Take(count - ids.Count).Select(x => x.ProductId).ToList());
        var query = Query<DigikalaProduct>.In(p => p.ProductId, ids);
        var update = Update<DigikalaProduct>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        DigikalaProducts.Update(query, update, UpdateFlags.Multi);

        return ids;
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