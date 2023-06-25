using DigikalaCrawler.Data.Mongo;
using DigikalaCrawler.Data.Mongo.DBModels;
using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using System.Reflection.PortableExecutable;
using SharpCompress.Common;
using MongoDB.Bson.IO;

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

        [HttpGet("/[controller]/DowloadSitemap")]
        public async Task<IActionResult> DowloadSitemap()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaSiteMap");
            _logger.Log(LogLevel.Information, "0 _ Path: " + path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _logger.Log(LogLevel.Information, "1");
            IList<string> sitemaps = await _crawler.GetMainSitemap();
            _logger.Log(LogLevel.Information, "2 _ Count: " + sitemaps.Count());
            for (int i = 0; i < sitemaps.Count(); i++)
            {
                if(!System.IO.File.Exists(path))
                    await _crawler.DownloadSitemap(sitemaps[i], path);
                _logger.Log(LogLevel.Information, $"{i}");
            }
           
            return Ok("Success");
        }

        [HttpGet("/[controller]/SetSitemap")]
        public async Task<IActionResult> SetSitemap()
        {
            List<long> ids = new List<long>();
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaSiteMap");
            if (!System.IO.File.Exists(Path.Combine(path, "All.txt")))
            {
                var sitemaps = System.IO.Directory.GetFiles(path, "*.xml");
                _logger.Log(LogLevel.Information, "2 _ Count: " + sitemaps.Count());
                List<long> productLinks = new List<long>();
                for (int i = 0; i < sitemaps.Count(); i++)
                {
                    List<string> _fullUrl = new List<string>();
                    try
                    {
                        _fullUrl.AddRange(await _crawler.GetSitemap(sitemaps[i]));
                    }
                    catch
                    {
                        _fullUrl.AddRange(await _crawler.GetSitemap1(sitemaps[i]));
                    }
                    List<long> _productIds = _crawler.GetProductIdFromUrls(_fullUrl.Where(x => x.Contains("dkp-")).ToList());
                    Console.Write($"_{i}");
                    productLinks.AddRange(_productIds);
                }
                ids = productLinks.Distinct().OrderBy(x => x).ToList();
                string content = string.Join("\n", ids);
                System.IO.File.WriteAllText(Path.Combine(path, "All.txt"), content);
            }
            else
            {
                ids = System.IO.File.ReadAllLines(Path.Combine(path, "All.txt")).Select(x => long.Parse(x)).ToList();
            }

            int count = (int)Math.Ceiling((double)ids.Count() / 500000);
            for (int i = 0; i < count; i++)
            {
                var _ids = ids.Skip(i* 500000).Take(500000);
                string content = string.Join("\n", _ids);
                System.IO.File.WriteAllText(Path.Combine(path, $"{i}--{i*500000}-{((i+1)*500000)-1}.crawl"), content);
            }
            //_digi.InsertPages(ids);
            //Console.Write(" _ insert: " + ids.Count());
            //_logger.Log(LogLevel.Information, "\n3 _ Success!");
            //_digi.CreateIndex("ProductId", "UserId", "Assign", "Success");
            //_logger.Log(LogLevel.Information, "\n4 _ Create Index!");
            //_digi.InsertPages(productLinks);
            return Ok("Success");
        }

        [HttpGet("/[controller]/SetCrawlPages/{page}")]
        public async Task<IActionResult> SetCrawlPages(int page)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaSiteMap");
            var crawls = System.IO.Directory.GetFiles(path, "*.crawl");

            string filePath = crawls.FirstOrDefault(x => x.Contains(page+"--"));
            List<long> ids = System.IO.File.ReadAllLines(filePath).Select(x=>long.Parse(x)).ToList();
            _digi.InsertPages(ids);
            Console.Write(" _ insert: " + ids.Count());
            await Task.Delay(10 * 1000);
            _logger.Log(LogLevel.Information, "\n3 _ Success!");
            _digi.DropIndex();
            await Task.Delay(2 * 1000);
            string[] ind= { "ProductId", "UserId", "Assign", "Success" };
            _digi.CreateIndex(ind);
            _logger.Log(LogLevel.Information, "\n4 _ Create Index!");
            return Ok("Success");
        }

        [HttpPost("/[controller]/SendProducts")]
        public async Task<IActionResult> SendProducts([FromBody] object json)
        {
            SetProductsDTO dto = new SetProductsDTO();
            string s = json.ToString();
            dto = Newtonsoft.Json.JsonConvert.DeserializeObject<SetProductsDTO>(s);
            _logger.LogWarning($"Comments:{dto.Products.Sum(x=>x.CommentsCount)}, Send:{dto.Products.Sum(x => x.SendCommentsCount)}");
            _logger.LogWarning($"Comments:{dto.Products.Sum(x=>x.CommentsCount)}, Send:{dto.Products.Sum(x => x.SendCommentsCount)}, Recive:{dto.Products.Where(x=>x.CommentData!=null && x.CommentData.Comments.Any()).Sum(x=>x.CommentData.Comments.Count())}");
            
            //dto = (SetProductsDTO)json;
            if (CountStatic.LastTime.ContainsKey(dto.UserId))
                CountStatic.LastTime[dto.UserId] = DateTime.Now;
            else
                CountStatic.LastTime.Add(dto.UserId, DateTime.Now);

            var products = dto.Products.Select(x => new DigikalaProductCrawl()
            {
                ClientError = x.ClientError,
                CommentData = x.CommentData,
                Questions = x.Questions,
                CrawleDate = DateTime.Now,
                Error = x.Error,
                ErrorMessage = x.ErrorMessage,
                JsonObject = x.JsonObject,
                ProductData = x.Product,
                ProductId = x.ProductId,
                Success = true,
                UserId = dto.UserId,
                CommentsCount = x.CommentsCount,
                SendCommentsCount = x.SendCommentsCount,
                QuestionsCount = x.QuestionsCount
            }).ToList();
            foreach (var product in products)
            {
                _digi.InsertDigikalaProductCrawl(product);
                //await Task.Run(() =>
                //{
                //    _digi.InsertDigikalaProductCrawl(product);
                //});
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
                sb.AppendLine(string.Join("\n", CountStatic.LastTime.Select(x => "User "+ x.Key + ": " + (DateTime.Now - x.Value).TotalSeconds + "s")));
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