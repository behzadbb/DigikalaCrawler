using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Models.Question;
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
            HttpClient client;
            if (_config.UseProxy)
            {
                MihaZupan.HttpToSocks5Proxy proxy = new MihaZupan.HttpToSocks5Proxy(_config.ProxyHost, _config.ProxyPort);
                var handler = new HttpClientHandler
                {
                    Proxy = proxy
                };
                client = new HttpClient(handler);
            }
            else
            {
                client = new HttpClient();
            }
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
                var lastSlashIndex = url.LastIndexOf('/');
                var dkpIndex = url.IndexOf("dkp-");
                if (dkpIndex - lastSlashIndex == 1)
                {
                    return long.Parse(url.Substring(dkpIndex, url.Length - dkpIndex).Replace("dkp-", ""));
                }
                var i = long.Parse(url.Substring(dkpIndex, lastSlashIndex - dkpIndex).Replace("dkp-", ""));
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
                string res = await GetHttp(url).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CommentObjV1>(res);
            }
            catch
            {
                return new CommentObjV1();
            }
        }

        public async Task<CommentDetails> GetProductComments(long productId)
        {
            int random = new Random().Next(10, 45);
            CommentDetails cm = (await GetProductComment(productId, 1)).data;
            List<Task<CommentObjV1>> tasks = new List<Task<CommentObjV1>>();
            if (cm != null && cm.pager.total_pages > 1)
            {
                for (int i = 2; i <= cm.pager.total_pages; i++)
                {
                    tasks.Add(GetProductComment(productId, i));
                    Thread.Sleep(random);
                    if (i > 10 && i % 5 == 0)
                    {
                        Thread.Sleep(50);
                    }
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

        #region Question
        public async Task<QuestionResponse> GetQuestion(long productId, int page = 1)
        {
            try
            {
                string url =  $"https://api.digikala.com/v1/product/{productId}/questions/?page={page}&sort=created_at";
                string res = await GetHttp(url).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QuestionResponse>(res);
            }
            catch
            {
                return new QuestionResponse();
            }
        }

        public async Task<Questions> GetQuestions(long productId)
        {
            int random = new Random().Next(10, 45);
            Questions _questions = (await GetQuestion(productId, 1)).data;
            List<Task<QuestionResponse>> tasks = new List<Task<QuestionResponse>>();
            if (_questions != null && _questions.pager.total_pages > 1)
            {
                for (int i = 2; i <= _questions.pager.total_pages; i++)
                {
                    tasks.Add(GetQuestion(productId, i));
                    Thread.Sleep(random);
                    if (i > 10 && i % 5 == 0)
                    {
                        Thread.Sleep(50);
                    }
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
                if (_data != null && _data.data != null && _data.data.questions != null && _data.data.questions.Count() > 0)
                    _questions.questions.AddRange(_data.data.questions.ToList());
            }
            return _questions;
        }
        #endregion

        #region Server Call
        public async Task<List<long>> GetFreeProductsFromServer(bool checkUserId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.Server}/Digikala/GetFreeProducts/{_config.UserId}/{_config.Count}/{checkUserId}";
                string res = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<long>>(res);
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