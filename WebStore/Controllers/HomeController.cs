using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() // http://localhost:500/Home/Index
        {
            return Content("Hello from first Controller");
        }

        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parametr {id}");
        }
    }
}
