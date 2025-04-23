using CsvHelper.Configuration;
using CsvHelper;
using EmpLoad.Models.Foundations.Employees;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using EmpLoad.Services.Foundations.EmployeeMaps;
using EmpLoad.Services.Foundations.Employees;
using Microsoft.EntityFrameworkCore;
using System;

namespace EmpLoad.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeServce employeeServce;

        public HomeController(IEmployeeServce employeeServce)=>
            this.employeeServce = employeeServce;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await this.employeeServce
                .RetrieveAllEmployeesAsync();

            return View(employees.OrderBy(e => e.Surname).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                });
                csv.Context.RegisterClassMap<EmployeeMap>();

                var records = csv.GetRecords<Employee>().ToList();
                await this.employeeServce.AddEmployeesAsync(records);
                ViewBag.Message = $"{records.Count} rows successfully imported.";
            }

            var employees = await this.employeeServce.RetrieveAllEmployeesAsync();
            return View("Index", employees.OrderBy(e => e.Surname).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var existing = await this.employeeServce.RetrieveEmployeeByIdAsync(employee.Id);
                if (existing != null)
                {
                    existing.PayrollNumber = employee.PayrollNumber;
                    existing.Forenames = employee.Forenames;
                    existing.Surname = employee.Surname;
                    existing.DateOfBirth = employee.DateOfBirth;
                    existing.Telephone = employee.Telephone;
                    existing.Mobile = employee.Mobile;
                    existing.Address = employee.Address;
                    existing.Address2 = employee.Address2;
                    existing.Postcode = employee.Postcode;
                    existing.EmailHome = employee.EmailHome;
                    existing.StartDate = employee.StartDate;

                    await this.employeeServce.ModifyEmployeeAsync(existing);
                    ViewBag.Message = "Employee updated successfully.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async ValueTask<IActionResult> Delete(int employeeId)
        {
            var employee = await employeeServce.RemoveEmployeeByIdAsync(employeeId);
            TempData["Message"] = employee != null
                ? "Employee deleted successfully."
                : "Employee not found.";

            return RedirectToAction("Index");
        }
    }
}
