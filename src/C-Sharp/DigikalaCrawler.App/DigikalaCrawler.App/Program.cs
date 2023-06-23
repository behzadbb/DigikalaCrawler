#region Route Map Project
// 1.Get Sitemap
// 2.Read Sitemap sturcture and Get Links from Sitemaps
// 3.Save Links and other information on database
// 4.Get WebPage
// 5.Process and Parse Webpage information
// 6.Save on a Database
// 7.Parallel Programing for Distributed

#endregion

using DigikalaCrawler.Data.Mongo;

//using (DigikalaCrawlerServiceV1 digikalaCrawler = new DigikalaCrawlerServiceV1())
//{
//    //await digikalaCrawler.GetMainSitemap();
//    //await digikalaCrawler.DownloadSitemap("https://dkstatics-public.digikala.com/digikala-site-map/117f96a4d39d008a661ed4d8768ccfa8bcee005a_1647113623.gz",@"d:\");
//    //await digikalaCrawler.GetSitemap(@"d:\117f96a4d39d008a661ed4d8768ccfa8bcee005a_1647113623.xml");
//    //digikalaCrawler.GetProductIdFromUrl(@"https://www.digikala.com/product/dkp-7971541/%D8%AA%DB%8C-%D8%B4%D8%B1%D8%AA-%D8%A2%D8%B3%D8%AA%DB%8C%D9%86-%DA%A9%D9%88%D8%AA%D8%A7%D9%87-%D9%85%D8%B1%D8%AF%D8%A7%D9%86%D9%87-%DA%A9%D8%AF-ar258");
//    //var s = await digikalaCrawler.GetProduct(3246506);
//    var start = DateTime.Now;
//    //var s = await digikalaCrawler.GetProductComments(7353298);
//    var dkp = digikalaCrawler.GetProductIdFromUrl("https://www.digikala.com/product/dkp-968477");
//    var end = DateTime.Now;
//    Console.WriteLine("\n\n\tTime: " + (end - start).Milliseconds);
//    //Console.WriteLine("\n\n\tCount: " + s.comments.Count());
//}
//DigikalaMongo digikalaMongo = new DigikalaMongo();
//digikalaMongo.CreateIndex("ProductId", "CommentDetails");
//var allData = digikalaMongo.GetDigikalaProductCrawl1();
//Console.WriteLine(allData);
//var uniq = allData.Distinct().ToList();
//var str = string.Join("\n", uniq);
//File.WriteAllText(@"d:\digikala_crawl_ids.txt", str);
//Console.WriteLine("File write ---> count: " + uniq);

#region Spilit Comments
//List<string> cm = new List<string>();

//var batch = 10000;
//var TotalId = File.ReadAllLines(@"d:\digikala_crawl_ids.txt").Select(x => long.Parse(x)).ToList();
//var LoopCount = TotalId.Count / batch;
//Console.WriteLine("TotalId: " + TotalId.Count);
//Console.WriteLine("LoopCount: " + LoopCount);
//try
//{
//    for (int i = 0; i <= LoopCount; i++)
//    {
//        var allData1 = digikalaMongo.GetDigikalaProductCrawlOnlyTextById(TotalId.Skip(i * batch).Take(batch).ToList());
//        var msg = allData1.Where(x => x.CommentDetails != null && x.CommentDetails.comments.Count > 0).Select(x => x.CommentDetails.comments.Select(u => u.body)).ToList();
//        var msg1 = allData1.Where(x => x.CommentDetails != null && x.CommentDetails.comments.Count > 0).Select(x => x.CommentDetails.comments.Select(u => u.title)).ToList();
//        var advantages = allData1.Where(x => x.CommentDetails != null && x.CommentDetails.comments.Count > 0).Select(x => x.CommentDetails.comments.Select(u => u.advantages)).ToList();
//        var disadvantages = allData1.Where(x => x.CommentDetails != null && x.CommentDetails.comments.Count > 0).Select(x => x.CommentDetails.comments.Select(u => u.disadvantages.Select(x => x))).ToList();
//        foreach (var item in msg)
//        {
//            cm.AddRange(item);
//        }
//        foreach (var item in msg1)
//        {
//            cm.AddRange(item);
//        }
//        foreach (var item in advantages)
//        {
//            foreach (var item1 in item)
//            {
//                cm.AddRange(item1.Select(x=>x));
//            }
//        }
//        foreach (var item in disadvantages)
//        {
//            foreach (var item1 in item)
//            {
//                cm.AddRange(item1.Select(x => x));
//            }
//        }
//        Console.Write(i + "_");
//        if (i > 0 && i % 10 == 0)
//        {
//            Console.WriteLine("\nComments Count: " + cm.Count());
//        }
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine("\n\nError:\n" + ex.Message + "\n\nStack:\n" + ex.StackTrace.ToString() + "\n\n");
//}


//Console.WriteLine("\n____\nAll Comments Count: " + cm.Count());

//Console.WriteLine("Uniq Comments Count: " + cm.Distinct().Count());
//List<string> final = cm.Where(x => x != null && x != "").Select(x => x.Trim()).Where(x => x != "").Distinct().ToList();
//Console.WriteLine("Tootal Comments Count: " + final.Count() + "\n____\n");
//digikalaMongo.InsertComments(final);
#endregion

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        using (DigikalaMongo digikalaMongo = new DigikalaMongo())
        {
            Console.WriteLine("Hello, World!");
            var comments = digikalaMongo.GetAllComments();
        }
        int a = 0;
    }
}


//digikalaMongo.CreateIndex("_id", "ProductId", "UserId", "Assign", "Success");