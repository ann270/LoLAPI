using LoLAPI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace LoLAPI.Clients;

public class OpenAIClient
{
    public async Task<string> OpenAI(string userMessage)
    {
        var client = new HttpClient();
        var apiUrl = "https://chatgpt-best-price.p.rapidapi.com/v1/chat/completions";
        var rapidApiKey = "69d7dc43d6msh39a8df6283d60e1p11c354jsn4ef5d9f548cd";
        var rapidApiHost = "chatgpt-best-price.p.rapidapi.com";

        var model = "gpt-3.5-turbo";
        var role = "user";
        var content = $"Write several champions who counter {userMessage} in the game League of Legends";
        var requestData = new
        {
            model,
            messages = new[]
            {
                new
                {
                    role,
                    content
                }
            }
        };
        var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(apiUrl),
            Headers =
            {
                { "X-RapidAPI-Key", rapidApiKey },
                { "X-RapidAPI-Host", rapidApiHost },
            },
            Content = requestContent
        };
        request.Content = requestContent;
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return body;
        }
    }
}



