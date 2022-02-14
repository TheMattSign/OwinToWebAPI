using OwinAPI.Models;
using System;
using System.Web.Http;

namespace OwinAPI.Controller
{
    public class MachineNameController : ApiController
    {
        public string Get()
        {
            return Environment.MachineName;
        }

        public TestModel Post([FromBody]TestInput input)
        {
            return new TestModel {
                Name = input.FirstName + " " + input.LastName,
                CallTime = DateTime.Now
            };
        }
    }
}
