using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Data;
using WebStore.Models;
using System.Linq;
using WebStore.Services.Interfaces;
using Microsoft.Extensions.Logging;
using WebStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Controllers
{
    //[Route("Employees/[action]/{id?}")]
    //[Route("Staff/[action]/{id?}")]
    [Authorize]
    public class EmployeesController : Controller
    {
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

            if (employee is null)
            {
                return NotFound();
            }


            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Phone = employee.Phone,
            };

            return View(model);
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        #region Edit
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View(new EmployeeViewModel());
            }

            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Phone = employee.Phone,
            };

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Name == "Бин" && model.LastName == "Асама" && model.Patronymic == "Ладан")
                ModelState.AddModelError("", "Террористы не пройдут!");

            if (!ModelState.IsValid) return View(model);
           
            var employee = new Employee
            {
                Id = model.Id,
                FirstName = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                Phone = model.Phone,
            };

            if (employee.Id == 0)
            {
                _EmployeesData.Add(employee);
            }
            else
            {
                _EmployeesData.Update(employee);
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region Delete
        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var employee = _EmployeesData.GetById(id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                Phone = employee.Phone,
            });
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrators)]
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
