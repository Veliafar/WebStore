using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {


        private readonly List<Employee> _Employees;

        public EmployeesController()
        {
            _Employees = TestData.Employees;
        }

        

        public IActionResult Index() => View(_Employees);// http://localhost:500/Home/Employees        

        public IActionResult Employee(int id)
        {
            var employee = _Employees.Find(x => x.Id == id);
            return View(employee);
        }
    }
}
