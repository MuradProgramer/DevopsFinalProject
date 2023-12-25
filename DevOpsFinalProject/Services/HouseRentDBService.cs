using DevOpsFinalProject.Config;
using DevOpsFinalProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace DevOpsFinalProject.Services;

public class HouseRentDBService : IHouseRentDBService
{

    private readonly IMongoCollection<House> _housesCollection;

    public HouseRentDBService(IOptions<MongoDbConfig> mongoConfig)
    {        
        var client = new MongoClient(mongoConfig.Value.ConnectionString);
        var db = client.GetDatabase(mongoConfig.Value.DatabaseName);
        _housesCollection = db.GetCollection<House>(mongoConfig.Value.Collection);
    }

    public async Task<List<House>> GetByPage(int pageNumber)
    {
        const int pageSize = 5;
        int skip = (pageNumber - 1) * pageSize;

        var houses = await _housesCollection.Find(new BsonDocument())
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

        return houses;
    }

    public async Task Create(House house) 
    { 
        await _housesCollection.InsertOneAsync(house); 
    }

    public async Task CreateMany(List<House> houses) { await _housesCollection.InsertManyAsync(houses); }

    public async Task DeleteAll() { await _housesCollection.DeleteManyAsync(a => a.HostName != null); }
}
