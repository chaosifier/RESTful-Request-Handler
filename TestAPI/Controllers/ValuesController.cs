using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet("GetFormattedResponse")]
        public object GetFormattedResponse()
        {
            return ResponseHelper.CreateFormattedResponse(true, "Test", "test message");
        }

        [HttpGet("GetInt")]
        public object GetInt()
        {
            return 5;
        }

        [HttpGet("GetNull")]
        public object GetNull()
        {
            return null;
        }

        [HttpGet("GetIntList")]
        public object GetIntList()
        {
            return new List<int> { 10, 11, 12, 14 };
        }

        [HttpGet("GetBool")]
        public object GetBool()
        {
            return true;
        }

        [HttpGet("GetBoolList")]
        public object GetBoolList()
        {
            return new List<bool> { true, true, false, true, true };
        }

        [HttpGet("GetString")]
        public object GetString()
        {
            return "Hello world!";
        }

        [HttpGet("GetStringList")]
        public object GetStringList()
        {
            return new List<string>() { "one", "two", "three" };
        }

        [HttpGet("GetDateTime")]
        public object GetDateTime()
        {
            return DateTime.Now;
        }


        [HttpGet("GetModel")]
        public object GetModel()
        {
            return new TestModel();
        }


        [HttpGet("GetModelList")]
        public object GetModelList()
        {
            return
                new List<TestModel>(){
                    new TestModel(),
                    new TestModel(),
                    new TestModel(),
                    new TestModel()
                };
        }

        public class TestModel
        {
            public int ID { get; set; } = 1;
            public string Name { get; set; } = "Test Bahadur";
            public DateTime Date { get; set; } = DateTime.Now;
            public double Salary { get; set; } = 100.12;
        }
    }
}
