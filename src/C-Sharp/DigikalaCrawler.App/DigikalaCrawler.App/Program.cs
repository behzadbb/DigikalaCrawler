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
DigikalaMongo digikalaMongo = new DigikalaMongo();

digikalaMongo.CreateIndex("_id", "ProductId", "UserId", "Assign", "Success");