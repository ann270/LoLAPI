using LoLAPI.Models;
using Newtonsoft.Json;

namespace LoLAPI.Clients;

public class YouTubeClient
{
    private HttpClient _client;
    private static string _address;
    
    public YouTubeClient()
    {
        _address = Constants.YouTubeAddress;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_address);
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "69d7dc43d6msh39a8df6283d60e1p11c354jsn4ef5d9f548cd");
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "youtube-search-results.p.rapidapi.com");
    }

    public async Task<YouTubeRepository> GetYouTubeVideo(string keyword)
    {
        var response = await _client.GetAsync($"/youtube-search/?q={keyword}");
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<YouTubeRepository>(content);
        result.Items = result.Items.Take(10).ToList();
        return result;
    }
}