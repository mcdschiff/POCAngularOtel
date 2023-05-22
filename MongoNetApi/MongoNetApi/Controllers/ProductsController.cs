using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoNetApi.Models;

namespace MongoNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMongoCollection<Products> _productCollection;
        private readonly IConfiguration _configuration;
      
        public ProductsController(IConfiguration configuration, IMongoDatabase database)
        {
            _configuration = configuration;
            _productCollection = database.GetCollection<Products>("Products");
        }

        [HttpGet]
        public JsonResult Get()
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = _productCollection.AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Products prod)
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int lastProductId = _productCollection.AsQueryable().Count();
            prod.ProductId = lastProductId + 1;

            _productCollection.InsertOne(prod);

            return new JsonResult("Added Succesfully");
        }

        [HttpPut]
        public JsonResult Put(Products prod)
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Products>.Filter.Eq("ProductId", prod.ProductId);
            var update = Builders<Products>.Update.Set("ProductName", prod.ProductName)
                                                    .Set("ProductPrice", prod.ProductPrice)
                                                    .Set("ProductCategory", prod.ProductCategory);
                                                    

            _productCollection.UpdateOne(filter, update);

            return new JsonResult("Update Succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Products>.Filter.Eq("ProductId", id);

            _productCollection.DeleteOne(filter);

            return new JsonResult("Deleted Succesfully");
        }

    }
}
