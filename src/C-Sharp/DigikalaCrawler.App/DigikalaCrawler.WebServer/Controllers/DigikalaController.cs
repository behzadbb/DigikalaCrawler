using DigikalaCrawler.Data.Mongo;
using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DigikalaCrawler.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigikalaController : ControllerBase
    {
        private readonly ILogger<DigikalaController> _logger;
        private readonly DigikalaMongo _digi;
        private readonly DigikalaCrawlerServiceV1 _crawler;

        public DigikalaController(ILogger<DigikalaController> logger, DigikalaMongo digi, DigikalaCrawlerServiceV1 crawler)
        {
            _logger = logger;
            _digi = digi;
            _crawler = crawler;
        }

        [HttpGet("/[controller]/GetFreeProducts/{userid}/{count}/{checkUserId}")]
        public IEnumerable<long> GetFreeProducts(int userid, int count, bool checkUserId)
        {
            return _digi.GetFreeProduct(userid, count, checkUserId);
        }

        [HttpGet("/[controller]/SetSitemap")]
        public async Task<IActionResult> SetSitemap()
        {
            string path = "";
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaSiteMap");
            _logger.Log(LogLevel.Information, "0 _ Path: " + path);
            if (string.IsNullOrEmpty(path))
            {
                return Ok("Path is Empty");
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _logger.Log(LogLevel.Information, "1");
            IList<string> sitemaps = await _crawler.GetMainSitemap();
            _logger.Log(LogLevel.Information, "2 _ Count: " + sitemaps.Count());
            List<long> productLinks = new List<long>();
            foreach (string sitemap in sitemaps.Skip(10).Take(2))
            {
                var _xmlPath = await _crawler.DownloadSitemap(sitemap, path);
                var _fullUrl = await _crawler.GetSitemap(_xmlPath);
                //productLinks.AddRange(_crawler.GetProductIdFromUrls(main2.Where(x => x.Contains("dkp-")).ToList()));
                List<long> _productIds = _crawler.GetProductIdFromUrls(_fullUrl.Where(x => x.Contains("dkp-")).ToList());
                //_digi.InsertPages(_productLinks);
                productLinks.AddRange(_productIds);

                //if (productLinks.Count() > 150000)
                //{
                //    _digi.InsertPages(productLinks);
                //    Console.Write(" _ insert: " + productLinks.Count());
                //    productLinks.Clear();
                //}
            }
            List<long> ids = productLinks.Distinct().OrderBy(x => x).ToList();
            _digi.InsertPages(ids);
            Console.Write(" _ insert: " + ids.Count());
            _logger.Log(LogLevel.Information, "\n3 _ Success!");
            _digi.CreateIndex("ProductId", "UserId");
            _logger.Log(LogLevel.Information, "\n4 _ Create Index!");
            //_digi.InsertPages(productLinks);
            return Ok("Success");
        }

        [HttpPost("/[controller]/SendProducts")]
        public async Task<IActionResult> SendProducts(SetProductsDTO dto)
        {
            if (CountStatic.LastTime.ContainsKey(dto.UserId))
                CountStatic.LastTime[dto.UserId] = DateTime.Now;
            else
                CountStatic.LastTime.Add(dto.UserId, DateTime.Now);

            var products = dto.Products.Select(x => new DigikalaProductCrawl()
            {
                ClientError = x.ClientError,
                CommentDetails = x.Comments,
                Questions = x.Questions,
                CrawleDate = DateTime.Now,
                Error = x.Error,
                ErrorMessage = x.ErrorMessage,
                JsonObject = x.JsonObject,
                ProductData = x.Product,
                ProductId = x.ProductId,
                Success = true,
                UserId = dto.UserId
            }).ToList();
            foreach (var product in products)
            {
                await Task.Run(() =>
                {
                    _digi.InsertDigikalaProductCrawl(product);
                });
            }
            _ = Task.Run(() =>
            {
                _digi.UpdateProductsCrawlAsync(products.Select(x => x.ProductId).ToList());
            });
            return Ok();
        }

        [HttpGet("/[controller]/GetProductCount")]
        public IActionResult GetProductCount()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_digi.ProductCount());
            if (CountStatic.LastTime.Any())
                sb.AppendLine(string.Join("\n", CountStatic.LastTime.Select(x => x.Key + ": " + (DateTime.Now - x.Value).TotalSeconds + "s")));
            return Ok(sb.ToString());
        }

        [HttpGet("/[controller]/CreateIndex")]
        public IActionResult CreateIndex()
        {
            _digi.CreateIndex("_id", "ProductId", "UserId", "Assign", "Success");
            return Ok("Create Index!!!");
        }

        [HttpGet("/[controller]/DropIndex")]
        public IActionResult DropIndex()
        {
            _digi.DropIndex();
            return Ok("Drop Index!!!");
        }
    }


    public static class CountStatic
    {
        static CountStatic()
        {
            LastTime = new Dictionary<int, DateTime>();
        }
        public static Dictionary<int, DateTime> LastTime { get; set; }
    }
}