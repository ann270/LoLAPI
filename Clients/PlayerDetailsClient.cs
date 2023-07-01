using LoLAPI.Models;
using Newtonsoft.Json;

namespace LoLAPI.Clients;

public class PlayerDetailsClient
{
    private HttpClient _client;
    private static string _address;

    public PlayerDetailsClient()
    {
        _address = Constants.AccountsAddress;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_address);
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "bb38d1d1b8msh69e107efafa46b9p1c2754jsne5eaa2106a35");
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "league-of-legends-galore.p.rapidapi.com");
    }

    public async Task <List<PlayerDetails>> GetPlayerDetails(string username, string region)
    {
        var response = await _client.GetAsync($"/api/getPlayerRank?name={username}&region={region}");
        response.EnsureSuccessStatusCode();

        var content = response.Content.ReadAsStringAsync().Result;

        var result = JsonConvert.DeserializeObject<List<PlayerDetails>>(content);
        
        return result;
    }
}
    