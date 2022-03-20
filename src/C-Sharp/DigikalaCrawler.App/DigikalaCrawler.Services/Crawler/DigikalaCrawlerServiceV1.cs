using RestSharp;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Xml;

namespace DigikalaCrawler.Services.Crawler;

public class DigikalaCrawlerServiceV1
{
    public Task<RestResponse> GetHttp(string url)
    {
        RestClient client = new RestClient(url);
        RestRequest request = new RestRequest();
        return client.ExecuteGetAsync(request);
    }

    #region Sitemap
    public async Task<IList<string>> GetMainSitemap()
    {
        RestResponse response = await GetHttp("https://www.digikala.com/sitemap.xml");
        List<string> locs = new List<string>();
        if (response.IsSuccessful)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response.Content);
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
        return int.Parse( url.Substring(url.LastIndexOf("dkp-"),url.LastIndexOf('/')-url.LastIndexOf("dkp-")).Replace("dkp-",""));
    }

    public int[] GetProductIdFromUrls(List<string> urls)
    {
        return urls.Select(x => GetProductIdFromUrl(x)).ToArray();
    }
    #endregion


}