using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Final.Models
{
    public class OrderDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("OrderID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId OrderID { get; set; }

        public string BarCodeID { get; set; }

        public int Quantity { get; set; }

        public int TotalPrice { get; set; }
    }
}
