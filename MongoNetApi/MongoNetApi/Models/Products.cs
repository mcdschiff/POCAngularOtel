using MongoDB.Bson;

namespace MongoNetApi.Models
{
    public class Products
    {
        public ObjectId Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; }
       
    }
}
