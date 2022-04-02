using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;
using System.Diagnostics;

DateTime startDateTime = DateTime.Now;
Console.WriteLine("Start: {0}", DateTime.Now);
Config _config;
loadConfig();
var checkUserId = true;
int k = 0;
double avrage = 0;
int LastCommentCount = 0;
int CommentCount = 0;
int ProductCount = 0;
double hours = 0;
int perHours = 0;
List<long> timeSheet = new List<long>();
Stopwatch sw = new Stopwatch();
while (!string.IsNullOrEmpty(_config.Server))
{
    sw.Restart();
    using (DigikalaCrawlerServiceV1 digi = new DigikalaCrawlerServiceV1(_config))
    {
        var ids = digi.GetFreeProductsFromServer(checkUserId).ToList();
        Thread.Sleep(50);
        checkUserId = false;
        SetProductsDTO products = new SetProductsDTO();
        for (int i = 0; i < ids.Count(); i++)
        {
            var product = new SetProductDTO();
            product.ProductId = ids[i];
            try
            {
                product.Product = digi.GetProduct(ids[i]).Result;
                Thread.Sleep(200);
                if (product.Product != null && product.Product.product.comments_count > 0)
                {
                    product.Comments = digi.GetProductComments(ids[i]).Result;
                }
                products.Products.Add(product);
            }
            catch (Exception ex)
            {
                product.Error = true;
                product.ErrorMessage = ex.Message;
                product.ClientError = true;
                products.Products.Add(product);
            }
        }
        if (products.Products.Count() > 0)
        {
            try
            {
                LastCommentCount = products.Products.Where(y => y.Product != null).Select(x => x.Product.product).Sum(x => x.comments_count);
                CommentCount += LastCommentCount;
                ProductCount += products.Products.Count();
            }
            catch
            {

            }
            digi.SendProductsServer(products);
        }
    }
    sw.Stop();
    timeSheet.Add(sw.ElapsedMilliseconds);
    if (k % 10 == 0)
    {
        try
        {
            timeSheet.Remove(timeSheet.Max());
            avrage = Math.Round(timeSheet.Average());
            hours = Math.Round((DateTime.Now - startDateTime).TotalHours, 4);
            perHours = Convert.ToInt32(CommentCount / hours);
        }
        catch
        {
        }
    }
    Console.Write("\r{0}\t| P:{1}_ \t | Last:{2}_ \t |  Avg:{3}_ \t| Cm:{4}_ \t| Total:{5}_\t | H:{6}_\t| Cmh:{7}______\t|", k++, ProductCount, sw.ElapsedMilliseconds, avrage, LastCommentCount, CommentCount, hours, perHours);
    Thread.Sleep(150);
}

void loadConfig()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaCrawler.config");
    if (!File.Exists(path))
    {
        _config = new Config { Server = "http://185.147.160.124:5000", UserId = new Random().Next(100, int.MaxValue), Count = 10, UseProxy = false, ProxyHost = "127.0.0.1", ProxyPort = 9150 };
        File.WriteAllText(path, JsonConvert.SerializeObject(_config));
    }
    else
    {
        string content = File.ReadAllText(path);
        _config = JsonConvert.DeserializeObject<Config>(content);
        Console.WriteLine(content);
    }
}