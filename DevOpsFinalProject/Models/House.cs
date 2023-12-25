using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DevOpsFinalProject.Models;

public class House
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }

    public string? HostName { get; set; }

    public bool IsForRent { get; set; }

    public string? ImageUrl { get; set; }
}
