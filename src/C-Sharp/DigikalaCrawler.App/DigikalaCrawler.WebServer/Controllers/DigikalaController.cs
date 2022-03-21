using DigikalaCrawler.Data.Mongo;
using DigikalaCrawler.Services.Crawler;
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

        [HttpGet("[controller]/GetFreeProduct/{userid}/{count}")]
        public IEnumerable<int> GetFreeProduct(int userid, int count)
        {
            return _digi.GetFreeProduct(userid, count);
        }

        [HttpGet("[controller]/SetSitemap")]
        public async Task<IActionResult> SetSitemap()
        {
            IList<string> sitemaps = await _crawler.GetMainSitemap();
            List<int> productLinks = new List<int>();
            foreach (string sitemap in sitemaps)
            {
                var main1 = await _crawler.DownloadSitemap(sitemap, @"e:\newSitemap");
                var main2 = await _crawler.GetSitemap(main1);
                productLinks.AddRange(_crawler.GetProductIdFromUrls(main2.Where(x => x.Contains("dkp-")).ToList()));
            }
            _digi.InsertPages(productLinks);
            return Ok();
        }
    }
}