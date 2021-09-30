using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Data;
using WebStore.Models;
using System.Linq;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {


        private readonly IEnumerable<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        
        //[Route("~/employees/all")]
        public IActionResult Index() => View(_Employees);// http://localhost:500/Home/Employees        

        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            //var employee = _Employees.FirstOrDefault(x => x.Id == id);
            var employee = _Employees.SingleOrDefault(x => x.Id == id);

            if(employee is null)
            {
                return NotFound();
            }


            return View(employee);
        }

        public IActionResult TestAction(string Param1, int Param2)
        {
            return Content($"P1:{Param1} - P2:{Param2}");
        }
    }
}
