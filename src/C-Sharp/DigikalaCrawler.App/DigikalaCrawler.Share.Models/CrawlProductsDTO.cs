using System.Collections.Generic;

namespace DigikalaCrawler.Share.Models;
public struct CrawlProductsDTO
{
    public List<long> IDs { get; set; }
    public int UserId { get; set; }
}