using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Internal;
using Amazon.Runtime;
using LoLAPI;
using LoLAPI.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<PlayerDetailsClient>();
builder.Services.AddSingleton<ScheduleClient>();
builder.Services.AddSingleton<YouTubeClient>();
builder.Services.AddSingleton<OpenAIClient>();
builder.Services.AddSingleton<ChampionSkinsClient>();

var credentials = new BasicAWSCredentials(Constants.AccessKey, Constants.SecretKey);
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.EUNorth1
};
var client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddSingleton<IDynamoDbClient, DynamoDbClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();