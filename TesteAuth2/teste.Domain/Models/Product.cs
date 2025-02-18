using MongoDB.Bson.Serialization.Attributes;

namespace teste.Domain.Models;

public class Product
{
    [BsonId]
    [BsonElement("code")]
    public string Code { get; set; }
    [BsonElement("name_product")]
    public string NameProduct { get; set; }
    [BsonElement("price_product")]
    public int Price { get; set; }
    public string CodBarras { get; set; }
    public bool Active { get; set; }
}
