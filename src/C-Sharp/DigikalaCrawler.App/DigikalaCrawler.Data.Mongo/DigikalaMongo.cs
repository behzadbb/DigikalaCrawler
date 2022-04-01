using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Share.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System.Text;

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

    public void InsertPages(List<long> productIds)
    {
        List<DigikalaProduct> pages = new List<DigikalaProduct>();
        pages.AddRange(productIds.Select(x => new DigikalaProduct(x)));
        DigikalaProducts.InsertBatch(pages);
    }

    public IList<long> GetFreeProduct(int userid, int count, bool checkUserId)
    {
        List<long> ids = new List<long>();

        if (checkUserId)
        {
            IMongoQuery querySelect1 = Query<DigikalaProduct>.Where(p => p.Assign && p.UserId == userid && p.Success == false);
            ids = DigikalaProducts.Find(querySelect1).Select(x => x.ProductId).ToList();
        }

        if (ids.Count() < count)
        {
            IMongoQuery querySelect2 = Query<DigikalaProduct>.Where(p => !p.Success && !p.Assign && p.UserId == null);
            ids.AddRange(DigikalaProducts.Find(querySelect2).SetLimit(count - ids.Count).Select(x => x.ProductId).ToList());
        }
        //var query = Query<DigikalaProduct>.In(p => p.ProductId, ids);
        //var update = Update<DigikalaProduct>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        //DigikalaProducts.Update(query, update, UpdateFlags.Multi);

        //Task.Run(() => { UpdateProductsAsync(ids, userid); });

        UpdateProductsAsync(ids, userid);
        return ids;
    }
    private void UpdateProductsAsync(List<long> ids, int userid)
    {
        var query = Query<DigikalaProduct>.In(p => p.ProductId, ids);
        var update = Update<DigikalaProduct>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        DigikalaProducts.Update(query, update, UpdateFlags.Multi);
    }

    public void SetProduct(SetProductDTO dto)
    {
        var query = Query<DigikalaProduct>.EQ(p => p.ProductId, dto.ProductId);
        try
        {
            var update = Update<DigikalaProduct>
            .Set(p => p.Assign, true)
            .Set(p => p.Success, true)
            .Set(p => p.CrawleDate, DateTime.Now)
            .Set(p => p.ProductData, dto.Product)
            .Set(p => p.CommentDetails, dto.Comments)
            .Set(p => p.Error, dto.Error)
            .Set(p => p.ErrorMessage, dto.ErrorMessage)
            .Set(p => p.ClientError, dto.ClientError);
            DigikalaProducts.Update(query, update);
        }
        catch (Exception ex)
        {
            string json = JsonConvert.SerializeObject(dto);
            var update = Update<DigikalaProduct>
            .Set(p => p.Assign, true)
            .Set(p => p.Success, true)
            .Set(p => p.CrawleDate, DateTime.Now)
            .Set(p => p.Error, true)
            .Set(p => p.ErrorMessage, ex.Message)
            .Set(p => p.JsonObject, json)
            .Set(p => p.ClientError, dto.ClientError)
            .Set(p => p.ServerError, true);
            DigikalaProducts.Update(query, update);
        }
    }

    public void CreateIndex(params string[] indexs)
    {
        DigikalaProducts.CreateIndex(indexs);
    }

    public string ProductCount()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("All: {0}\n", DigikalaProducts.Count());
        IMongoQuery query1 = Query<DigikalaProduct>.Where(p => p.Success);
        sb.AppendFormat("Crawl: {0}\n", DigikalaProducts.Find(query1).Count());
        IMongoQuery query2 = Query<DigikalaProduct>.Where(p => p.Error);
        sb.AppendFormat("Crawl Error: {0}\n", DigikalaProducts.Find(query2).Count());
        IMongoQuery query3 = Query<DigikalaProduct>.Where(p => p.Error && p.ClientError);
        sb.AppendFormat("Client Error: {0}\n", DigikalaProducts.Find(query3).Count());
        IMongoQuery query4 = Query<DigikalaProduct>.Where(p => p.Error && p.ClientError);
        sb.AppendFormat("Server Error: {0}\n", DigikalaProducts.Find(query4).Count());
        return sb.ToString();
    }
}