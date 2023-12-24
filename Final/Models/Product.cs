using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Final.Models
{   

    //when the product is Display on the screan it should display 2 dropdown bar with category and description , the imagelink need to display a image , all other
    //fleid  need to display text 
    public class Product
    {
        // id for the drop down bar 
        public ObjectId Id { get; set; }


        public string BarCode { get; set; }
        public string ProductName { get; set; }
        public int OriginPrice { get; set; }
        public int DisplayPrice { get; set; }
        // seperate category type 
        public List<string> Category { get; set; }
        // seperate des type 
        public string Description { get; set; }

        // seperate image type 
        public string linkImg { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
