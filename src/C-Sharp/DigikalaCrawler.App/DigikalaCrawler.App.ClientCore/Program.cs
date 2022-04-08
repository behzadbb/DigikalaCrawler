using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;
using System.Diagnostics;

Montoring monitoring = new Montoring();
Console.WriteLine("Start: {0}", DateTime.Now);
Thread.Sleep(1000);
Config _config;
loadConfig();
var checkUserId = true;
Console.WriteLine($"#_ \t| ALL \t\t| Lst \t| Lod \t| Crwl \t| Snd \t| Avg \t| CM \t| CMs \t| H: \t| CmH: ____");

Stopwatch sw = new Stopwatch();
while (!string.IsNullOrEmpty(_config.Server))
{
    sw.Restart();
    using (DigikalaCrawlerServiceV1 digi = new DigikalaCrawlerServiceV1(_config))
    {
        List<long> ids = await digi.GetFreeProductsFromServer(checkUserId);
        monitoring.LoadTimeProducts = Math.Round((double)sw.ElapsedMilliseconds / 1000, 1);
        int random = new Random().Next(3, 50);
        Thread.Sleep(random * 10);
        if (ids != null && ids.Any())
        {
            checkUserId = false;
            SetProductsDTO products = new SetProductsDTO();
            for (int i = 0; i < ids.Count(); i++)
            {
                var product = new SetProductDTO();
                product.ProductId = ids[i];
                try
                {
                    product.Product = digi.GetProduct(ids[i]).Result;
                    Thread.Sleep(random);
                    if (product.Product != null && product.Product.product.comments_count > 0)
                    {
                        product.Comments = digi.GetProductComments(ids[i]).Result;
                    }
                }
                catch (Exception ex)
                {
                    product.Error = true;
                    product.ErrorMessage = ex.Message;
                    product.ClientError = true;
                }
                products.Products.Add(product);
            }
            monitoring.LastCrawlTimeProducts = Math.Round((double)sw.ElapsedMilliseconds / 1000 - monitoring.LoadTimeProducts, 1);
            if (products.Products.Count() > 0)
            {
                try
                {
                    monitoring.LastCommentCount = products.Products.Where(y => y.Product != null).Select(x => x.Product.product).Sum(x => x.comments_count);
                    monitoring.TotalCommentCount += monitoring.LastCommentCount;
                    monitoring.TotalProductCount += products.Products.Count();
                }
                catch
                {

                }
                digi.SendProductsServer(products);
                monitoring.LastSendToServerTimeProducts = Math.Round(((double)sw.ElapsedMilliseconds / 1000) - monitoring.LoadTimeProducts - monitoring.LastCrawlTimeProducts, 1);
            }
        }
    }
    sw.Stop();
    monitoring.Last = Math.Round((double)sw.ElapsedMilliseconds / 1000, 1);
    monitoring.TimeSheet.Add(monitoring.Last);
    Calc();
    Thread.Sleep(5000);
}

void Calc()
{
    if (monitoring.K % 2 == 0)
    {
        try
        {
            monitoring.TimeSheet.Remove(monitoring.TimeSheet.Max());
            monitoring.AvrageCrawling = Math.Round(monitoring.TimeSheet.Average());
            monitoring.HoursDurration = Math.Round((DateTime.Now - monitoring.StartTime).TotalHours, 2);
            monitoring.CountPerHours = Convert.ToInt32(monitoring.TotalCommentCount / monitoring.HoursDurration);
            Thread.Sleep(1500);
        }
        catch
        {
        }
        if (monitoring.K % 10 == 0)
        {
            Thread.Sleep(5000);
        }
    }

    Console.Write($"\r{monitoring.K++}  \t| {monitoring.TotalProductCount} \t\t| { monitoring.Last} \t| {monitoring.LoadTimeProducts} \t| {monitoring.LastCrawlTimeProducts} \t| {monitoring.LastSendToServerTimeProducts} \t| {monitoring.AvrageCrawling} \t| {monitoring.LastCommentCount} \t| {(monitoring.TotalCommentCount < 10000 ? monitoring.TotalCommentCount : Math.Round((double)(monitoring.TotalCommentCount / 1000), 1)+"_k")} \t| {monitoring.HoursDurration} \t| {monitoring.CountPerHours}________________.");
}

void loadConfig()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaCrawler.config");
    if (!File.Exists(path))
    {
        _config = new Config { Server = "http://185.147.160.124:5000", UserId = new Random().Next(100, int.MaxValue), Count = 10, UseProxy = false, ProxyHost = "127.0.0.1", ProxyPort = 9150, LocalDatabase = false };
        File.WriteAllText(path, JsonConvert.SerializeObject(_config));
    }
    else
    {
        string content = File.ReadAllText(path);
        _config = JsonConvert.DeserializeObject<Config>(content);
        Console.WriteLine(content);
    }
}

public struct Montoring
{
    public DateTime StartTime { get; set; } = DateTime.Now;
    public double AvrageCrawling { set; get; } = 0;
    public int LastCommentCount { set; get; } = 0;
    public int TotalCommentCount { set; get; } = 0;
    public int TotalProductCount { set; get; } = 0;
    public double HoursDurration { set; get; } = 0;
    public int CountPerHours { set; get; } = 0;
    public double LoadTimeProducts { set; get; } = 0;
    public double LastCrawlTimeProducts { set; get; } = 0;
    public double LastSendToServerTimeProducts { set; get; } = 0;
    public double Last { set; get; } = 0;
    public long K { set; get; } = 0;
    public List<double> TimeSheet = new List<double>();
}