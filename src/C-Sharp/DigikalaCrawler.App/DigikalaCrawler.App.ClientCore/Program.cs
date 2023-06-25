using DigikalaCrawler.Share.Models;
using DigikalaCrawler.Share.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

Montoring monitoring = new Montoring();
Console.WriteLine("Start: {0}", DateTime.Now);
Thread.Sleep(1000);
Config _config;
loadConfig();
var checkUserId = true;
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"#_ \t| ALL  \t\t| Lst \t| Lod \t| Crwl \t| Snd \t| Avg \t| PPD \t| PCM \t| CM \t| CMs \t| H \t| CmH \t\t| Error ____");
Console.ForegroundColor = ConsoleColor.White;

IHost _host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpClient("ConfiguredHttpMessageHandler")
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    UseDefaultCredentials = true,

                });
        services.AddTransient<DigikalaCrawlerServiceV1>();
    }).ConfigureLogging(logging =>
    {
        logging.ClearProviders();

        //... add my providers here
    }).Build();
_host.Start();

Stopwatch sw = new Stopwatch();
while (!string.IsNullOrEmpty(_config.Server))
{
    sw.Restart();

    using (DigikalaCrawlerServiceV1 digi = _host.Services.GetRequiredService<DigikalaCrawlerServiceV1>())
    {
        digi.SetConfig(_config);
        List<long> ids = await digi.GetFreeProductsFromServer(checkUserId);
        monitoring.LoadTimeProducts = Math.Round((double)sw.ElapsedMilliseconds / 1000, 1);
        int random = new Random().Next(3, 50);
        //Console.WriteLine("Random: {0}", random);
        Thread.Sleep(random);
        if (ids != null && ids.Any())
        {
            checkUserId = false;
            SetProductsDTO products = new SetProductsDTO();
            products.UserId = _config.UserId;
            for (int i = 0; i < ids.Count(); i++)
            {
                var product = new SetProductDTO();
                product.ProductId = ids[i];
                try
                {
                    product.Product = digi.GetProduct(ids[i]).Result;
                    product.CommentsCount = product.Product.product.comments_count;
                    Thread.Sleep(random);
                    if (product.Product != null && product.Product.product.comments_count > 0)
                    {
                        for (int k = 1; k < 3; k++)
                        {
                            product.CommentData = digi.GetProductComments(ids[i]).Result;
                            product.SendCommentsCount = product.CommentData.Comments.Count();
                            if (product.CommentData == null || product.CommentData.Comments == null || Math.Min(product.Product.product.comments_count, 2000) > product.CommentData.Comments.Count())
                            {
                                Console.WriteLine("\n\n\t\t\tComment Error\n\n\n\n");
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (product.Product.product.comments_count > product.CommentData.Comments.Count())
                        {

                        }
                        if (product.CommentData == null || product.CommentData.Comments == null || Math.Min(product.Product.product.comments_count, 2000) > product.CommentData.Comments.Count())
                        {
                            throw new ArgumentException("Comment Count");
                        }
                    }
                    Thread.Sleep(random);
                    if (product.Product != null && product.Product.product.questions_count > 0)
                    {
                        product.Questions = digi.GetQuestions(ids[i]).Result;
                        for (int k = 1; k < 3; k++)
                        {
                            product.Questions = digi.GetQuestions(ids[i]).Result;
                            if (product.Questions == null || product.Questions.questions == null || Math.Min(product.Product.product.questions_count, 1000) > product.Questions.questions.Count())
                            {
                                Console.WriteLine($"\tQuestion Error:\tdkp-{ids[i]}\ttry={k}");
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (product.Questions == null || product.Questions.questions == null || product.Product.product.questions_count > product.Questions.questions.Count())
                        {
                            throw new ArgumentException("Question Count");
                        }
                        try
                        {

                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ++monitoring.ClientError;
                    product.Error = true;
                    product.ErrorMessage = ex.Message;
                    product.ClientError = true;
                }
                products.Products.Add(product);
            }
            monitoring.LastCrawlTimeProducts = Math.Round((double)sw.ElapsedMilliseconds / 1000 - monitoring.LoadTimeProducts, 1);
            if (products.Products.Count() > 0)
            {
                try
                {
                    monitoring.LastCommentCount = products.Products.Where(y => y.Product != null).Select(x => x.Product.product).Sum(x => x.comments_count);
                    monitoring.TotalCommentCount += monitoring.LastCommentCount;
                    monitoring.TotalProductCount += products.Products.Count();
                }
                catch
                {

                }
                monitoring.ProductComments = products.Products.Count(x => x.CommentData != null);
                digi.SendProductsServer(products);
                monitoring.LastSendToServerTimeProducts = Math.Abs(Math.Round(((double)sw.ElapsedMilliseconds / 1000) - monitoring.LoadTimeProducts - monitoring.LastCrawlTimeProducts, 1));
            }
        }
    }

    sw.Stop();
    monitoring.Last = Math.Abs(Math.Round((double)sw.ElapsedMilliseconds / 1000, 1));
    monitoring.TimeSheet.Add(monitoring.Last);
    Calc();
    Thread.Sleep(300);
}

void Calc()
{
    if (monitoring.K > 0 && monitoring.K % 2 == 0)
    {
        try
        {
            monitoring.TimeSheet.Remove(monitoring.TimeSheet.Max());
            monitoring.AvrageCrawling = Math.Round(monitoring.TimeSheet.Average());
            monitoring.HoursDurration = Math.Round((DateTime.Now - monitoring.StartTime).TotalHours, 2);
            monitoring.CountPerHours = Convert.ToInt32(monitoring.TotalCommentCount / monitoring.HoursDurration);
        }
        catch
        {
        }
        Thread.Sleep(100);
        if (monitoring.K % 10 == 0)
        {
            Thread.Sleep(1000);
            if (monitoring.K % 100 == 0)
            {
                Thread.Sleep(10000);
                if (monitoring.K % 20000 == 0)
                {
                    Thread.Sleep(60 * 1000);
                }
            }
        }
    }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"\r{String.Format("{0:00000}", ++monitoring.K)}\t");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"| {String.Format("{0:000000}", monitoring.TotalProductCount)} \t| {monitoring.Last} \t| {monitoring.LoadTimeProducts} \t| {monitoring.LastCrawlTimeProducts} \t| {monitoring.LastSendToServerTimeProducts} \t| {monitoring.AvrageCrawling} \t| {String.Format("{0:000}", monitoring.ProductPerDay)}k \t| {String.Format("{0:00}", monitoring.ProductComments)} \t| {monitoring.LastCommentCount} \t| {(monitoring.TotalCommentCount < 10000 ? monitoring.TotalCommentCount : Math.Round((double)(monitoring.TotalCommentCount / 1000), 1) + "_k")} \t| {monitoring.HoursDurration} \t| {(monitoring.CountPerHours > 1000 ? Math.Round((double)(monitoring.CountPerHours / 1000), 1) + "_k" : monitoring.CountPerHours)} \t\t| {String.Format("{0:000000}", monitoring.ClientError)}");
}

void loadConfig()
{
    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DigikalaCrawler.config");
    if (!File.Exists(path))
    {
        //_config = new Config { Server = "http://185.147.160.124:5000", UserId = new Random().Next(100, int.MaxValue), Count = 10, UseProxy = false, ProxyHost = "127.0.0.1", ProxyPort = 9150, LocalDatabase = false };
        _config = new Config { Server = "https://localhost:5001", UserId = new Random().Next(0, 100), Count = 10, UseProxy = false, ProxyHost = "127.0.0.1", ProxyPort = 9150, LocalDatabase = false };
        File.WriteAllText(path, JsonConvert.SerializeObject(_config));
    }
    else
    {
        string content = File.ReadAllText(path);
        _config = JsonConvert.DeserializeObject<Config>(content);
        Console.WriteLine(content);
    }
}