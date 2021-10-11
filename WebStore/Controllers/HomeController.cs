using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();// http://localhost:500/Home/Index

        //public IActionResult Page404() => View();

        public IActionResult Blogs() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult Cart() => View();

        public IActionResult Checkout() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Login() => View();

        public IActionResult ProductDetails() => View();


        public IActionResult Status(string id)
        {
            return id switch
            {
                "404" => View("Page404"),
                _ => Content($"Status --- {id}")                
            };
        }
    }
}
