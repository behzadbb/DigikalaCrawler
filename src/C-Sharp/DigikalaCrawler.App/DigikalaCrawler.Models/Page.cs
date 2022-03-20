namespace DigikalaCrawler.Models;

public class Page
{
    public int ProductId { get; set; }
    public int? UserId { get; set; }
    public DateTime? AssignDate { get; set; }
    public bool Crawl { get; set; } = false;
    public bool Success { get; set; } = false;
}