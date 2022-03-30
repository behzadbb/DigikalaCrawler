using DigikalaCrawler.Share.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DigikalaCrawler.Share.Services
{
    public class DigikalaCrawlerServiceV1 : IDisposable
    {
        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DigikalaCrawlerServiceV1()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region init
        private Config _config;

        List<string> proxies = new List<string>(){
            "130.185.73.139:8888",
            "185.72.27.98:8080",
            "188.95.89.81:8080",
            "217.60.194.51:8080",
            "37.255.135.18:8080",
            "46.100.166.38:8080",
            "5.202.191.226:8080",
            "77.104.97.5:8080",
            "78.38.100.121:8080",
            "79.127.56.147:8080",
            "80.191.162.2:514",
            "81.91.144.190:55443",
            "89.165.40.12:8080",
            "94.74.132.129:808"};

        public DigikalaCrawlerServiceV1()
        {
        }

        public DigikalaCrawlerServiceV1(Config config)
        {
            _config = config;
        }

        public Task<HttpResponseMessage> GetHttp(string url)
        {
            //var proxy = new WebProxy
            //{
            //    Address = new Uri(proxies[1]),
            //    BypassProxyOnLocal = false,
            //    UseDefaultCredentials = false,

            //    // *** These creds are given to the proxy server, not the web server ***
            //    Credentials = new NetworkCredential()
            //};
            //// Now create a client handler which uses that proxy
            //var httpClientHandler = new HttpClientHandler
            //{
            //    Proxy = proxy,
            //};
            //// Finally, create the HTTP client object
            //var client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
            HttpClient client = new HttpClient();
            return client.GetAsync(url);
        }
        #endregion

        #region Sitemap
        public async Task<IList<string>> GetMainSitemap()
        {
            var response = await GetHttp("https://www.digikala.com/sitemap.xml");
            List<string> locs = new List<string>();
            if (response.IsSuccessStatusCode)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(response.Content.ReadAsStringAsync().Result);
                var ienum = xml.GetElementsByTagName("loc").GetEnumerator();
                while (ienum.MoveNext())
                {
                    XmlNode title = (XmlNode)ienum.Current;
                    locs.Add(title.InnerText);
                }
            }
            return locs;
        }

        public async Task<string> DownloadSitemap(string url, string savePath)
        {
            string fileName = url.Substring(url.LastIndexOf("/") + 1, url.LastIndexOf(".") - url.LastIndexOf("/") - 1);
            string fileExt = url.Substring(url.LastIndexOf("."), url.Length - url.LastIndexOf("."));
            string fullPathCompress = Path.Combine(savePath, "Compress", fileName + fileExt);
            string fullPath = Path.Combine(savePath, fileName + ".xml");
            if (!Directory.Exists(Path.Combine(savePath, "Compress")))
                Directory.CreateDirectory(Path.Combine(savePath, "Compress"));
            if (!File.Exists(fullPathCompress))
            {
                WebClient ClientI = new WebClient();
                ClientI.DownloadFile(url, fullPathCompress);
                Thread.Sleep(500);
            }
            string ReadData = "";
            GZipStream instreamI = new GZipStream(File.OpenRead(fullPathCompress), CompressionMode.Decompress);
            StreamReader readerI = new StreamReader(instreamI, Encoding.UTF8);
            ReadData = readerI.ReadToEnd();
            readerI.Close();
            XmlDocument xdocI = new XmlDocument();
            xdocI.LoadXml(ReadData);
            xdocI.Save(fullPath);
            return fullPath;
        }

        public async Task<IList<string>> GetSitemap(string path)
        {
            List<string> locs = new List<string>();
            if (File.Exists(path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                var ienum = xml.GetElementsByTagName("loc").GetEnumerator();
                while (ienum.MoveNext())
                {
                    XmlNode title = (XmlNode)ienum.Current;
                    locs.Add(title.InnerText);
                }
            }
            return locs;
        }

        public long GetProductIdFromUrl(string url)
        {
            try
            {
                var s1 = url.LastIndexOf('/');
                var s2 = url.IndexOf("dkp-");
                if (s2 - s1 == 1)
                {
                    return long.Parse(url.Substring(url.IndexOf("dkp-"), url.Length - url.IndexOf("dkp-")).Replace("dkp-", ""));
                }
                var i = long.Parse(url.Substring(url.IndexOf("dkp-"), url.LastIndexOf('/') - url.IndexOf("dkp-")).Replace("dkp-", ""));
                return i;
            }
            catch (Exception)
            {
                Console.WriteLine("\n\t\tError Url:\n" + url + "\n\n\n");
                throw;
            }
        }

        public List<long> GetProductIdFromUrls(List<string> urls)
        {
            return urls.Select(x => GetProductIdFromUrl(x)).ToList();
        }
        #endregion

        #region Crawl
        public async Task<ProductData> GetProduct(long productId)
        {
            string url = "https://api.digikala.com/v1/product/" + productId + "/";
            string res = await GetHttp(url).Result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductObjV1>(res).data;
            return product;
        }

        public async Task<CommentObjV1> GetProductComment(long productId, int page = 1)
        {
            try
            {
                string url = $"https://api.digikala.com/v1/product/{productId}/comments/?page={page}&order=created_at";
                string res = await new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CommentObjV1>(res);
            }
            catch
            {
                return new CommentObjV1();
            }
        }

        public async Task<CommentDetails> GetProductComments(long productId)
        {
            CommentDetails cm = (await GetProductComment(productId, 1)).data;
            List<Task<CommentObjV1>> tasks = new List<Task<CommentObjV1>>();
            if (cm != null && cm.pager.total_pages > 1)
            {
                for (int i = 2; i <= cm.pager.total_pages; i++)
                {
                    tasks.Add(GetProductComment(productId, i));
                    Thread.Sleep(100);
                }
            }
            Task t = Task.WhenAll(tasks.ToArray());
            try
            {
                await t;
            }
            catch
            {
            }
            foreach (var task in tasks)
            {
                var _data = task.Result;
                if (_data != null && _data.data != null && _data.data.comments != null && _data.data.comments.Count() > 0)
                    cm.comments.AddRange(_data.data.comments.ToList());
            }
            return cm;
        }
        #endregion

        #region Server Call
        public IEnumerable<int> GetFreeProductsFromServer(bool checkUserId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.Server}/Digikala/GetFreeProducts/{_config.UserId}/{_config.Count}/{checkUserId}";
                string res = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<int>>(res);
            }
        }
        public Task SendProductsServer(SetProductsDTO products)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.Server}/Digikala/SendProducts/";
                var json = JsonConvert.SerializeObject(products);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                client.PostAsync(url, data).Result.Content.ReadAsStringAsync();
                return Task.CompletedTask;
            }
        }
        #endregion
    }
}