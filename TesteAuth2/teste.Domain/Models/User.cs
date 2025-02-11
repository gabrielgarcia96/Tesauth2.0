using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using teste.Domain.Models.Enums;

namespace teste.Domain.Models;

public class User
{
    [BsonId]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Roles { get; set; }
}
