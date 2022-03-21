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

        public DigikalaCrawlerServiceV1()
        {
        }

        public DigikalaCrawlerServiceV1(Config config)
        {
            _config = config;
        }

        public Task<HttpResponseMessage> GetHttp(string url)
        {
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

        public int GetProductIdFromUrl(string url)
        {
            return int.Parse(url.Substring(url.LastIndexOf("dkp-"), url.LastIndexOf('/') - url.LastIndexOf("dkp-")).Replace("dkp-", ""));
        }

        public List<int> GetProductIdFromUrls(List<string> urls)
        {
            return urls.Select(x => GetProductIdFromUrl(x)).ToList();
        }
        #endregion

        #region Crawl
        public async Task<ProductData> GetProduct(int productId)
        {
            string url = "https://api.digikala.com/v1/product/" + productId + "/";
            string res = await new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync();
            var product =JsonConvert.DeserializeObject<ProductObjV1>(res).data;
            return product;
        }

        public async Task<CommentObjV1> GetProductComment(int productId, int page = 1)
        {
            string url = $"https://api.digikala.com/v1/product/{productId}/comments/?page={page}&order=created_at";
            string res = await new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CommentObjV1>(res);
        }

        public async Task<CommentDetails> GetProductComments(int productId)
        {
            CommentDetails cm = (await GetProductComment(productId, 1)).data;
            List<Task<CommentObjV1>> tasks = new List<Task<CommentObjV1>>();
            if (cm.pager.total_pages > 1)
            {
                for (int i = 2; i <= cm.pager.total_pages; i++)
                {
                    tasks.Add(GetProductComment(productId, i));
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
                cm.comments.AddRange(task.Result.data.comments.ToList());
            }
            return cm;
        }
        #endregion

        #region Server Call
        public IEnumerable<int> GetFreeProductsFromServer()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.Server}/Digikala/GetFreeProducts/{_config.UserId}/{_config.Count}";
                string res = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<int>>(res);
            }
        }
        public IEnumerable<int> SendProductsServer(SetProductsDTO products)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_config.Server}/Digikala/SendProducts/";
                var json = JsonConvert.SerializeObject(products);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                string res = client.PostAsync(url,data).Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<int>>(res);
            }
        }
        #endregion
    }
}