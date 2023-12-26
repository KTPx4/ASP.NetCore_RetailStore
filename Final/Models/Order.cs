using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using static NuGet.Packaging.PackagingConstants;

namespace Final.Models
{
    public class Order
    {
        public ObjectId Id { get; set; }

        public string StaffEmail { get; set; }

        public string CustomerPhone { get; set; }

        public int ToltalPayment { get; set; }

        public int MoneyGiven { get; set; }

        public int MoneyExchange { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreateAt { get; set; }
       

    }
}
