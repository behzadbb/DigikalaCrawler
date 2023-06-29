using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Models.Question;
using HtmlAgilityPack;
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
        private readonly IHttpClientFactory _clientFactory;
        private DateTime last;

        public DigikalaCrawlerServiceV1(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            TicketState.SetNew();
        }

        public void SetConfig(Config config)
        {
            _config = config;
            last = DateTime.Now;
        }

        public async Task<HttpContent> GetHttp(string url)
        {
            HttpClient client;
            client = _clientFactory.CreateClient();
            for (int i = 1; i < 4; i++)
            {
                //Console.WriteLine($"_{(DateTime.Now - last).TotalMilliseconds}");
                var diff = TicketState.Diff;
                if (diff < 60)
                {
                    await Task.Delay(60 - diff);
                }
                //Console.Write($"\r{String.Format("{0:0000}", TicketState.Diff)}");
                TicketState.SetNew();
                var response = await client.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Content;
                }
                else
                {
                    await Task.Delay(i * 150);
                    client.Dispose();
                    client = _clientFactory.CreateClient();
                    Console.WriteLine($"Error:\n\n\n\t{url}\n\n\n");
                }
            }
            return (await client.GetAsync(url)).Content;
        }
        #endregion

        #region Sitemap
        public async Task<IList<string>> GetMainSitemap()
        {
            var response = await GetHttp("https://www.digikala.com/sitemap.xml");
            List<string> locs = new List<string>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response.ReadAsStringAsync().Result);
            var ienum = xml.GetElementsByTagName("loc").GetEnumerator();
            while (ienum.MoveNext())
            {
                XmlNode title = (XmlNode)ienum.Current;
                locs.Add(title.InnerText);
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
            try
            {
                if (!File.Exists(fullPathCompress))
                {
                    WebClient ClientI = new WebClient();
                    ClientI.DownloadFile(url, fullPathCompress);
                    Console.Write("|");
                    Thread.Sleep(500);
                }
                if (!File.Exists(fullPath))
                {
                    string ReadData = "";
                    GZipStream instreamI = new GZipStream(File.OpenRead(fullPathCompress), CompressionMode.Decompress);
                    StreamReader readerI = new StreamReader(instreamI, Encoding.UTF8);
                    ReadData = readerI.ReadToEnd();
                    readerI.Close();
                    XmlDocument xdocI = new XmlDocument();
                    xdocI.LoadXml(ReadData);
                    xdocI.Save(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nurl: {url}\n{ex.Message}\n");
            }
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
        public async Task<IList<string>> GetSitemap1(string path)
        {
            List<string> locs = new List<string>();
            if (File.Exists(path))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(path);

                // Get all <loc> elements
                var locNodes = doc.DocumentNode.SelectNodes("//loc").Select(x => x.InnerText).ToList();
                return locNodes;
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
            string res = await (await GetHttp(url)).ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductObjV1>(res).data;
        }

        public async Task<CommentObjV1> GetProductComment(long productId, int page = 1)
        {
            try
            {
                //Console.Write("\t|--> ");
                string url = $"https://api.digikala.com/v1/product/{productId}/comments/?page={page}&order=created_at";
                string res = await (await GetHttp(url)).ReadAsStringAsync();
                if (res.Contains("advantages\":{"))
                {
                    res = FixAdventages(res);
                }
                return JsonConvert.DeserializeObject<CommentObjV1>(res);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nComment Error, DKP: {productId}  ,  Page: {page}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nError:\t{ex.Message}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nStack:\t{ex.StackTrace}");
                Console.ForegroundColor = ConsoleColor.White;
                throw ex;
            }
        }
        private string FixAdventages(string str)
        {
            while (str.Contains("advantages\":{"))
            {
                var index = str.IndexOf("advantages\":{");
                var index1 = str.IndexOf('}', index);
                var s = str.Substring(index, index1 - index + 1);
                string new1 = s.Replace("{", "[").Replace("}", "]").Replace("\":\"", "\",\"");
                str = str.Replace(s, new1);
            }
            return str;
        }

        public async Task<List<CommentObjV1>> GetProductCommentsPart(long productId, params int[] pages)
        {
            int random = new Random().Next(65, 80);
            List<CommentObjV1> _comments = new List<CommentObjV1>();

            List<Task<CommentObjV1>> tasks = new List<Task<CommentObjV1>>();
            for (int i = 0; i < pages.Length; i++)
            {
                tasks.Add(GetProductComment(productId, pages[i]));
                await Task.Delay(5);
            }
            Task t = Task.WhenAll(tasks.ToArray());
            try
            {
                await t;
            }
            catch
            {
            }
            for (int i = 0; i < tasks.Count(); i++)
            {
                var _data = tasks[i].Result;
                if (_data != null && _data.Data != null && _data.Data.Comments != null && _data.Data.Comments.Count() > 0)
                    _comments.Add(_data);
                else
                    Console.WriteLine("GetProductComments, i:" + i + "\t staus:" + _data.Status);
            }
            //Console.WriteLine("______________" + string.Join(" | ", pages) + "______________");
            return _comments;
        }

        public async Task<CommentData> GetProductComments(long productId, int pages)
        {
            var conditions = Math.Min(100, (int)Math.Ceiling((double)pages / 20));
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"\t\tDKP: {productId}  -  Comments: {pages}  -  Condition: {conditions}");
            //Console.ForegroundColor = ConsoleColor.Cyan;

            int step = 15;
            List<CommentObjV1> _comments = new List<CommentObjV1>();
            for (int i = 0; i < conditions; i++)
            {
                List<int> _pages = new List<int>();
                int k = i * step;
                while (k < pages && k < ((i + 1) * step) && k < conditions)
                {
                    _pages.Add(k + 1);
                    k++;
                }
                _comments.AddRange(await GetProductCommentsPart(productId, _pages.ToArray()));
                if (k >= conditions)
                {
                    break;
                }
                //await Task.Delay(100);
            }

            CommentData comment = new CommentData();
            comment.SortOptions = _comments[0].Data.SortOptions;
            comment.Sort = _comments[0].Data.Sort;
            comment.IntentData = _comments[0].Data.IntentData;
            comment.Ratings = _comments[0].Data.Ratings;
            comment.MediaComments = _comments.SelectMany(x => x.Data.MediaComments).Distinct().ToList();
            comment.Comments = _comments.SelectMany(x => x.Data.Comments).Distinct().ToList();
            comment.Pager = _comments[0].Data.Pager;
            //Console.ForegroundColor = ConsoleColor.White;
            return comment;
        }

        #endregion

        #region Question
        public async Task<QuestionResponse> GetQuestion(long productId, int page = 1)
        {
            try
            {
                string url = $"https://api.digikala.com/v1/product/{productId}/questions/?page={page}&sort=created_at";
                string res = await (await GetHttp(url)).ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QuestionResponse>(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<QuestionResponse>> GetQuestionPart(long productId, params int[] pages)
        {
            int random = new Random().Next(65, 80);
            List<QuestionResponse> _questions = new List<QuestionResponse>();

            List<Task<QuestionResponse>> tasks = new List<Task<QuestionResponse>>();
            for (int i = 0; i < pages.Length; i++)
            {
                tasks.Add(GetQuestion(productId, pages[i]));
                //await Task.Delay(random);
            }
            Task t = Task.WhenAll(tasks.ToArray());
            try
            {
                await t;
            }
            catch
            {
            }
            for (int i = 0; i < tasks.Count(); i++)
            {
                var _data = tasks[i].Result;
                if (_data != null && _data.data != null && _data.data.questions != null && _data.data.questions.Count() > 0)
                    _questions.Add(_data);
                else
                    Console.WriteLine("Get Question Part, i:" + i + "\t staus:" + _data.status);
            }
            //Console.WriteLine("______________" + string.Join(" | ", pages) + "______________");
            return _questions;
        }


        public async Task<Questions> GetQuestions(long productId, int pages)
        {
            var conditions = Math.Min(50, (int)Math.Ceiling((double)pages / 20));
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"\t\tDKP: {productId}  -  Question: {pages}  -  Condition: {conditions}");
            //Console.ForegroundColor = ConsoleColor.Cyan;

            int step = 5;
            List<QuestionResponse> _question = new List<QuestionResponse>();
            for (int i = 0; i < conditions; i++)
            {
                List<int> _pages = new List<int>();
                int k = i * step;
                while (k < pages && k < ((i + 1) * step) && k < conditions)
                {
                    _pages.Add(k + 1);
                    k++;
                }
                _question.AddRange(await GetQuestionPart(productId, _pages.ToArray()));
                if (k >= conditions)
                {
                    break;
                }
                //await Task.Delay(100);
            }

            Questions q = new Questions();
            q.pager = _question[0].data.pager;
            q.questions = _question.SelectMany(x => x.data.questions).ToList();
            //Console.ForegroundColor = ConsoleColor.White;
            return q;
        }

        public async Task<Questions> GetQuestions(long productId)
        {
            int random = new Random().Next(20, 50);
            Questions _questions = (await GetQuestion(productId, 1)).data;
            List<Task<QuestionResponse>> tasks = new List<Task<QuestionResponse>>();
            if (_questions != null && _questions.pager.total_pages > 1)
            {
                for (int i = 2; i <= Math.Min(_questions.pager.total_pages, 49); i++)
                {
                    tasks.Add(GetQuestion(productId, i));
                    Thread.Sleep(random);
                    if (i > 9 && i % 5 == 0)
                        await Task.Delay(100);
                    if (i > 9 && i % 10 == 0)
                        Thread.Sleep(1500);
                    if (i > 19)
                        Thread.Sleep(50);
                    if (i > 30)
                        Thread.Sleep(50);
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
            using (HttpClient client = _clientFactory.CreateClient())
            {
                string url = $"{_config.Server}/Digikala/GetFreeProducts/{_config.UserId}/{_config.Count}/{checkUserId}";
                string res = await client.GetAsync(url).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<long>>(res);
            }
        }
        public async Task SendProductsServer(SetProductsDTO products)
        {
            if (_config.LocalDatabase)
            {
                Task.Run(() => SaveToFile(products));
                using (HttpClient client = _clientFactory.CreateClient())
                {
                    string url = $"{_config.Server}/Digikala/CrawlProducts/";
                    var _data = new CrawlProductsDTO
                    {
                        IDs = products.Products.Select(x => x.ProductId).ToList(),
                        UserId = products.UserId
                    };
                    var json = JsonConvert.SerializeObject(_data);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PostAsync(url, data).Result.Content.ReadAsStringAsync();
                }
            }
            else
            {
                using (HttpClient client = _clientFactory.CreateClient())
                {
                    string url = $"{_config.Server}/Digikala/SendProducts/";
                    var json = JsonConvert.SerializeObject(products);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PostAsync(url, data).Result.Content.ReadAsStringAsync();
                }
            }
        }
        #endregion

        #region
        private async Task<string> GetPath(long productId)
        {
            var _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Digikala_Crawl", Math.Round(productId / 1000000.0, 0).ToString());
            var _pathFile = Path.Combine(_path, $"{productId}.json");
            if (Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            return _pathFile;
        }
        public async Task SaveToFile(SetProductDTO product)
        {
            var _pathFile = GetPath(product.ProductId);
            var json = JsonConvert.SerializeObject(product);
            await File.WriteAllTextAsync(await _pathFile, json);
        }
        public async Task SaveToFile(SetProductsDTO products)
        {
            foreach (var product in products.Products)
            {
                await SaveToFile(product);
            }
        }
        #endregion
    }
}