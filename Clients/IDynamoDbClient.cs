using LoLAPI.Models;

namespace LoLAPI.Clients;

public interface IDynamoDbClient
{
    public Task PostDataToBd(DBRepository dbRepository);
    public Task GetDataFromDB(DBRepository dbRepository);
    public Task DeleteDataFromDB(DBRepository dbRepository);
}