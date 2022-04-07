using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;
using System.Diagnostics;

Montoring montoring = new Montoring();
Console.WriteLine("Start: {0}", DateTime.Now);
Config _config;
loadConfig();
var checkUserId = true;

Stopwatch sw = new Stopwatch();
while (!string.IsNullOrEmpty(_config.Server))
{
    sw.Restart();
    using (DigikalaCrawlerServiceV1 digi = new DigikalaCrawlerServiceV1(_config))
    {
        List<long> ids = await digi.GetFreeProductsFromServer(checkUserId);
        montoring.LoadTimeProducts = sw.ElapsedMilliseconds;
        int random = new Random().Next(3, 30);
        Thread.Sleep(random);
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
            if (products.Products.Count() > 0)
            {
                try
                {
                    montoring.LastCommentCount = products.Products.Where(y => y.Product != null).Select(x => x.Product.product).Sum(x => x.comments_count);
                    montoring.TotalCommentCount += montoring.LastCommentCount;
                    montoring.TotalProductCount += products.Products.Count();
                }
                catch
                {

                }
                digi.SendProductsServer(products);
            }
        }
    }
    sw.Stop();
    montoring.TimeSheet.Add(sw.ElapsedMilliseconds);
    Calc();
    Thread.Sleep(150);
}

void Calc()
{
    if (montoring.K % 5 == 0)
    {
        try
        {
            montoring.TimeSheet.Remove(montoring.TimeSheet.Max());
            montoring.AvrageCrawling = Math.Round(montoring.TimeSheet.Average());
            montoring.HoursDurration = Math.Round((DateTime.Now - montoring.StartTime).TotalHours, 4);
            montoring.CountPerHours = Convert.ToInt32(montoring.TotalCommentCount / montoring.HoursDurration);
        }
        catch
        {
        }
    }
    Console.Write($"\r{montoring.K++}_\t|_P:{montoring.TotalProductCount}_\t| Last Crawl:{sw.ElapsedMilliseconds}_\t |  Avg:{montoring.AvrageCrawling}_ \t| Last Comment:{montoring.LastCommentCount}_ \t| Total:{montoring.TotalCommentCount}_\t | H:{montoring.HoursDurration}_\t | Comment/H:{montoring.CountPerHours}_/t | LoadProductTime:_{montoring.LoadTimeProducts}____\t| .");
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
    public long LoadTimeProducts { set; get; } = 0;
    public long K { set; get; } = 0;
    public List<long> TimeSheet = new List<long>();
}