using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;
using System.Diagnostics;

Console.WriteLine("Start: {0}", DateTime.Now);
Config _config;
loadConfig();
var checkUserId = true;
int k = 0;
double avrage = 0;
int LastCommentCount = 0;
int CommentCount = 0;
int ProductCount = 0;
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
                Thread.Sleep(100);
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
                LastCommentCount = products.Products.Select(x => x.Product.product).Sum(x => x.comments_count);
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
    if (k % 5 == 0)
    {
        avrage = Math.Round(timeSheet.Average());
    }
    Console.Write("\r{0} Iteration \t| Product:{1}\t| Last:{2} \t| Avg:{3} \t| Cm:{4} \t|\t  Total Cm:{5}\t\t", k++, ProductCount, sw.ElapsedMilliseconds, avrage, LastCommentCount, CommentCount);
    Thread.Sleep(150);
}

void loadConfig()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaCrawler.config");
    if (!File.Exists(path))
    {
        _config = new Config { Server = "http://185.147.160.124:5000", UserId = new Random().Next(100, int.MaxValue), Count = 10 };
        File.WriteAllText(path, JsonConvert.SerializeObject(_config));
    }
    else
    {
        string content = File.ReadAllText(path);
        _config = JsonConvert.DeserializeObject<Config>(content);
    }
}