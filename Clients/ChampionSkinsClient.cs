using LoLAPI.Models;
using Newtonsoft.Json;

namespace LoLAPI.Clients;

public class ChampionSkinsClient
{
    private HttpClient _client;
    private static string _address;
    
    public ChampionSkinsClient()
    {
        _address = Constants.ChampionSkinsAddress;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_address);
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "69d7dc43d6msh39a8df6283d60e1p11c354jsn4ef5d9f548cd");
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "league-of-legends-champions.p.rapidapi.com");
    }
    
    public async Task<ChampionSkins> GetChampionSkins(string name)
    {
        var response = await _client.GetAsync($"champions/en-us/{name}");
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<ChampionSkins>(content);
        return result;
    }
}