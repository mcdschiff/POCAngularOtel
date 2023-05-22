using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoNetApi.Models;

namespace MongoNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentController : ControllerBase
    {
        private readonly IMongoCollection<Departament> _departamentCollection;
        private readonly IConfiguration _configuration;
        public DepartamentController(IConfiguration configuration, IMongoDatabase database)
        {
            _configuration = configuration;
            _departamentCollection = database.GetCollection<Departament>("Departament");
        }

        [HttpGet]
        public JsonResult Get()
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList= _departamentCollection.AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Departament dep)
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

           int lastDepartamentId = _departamentCollection.AsQueryable().Count();
            dep.DepartamentId = lastDepartamentId + 1;

            _departamentCollection.InsertOne(dep);

            return new JsonResult("Added Succesfully");
        }

        [HttpPut]
        public JsonResult Put(Departament dep)
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Departament>.Filter.Eq("DepartamentId",dep.DepartamentId);
            var update = Builders<Departament>.Update.Set("DepartamentName", dep.DepartamentName);

            _departamentCollection.UpdateOne(filter,update);

            return new JsonResult("Update Succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
           // MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Departament>.Filter.Eq("DepartamentId",id);
           
            _departamentCollection.DeleteOne(filter);

            return new JsonResult("Deleted Succesfully");
        }

    }
}
