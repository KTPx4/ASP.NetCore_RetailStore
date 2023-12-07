using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Final.Models
{
    public class Account
    {
       
        public ObjectId Id { get; set; }
        public String fullName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String User {  get; set; }

      
        public String Role { get; set; }
        
        
        public bool isActive {  get; set; }

    
        public bool isDeleted { get; set; }
        
       
        public bool firstLogin { get; set; }

    }
}
