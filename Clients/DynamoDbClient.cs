using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using LoLAPI.Models;

namespace LoLAPI.Clients;

public class DynamoDbClient : IDynamoDbClient, IDisposable
{
    public string _tableName;
    private readonly IAmazonDynamoDB _dynamoDb;

    public DynamoDbClient(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
        _tableName = Constants.TableName;
    }

    public async Task PostDataToBd(DBRepository dbRepository)
    {
        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = new Dictionary<string, AttributeValue>
            {
                {"RequestID", new AttributeValue{ S = dbRepository.RequestID } },
                {"UserID", new AttributeValue{ S = dbRepository.UserID } },
                {"StartTime", new AttributeValue{ S = dbRepository.StartTime } },
                {"BlockName", new AttributeValue{ S = dbRepository.BlockName } },
                {"LeagueName", new AttributeValue{ S = dbRepository.LeagueName } },
                {"MatchTeams", new AttributeValue{ SS = dbRepository.MatchTeams } },
                {"MatchID", new AttributeValue{ S = dbRepository.MatchID } }
            }
        };

        var response = await _dynamoDb.PutItemAsync(request);
    }
    
    public async Task GetDataFromDB(DBRepository dbRepository)
    {
        var request = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "RequestID", new AttributeValue { S = dbRepository.RequestID } },
                { "UserID", new AttributeValue { S = dbRepository.UserID } }
            }
        };

        var response = await _dynamoDb.GetItemAsync(request);
        if (response.Item != null)
        {
            dbRepository.StartTime = response.Item["StartTime"].S;
            dbRepository.BlockName = response.Item["BlockName"].S;
            dbRepository.LeagueName = response.Item["LeagueName"].S;
            dbRepository.MatchTeams = response.Item["MatchTeams"].SS.ToList();
            dbRepository.MatchID = response.Item["MatchID"].S;
        }
    }
    
    public async Task DeleteDataFromDB(DBRepository dbRepository)
    {
        var request = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "RequestID", new AttributeValue { S = dbRepository.RequestID } },
                { "UserID", new AttributeValue { S = dbRepository.UserID } }
            }
        };
        await _dynamoDb.DeleteItemAsync(request);
    }
    
    public void Dispose()
    {
        _dynamoDb.Dispose();
    }
}