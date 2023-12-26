using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Final.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Phone { get; set; }

        public string fullName { get; set; }

        public string Address { get; set; }

       
    }
}
