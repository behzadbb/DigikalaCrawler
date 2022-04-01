using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;

Config _config;
loadConfig();
var checkUserId = true;
int k = 0;
while (!string.IsNullOrEmpty(_config.Server))
{
    using (DigikalaCrawlerServiceV1 digi = new DigikalaCrawlerServiceV1(_config))
    {
        var ids = digi.GetFreeProductsFromServer(checkUserId).ToList();
        checkUserId = false;
        SetProductsDTO products = new SetProductsDTO();
        for (int i = 0; i < ids.Count(); i++)
        {
            var product = new SetProductDTO();
            product.ProductId = ids[i];
            try
            {
                product.Product = digi.GetProduct(ids[i]).Result;
                Thread.Sleep(150);
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
            digi.SendProductsServer(products);
        }
    }
    Console.Write("\r{0} \tIteration", k++);
}

void loadConfig()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaCrawler.config");
    if (!File.Exists(path))
    {
        _config = new Config { Server = "", UserId = new Random().Next(20000), Count = 10 };
        File.WriteAllText(path, JsonConvert.SerializeObject(_config));
        Console.WriteLine("......\n\n\t\tPlease Check Config on Desktop !!!\n\n......");
    }
    else
    {
        string content = File.ReadAllText(path);
        _config = JsonConvert.DeserializeObject<Config>(content);
    }
}