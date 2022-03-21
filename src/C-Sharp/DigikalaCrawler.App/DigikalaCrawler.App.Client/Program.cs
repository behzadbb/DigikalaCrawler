using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace DigikalaCrawler.App.Client
{
    public class Program
    {
        private static Config _config;
        public static void Main(string[] args)
        {
            loadConfig();
            while (!string.IsNullOrEmpty(_config.Server))
            {
                using (DigikalaCrawlerServiceV1 digi = new DigikalaCrawlerServiceV1(_config))
                {
                    var ids = digi.GetFreeProductsFromServer();
                    SetProductsDTO products = new SetProductsDTO();
                    foreach (var id in ids)
                    {
                        var product = new SetProductDTO();
                        product.ProductId = id;
                        product.Product = digi.GetProduct(id).Result;
                        if (product.Product != null && product.Product.product.comments_count > 0)
                        {
                            product.Comments = digi.GetProductComments(id).Result;
                        }
                        products.Products.Add(product);
                    }
                    if (products.Products.Count() > 0)
                    {
                        digi.SendProductsServer(products);
                        Console.WriteLine("s");
                    }
                }
            }
        }
        public static void loadConfig()
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
    }
}