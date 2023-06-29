using System;

namespace DigikalaCrawler.Share.Models;
public static class TicketState
{
    public static DateTime Last { get; set; }
    public static int Diff => (int)(DateTime.Now - Last).TotalMilliseconds;
    public static void SetNew() => Last = DateTime.Now;
}