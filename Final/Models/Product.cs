using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Final.Models
{
    public class Product
    {

        public ObjectId Id { get; set; }


        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public int OriginPrice { get; set; }
        public int DisplayPrice { get; set; }
        public List<string> Category { get; set; }
        public string Description { get; set; }
        public string linkImg { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
