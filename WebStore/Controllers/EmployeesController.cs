using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {

        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27, Phone = "+79997778881" },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Age = 19, Phone = "+79997778882" },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 34, Phone = "+79997778883" },
        };

        public IActionResult Index() => View(__Employees);// http://localhost:500/Home/Employees        

        public IActionResult Employee(int id)
        {
            var employee = __Employees.Find(x => x.Id == id);
            return View(employee);
        }
    }
}
