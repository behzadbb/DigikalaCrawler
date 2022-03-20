using DigikalaCrawler.Models;
using DigikalaCrawler.Models.Comments;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Xml;
using File = System.IO.File;

namespace DigikalaCrawler.Services.Crawler;

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

    public Task<HttpResponseMessage> GetHttp(string url)
    {
        HttpClient client = new HttpClient();
        return client.GetAsync(url);
    }

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

    public async Task DownloadSitemap(string url, string savePath)
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

    public int[] GetProductIdFromUrls(List<string> urls)
    {
        return urls.Select(x => GetProductIdFromUrl(x)).ToArray();
    }
    #endregion

    public async Task<ProductData> GetProduct(int productId)
    {
        string url = "https://api.digikala.com/v1/product/" + productId + "/";
        ProductData product = new ProductData();
        return await new HttpClient().GetFromJsonAsync<ProductData>(url);
    }

    public async Task<CommentObjV1> GetProductComment(int productId, int page = 1)
    {
        string url = $"https://api.digikala.com/v1/product/{productId}/comments/?page={page}&order=created_at";
        return await new HttpClient().GetFromJsonAsync<CommentObjV1>(url);
    }

    public async Task<CommentDetails> GetProductComments(int productId)
    {
        var cm = (await GetProductComment(productId, 1)).data;
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
}