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
    }
}
