using MongoDB.Bson.Serialization.Attributes;

namespace teste.Domain.Models;

public class Product
{
    [BsonId]
    [BsonElement("id")]
    public string id { get; set; } = Guid.NewGuid().ToString();
    [BsonElement("code_product")]
    public int Code { get; set; }
    [BsonElement("name_product")]
    public string NameProduct { get; set; }
    [BsonElement("price_product")]
    public double Price { get; set; }
    [BsonElement("codbarras_product")]
    public string? CodBarras { get; set; }
    [BsonElement("image_url")]
    public string? ImageUrl { get; set; }
    [BsonElement("stats_product")]
    public bool Active { get; set; }
}
