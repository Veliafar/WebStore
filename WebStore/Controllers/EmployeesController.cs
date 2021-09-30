using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Data;
using WebStore.Models;
using System.Linq;
using WebStore.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {


        private readonly IEnumerable<Employee> _Employees;
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesController> _Logger;

        public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        
        //[Route("~/employees/all")]
        public IActionResult Index() => View(_EmployeesData.GetAll());// http://localhost:500/Home/Employees        

        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            //var employee = _Employees.FirstOrDefault(x => x.Id == id);
            var employee = _EmployeesData.GetById(id);

            if(employee is null)
            {
                return NotFound();
            }


            return View(employee);
        }
    }
}
