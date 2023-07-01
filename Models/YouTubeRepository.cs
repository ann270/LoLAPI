namespace LoLAPI.Models;

public class YouTubeRepository
{
    public List<Items> Items { get; set; }
}

public class Items
{
    public string Title { get; set; }
    public  string Url { get; set; }
}