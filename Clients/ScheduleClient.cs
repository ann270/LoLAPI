using LoLAPI.Models;
using Newtonsoft.Json;

namespace LoLAPI.Clients;

public class ScheduleClient
{
    private HttpClient _client;
    private static string _address;

    public ScheduleClient()
    {
        _address = Constants.EsportsAddress;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_address);
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "bb38d1d1b8msh69e107efafa46b9p1c2754jsne5eaa2106a35");
        _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "league-of-legends-esports.p.rapidapi.com");
    }

    public async Task <EsportsSchedule> GetSchedule()
    {
        var response = await _client.GetAsync("/schedule?leagueId=98767991299243165%252C99332500638116286%252C98767991302996019");
        response.EnsureSuccessStatusCode();
        var content = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<EsportsSchedule>(content);
        var currentDateTime = DateTime.UtcNow;
        var filteredEvents = result.Data.Schedule.Events
            .Where(e => DateTime.Parse(e.StartTime) > currentDateTime)
            .ToList();
        filteredEvents.Sort((e1, e2) => DateTime.Parse(e1.StartTime).CompareTo(DateTime.Parse(e2.StartTime)));
        var next5Events = filteredEvents.Take(5).ToList();
        var filteredSchedule = new EsportsSchedule
        {
            Data = new Data
            {
                Schedule = new Schedule
                {
                    Events = next5Events
                }
            }
        };
        
        return filteredSchedule;
    }
}