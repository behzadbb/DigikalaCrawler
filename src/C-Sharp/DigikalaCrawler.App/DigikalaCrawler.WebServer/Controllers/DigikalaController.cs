using DigikalaCrawler.Data.Mongo;
using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<int> GetFreeProducts(int userid, int count, bool checkUserId)
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
            List<int> productLinks = new List<int>();
            foreach (string sitemap in sitemaps)
            {
                var main1 = await _crawler.DownloadSitemap(sitemap, path);
                var main2 = await _crawler.GetSitemap(main1);
                //productLinks.AddRange(_crawler.GetProductIdFromUrls(main2.Where(x => x.Contains("dkp-")).ToList()));
                var _productLinks = _crawler.GetProductIdFromUrls(main2.Where(x => x.Contains("dkp-")).ToList());
                _digi.InsertPages(_productLinks);
                Thread.Sleep(50);
                Console.Write(" _ " + _productLinks.Count());
            }
            _logger.Log(LogLevel.Information, "3 _ Success!");
            _digi.CreateIndex();
            _logger.Log(LogLevel.Information, "4 _ Create Index!");
            //_digi.InsertPages(productLinks);
            return Ok("Success");
        }

        [HttpPost("/[controller]/SendProducts")]
        public async Task<IActionResult> SendProducts(SetProductsDTO dto)
        {
            foreach (var product in dto.Products)
            {
                _digi.SetProduct(product);
            }
            return Ok();
        }

        [HttpGet("/[controller]/GetProductCount")]
        public async Task<IActionResult> GetProductCount()
        {
            return Ok(_digi.ProductCount());
        }
    }
}