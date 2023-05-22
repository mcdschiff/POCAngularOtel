using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoNetApi.Models;

namespace MongoNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMongoCollection<Employee> _employeeCollection;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env, IMongoDatabase database)
        {
            _configuration = configuration;
            _env = env;
            _employeeCollection = database.GetCollection<Employee>("Employee");

        }

        [HttpGet]
        public JsonResult Get()

        {
            var dbList= _employeeCollection.AsQueryable();

            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            //var dbList = dbClient.GetDatabase("Store").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int lastEmployeetId = _employeeCollection.AsQueryable().Count();
            emp.EmployeeId = lastEmployeetId + 1;

            _employeeCollection.InsertOne(emp);

            return new JsonResult("Added Succesfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
           //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", emp.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", emp.EmployeeName)
                                                    .Set("Departament", emp.Departament)
                                                    .Set("DateOfJoining", emp.DateOfJoining)
                                                    .Set("PhotoFileName", emp.PhotoFileName);

           _employeeCollection.UpdateOne(filter, update);

            return new JsonResult("Update Succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            //MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId",id);

            _employeeCollection.DeleteOne(filter);

            return new JsonResult("Deleted Succesfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath=_env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create) ) 
                {
                    postedFile.CopyTo(stream);        
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }




    }
}
