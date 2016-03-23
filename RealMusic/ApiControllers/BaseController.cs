using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RealMusic.Db;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RealMusic.ApiControllers
{
    public class BaseController : Controller
    {
        protected readonly IMapStore Data;

        public BaseController()
        {
            
        }

        public BaseController(IMapStore data)
        {
            Data = data;
        }
    }
}
