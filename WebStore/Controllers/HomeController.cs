using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {


        

        public IActionResult Index() => View();// http://localhost:500/Home/Index

        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parametr {id}");
        }         
    }
}
