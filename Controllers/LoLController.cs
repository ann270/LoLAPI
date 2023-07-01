using LoLAPI.Clients;
using LoLAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LoLAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoLController : ControllerBase
{
    private readonly ILogger<LoLController> _logger;
    private readonly PlayerDetailsClient _playerDetails;
    private readonly ScheduleClient _scheduleClient;
    private readonly IDynamoDbClient _dynamoDbClient;
    private readonly YouTubeClient _youTubeClient;
    private readonly ChampionSkinsClient _championSkinsClient;

    public LoLController(ILogger<LoLController> logger, PlayerDetailsClient playerDetailsClient, ScheduleClient scheduleClient, IDynamoDbClient dynamoDbClient, YouTubeClient youTubeClient, OpenAIClient openAiClient, ChampionSkinsClient championSkinsClient)
    {
        _logger = logger;
        _playerDetails = playerDetailsClient;
        _scheduleClient = scheduleClient;
        _dynamoDbClient = dynamoDbClient;
        _youTubeClient = youTubeClient;
        _championSkinsClient = championSkinsClient;
    }

    [HttpGet("GetAccountStatistic")]
    public async Task<List<PlayerDetails>> GetPlayerDetails([FromQuery] PlayerParameters parameters)
    {
        var player = await _playerDetails.GetPlayerDetails(parameters.Username, parameters.Region);
        return player;
    }

    [HttpGet("GetChampionSkins")]
    public async Task<ChampionSkins> GetChampionSkins([FromQuery] ChampionSkinsParameters championSkinsParameters)
    {
        var champion = await _championSkinsClient.GetChampionSkins(championSkinsParameters.Name);
        return champion;
    }

    [HttpGet("GetSchedule")]
    public async Task <EsportsSchedule> GetSchedule()
    {
        var schedule = await _scheduleClient.GetSchedule();
        
        return schedule;
    }

    [HttpPost("AddToDB")]
    public async Task<IActionResult> AddToDb([FromBody] DBRepository dbRepository, [FromServices] IHttpContextAccessor httpContextAccessor)
    {
        string RequestID = httpContextAccessor.HttpContext.TraceIdentifier;
        dbRepository.RequestID = RequestID;
        await _dynamoDbClient.PostDataToBd(dbRepository);
        return Ok();
    }

    [HttpGet("GetDataFromDB")]
    public async Task<IActionResult> GetDataFromDB(string requestID, string userID)
    {
        DBRepository dbRepository = new DBRepository
        {
            RequestID = requestID,
            UserID = userID
        };
        await _dynamoDbClient.GetDataFromDB(dbRepository);
        return Ok(dbRepository);
    }
    
    [HttpDelete("DeleteDataFromDB")]
    public async Task<IActionResult> DeleteDataFromDB(string requestID, string userID)
    {
        DBRepository dbRepository = new DBRepository
        {
            RequestID = requestID,
            UserID = userID
        };

        await _dynamoDbClient.DeleteDataFromDB(dbRepository);
        return Ok();
    }

    [HttpGet("YouTubeSearch")]
    public async Task<YouTubeRepository> GetYouTubeVideo([FromQuery] YouTubeParameters parameters)
    {
        var search = await _youTubeClient.GetYouTubeVideo(parameters.Keyword);
        return search;
    }

    [HttpPost("OpenAI")]
    public async Task<ActionResult<OpenAiResponse>> OpenAi([FromBody] OpenAiRepository openAiRepository)
    {
        var client = new OpenAIClient();
        var result = await client.OpenAI(openAiRepository.messages.FirstOrDefault()?.content);
        var openAiResponse = new OpenAiResponse
        {
            Content = ParseContentFromSwaggerResponse(result)
        };
        return Ok(openAiResponse);
    }
    
    private string ParseContentFromSwaggerResponse(string swaggerResponse)
    {
        var parsedResponse = JObject.Parse(swaggerResponse);
        var content = parsedResponse["choices"]?.FirstOrDefault()?["message"]?["content"]?.ToString();
        return content;
    }
}

