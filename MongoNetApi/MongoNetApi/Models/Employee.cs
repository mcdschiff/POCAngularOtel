using MongoDB.Bson;

namespace MongoNetApi.Models
{
    public class Employee
    {
        public ObjectId Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Departament { get; set; }
        public string DateOfJoining { get; set; } 
        public string PhotoFileName { get; set; }

    }
}
