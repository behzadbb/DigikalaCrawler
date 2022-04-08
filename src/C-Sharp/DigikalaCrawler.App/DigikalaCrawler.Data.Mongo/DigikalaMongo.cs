using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Share.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace DigikalaCrawler.Data.Mongo;
public class DigikalaMongo : IDisposable
{
    private MongoClient client;
    private MongoServer server;
    private MongoDatabase db;
    //private MongoCollection<DigikalaProduct> DigikalaProducts;
    private MongoCollection<DigikalaPage> DigikalaPages;
    private MongoCollection<DigikalaProductCrawl> DigikalaProductCrawls;
    private bool disposedValue;

    public DigikalaMongo()
    {
        client = new MongoClient("mongodb://localhost");
        server = client.GetServer();
        db = server.GetDatabase("DigikalaDB");
        //DigikalaProducts = db.GetCollection<DigikalaProduct>("DigikalaProducts");
        DigikalaPages = db.GetCollection<DigikalaPage>("DigikalaPages");
        DigikalaProductCrawls = db.GetCollection<DigikalaProductCrawl>("DigikalaProductCrawls");
    }

    public void InsertPages(List<long> productIds)
    {
        List<DigikalaPage> pages = new List<DigikalaPage>();
        pages.AddRange(productIds.Select(x => new DigikalaPage() { ProductId = x }));
        DigikalaPages.InsertBatch(pages);
    }

    public List<long> GetFreeProduct(int userid, int count, bool checkUserId)
    {
        //Stopwatch sw = new Stopwatch();
        //Console.Clear();
        List<long> ids = new List<long>();
        //sw.Restart();
        if (checkUserId)
        {
            //Console.WriteLine(" T1: " + sw.ElapsedMilliseconds);
            IMongoQuery querySelect1 = Query<DigikalaPage>.Where(p => p.Assign && p.UserId == userid && p.Success == false);
            ids = DigikalaPages.Find(querySelect1).Select(x => x.ProductId).ToList();
        }

        if (ids.Count() < count)
        {
            IMongoQuery querySelect2 = Query<DigikalaPage>.Where(p => !p.Success && !p.Assign && p.UserId == null);
            ids.AddRange(DigikalaPages.Find(querySelect2).SetLimit(count).Select(x => x.ProductId).ToList());
            //Console.Write(" T2: " + sw.ElapsedMilliseconds);
        }
        //var query = Query<DigikalaProduct>.In(p => p.ProductId, ids);
        //var update = Update<DigikalaProduct>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        //DigikalaProducts.Update(query, update, UpdateFlags.Multi);

        Task.Run(() => { UpdateProductsFreeAsync(ids, userid); });

        //UpdateProductsFreeAsync(ids, userid);
        //Console.Write(" T3: " + sw.ElapsedMilliseconds);
        return ids;
    }

    public Task UpdateProductsFreeAsync(List<long> ids, int userid)
    {
        var query = Query<DigikalaPage>.In(p => p.ProductId, ids);
        var update = Update<DigikalaPage>.Set(p => p.Assign, true).Set(p => p.Success, false).Set(p => p.UserId, userid).Set(p => p.AssignDate, DateTime.Now);
        DigikalaPages.Update(query, update, UpdateFlags.Multi);
        return Task.CompletedTask;
    }

    public Task UpdateProductCrawlAsync(long id)
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        var query = Query<DigikalaPage>.EQ(p => p.ProductId, id);
        var update = Update<DigikalaPage>.Set(p => p.Success, true).Set(p => p.CrawleDate, DateTime.Now);
        DigikalaPages.Update(query, update);
        //sw.Stop();
        //Console.WriteLine("\n Update: " + sw.ElapsedMilliseconds + " ms");
        return Task.CompletedTask;
    }

    public Task UpdateProductsCrawlAsync(List<long> ids)
    {
        var query = Query<DigikalaPage>.In(p => p.ProductId, ids);
        var update = Update<DigikalaPage>.Set(p => p.Success, true).Set(p => p.CrawleDate, DateTime.Now);
        DigikalaPages.Update(query, update, UpdateFlags.Multi);
        return Task.CompletedTask;
    }

    //public void SetProduct(SetProductDTO dto)
    //{
    //    var query = Query<DigikalaProduct>.EQ(p => p.ProductId, dto.ProductId);
    //    try
    //    {
    //        var update = Update<DigikalaProduct>
    //        .Set(p => p.Assign, true)
    //        .Set(p => p.Success, true)
    //        .Set(p => p.CrawleDate, DateTime.Now)
    //        .Set(p => p.ProductData, dto.Product)
    //        .Set(p => p.CommentDetails, dto.Comments)
    //        .Set(p => p.Error, dto.Error)
    //        .Set(p => p.ErrorMessage, dto.ErrorMessage)
    //        .Set(p => p.ClientError, dto.ClientError);
    //        DigikalaProducts.Update(query, update);
    //    }
    //    catch (Exception ex)
    //    {
    //        string json = JsonConvert.SerializeObject(dto);
    //        var update = Update<DigikalaProduct>
    //        .Set(p => p.Assign, true)
    //        .Set(p => p.Success, true)
    //        .Set(p => p.CrawleDate, DateTime.Now)
    //        .Set(p => p.Error, true)
    //        .Set(p => p.ErrorMessage, ex.Message)
    //        .Set(p => p.JsonObject, json)
    //        .Set(p => p.ClientError, dto.ClientError)
    //        .Set(p => p.ServerError, true);
    //        DigikalaProducts.Update(query, update);
    //    }
    //}

    public void CreateIndex(params string[] indexs)
    {
        try
        {
            DigikalaPages.DropAllIndexes();
            Thread.Sleep(200);
            DigikalaPages.CreateIndex(indexs);
            Console.WriteLine("Success, Drop Index and Create Index");
        }
        catch
        {
            Console.WriteLine("Error, ReIndex Run");
            DigikalaPages.ReIndex();
        }
    }

    public void DropIndex()
    {
        try
        {
            DigikalaPages.DropAllIndexes();
            Console.WriteLine("Success, Drop Index");
        }
        catch
        {
            Console.WriteLine("Error, Drop Index Run");
        }
    }

    public string ProductCount()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("All: {0}\n", DigikalaPages.Count());
        sb.AppendFormat("Crawl: {0}\n", DigikalaProductCrawls.Count());
        return sb.ToString();
    }

    public void InsertBatchPages(List<DigikalaPage> pages)
    {
        DigikalaPages.InsertBatch(pages);
    }

    public Task InsertDigikalaProductCrawl(DigikalaProductCrawl dto)
    {
        try
        {
            DigikalaProductCrawls.Insert(dto);
        }
        catch (Exception ex)
        {
            DigikalaProductCrawl newObj = new DigikalaProductCrawl();
            newObj.JsonObject = JsonConvert.SerializeObject(dto);
            newObj.ServerError = true;
            newObj.UserId = dto.UserId;
            newObj.CrawleDate = DateTime.Now;
            newObj.ErrorMessage = ex.Message;
            newObj.Success = true;
            newObj.Error = true;
            newObj.ProductId = dto.ProductId;
            DigikalaProductCrawls.Insert(newObj);
        }
        return Task.CompletedTask;
    }

    public Task InsertBatchDigikalaProductCrawl(List<DigikalaProductCrawl> digikalaProductCrawls)
    {
        DigikalaProductCrawls.InsertBatch(digikalaProductCrawls);
        return Task.CompletedTask;
    }

    public long GetDigikalaProductCrawl()
    {
        return DigikalaProductCrawls.Count();
    }

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~DigikalaMongo()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}