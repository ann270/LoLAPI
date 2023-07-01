using Amazon.DynamoDBv2.DataModel;

namespace LoLAPI.Models;

[DynamoDBTable("schedule-db")]
public class DBRepository
{
    [DynamoDBHashKey("RequestID")]
    public string RequestID { get; set; }
    
    [DynamoDBRangeKey("UserID")]
    public string UserID { get; set; }
    
    [DynamoDBProperty("StartTime")]
    public string StartTime { get; set; }
    
    [DynamoDBProperty("BlockName")]
    public string BlockName { get; set; }
    
    [DynamoDBProperty("LeagueName")]
    public string LeagueName { get; set; }
    
    [DynamoDBProperty("MatchTeams")]
    public List<string> MatchTeams { get; set; }
    
    [DynamoDBProperty("MatchID")]
    public string MatchID { get; set; }
}

