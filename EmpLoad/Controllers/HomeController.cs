using CsvHelper.Configuration;
using CsvHelper;
using EmpLoad.Brokers.Storages;
using EmpLoad.Models.Foundations.Employees;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using EmpLoad.Services.Foundations.EmployeeMaps;

namespace EmpLoad.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStorageBroker storageBroker;

        public HomeController(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await this.storageBroker
                .SelectAllEmployeesAsync();

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
                await this.storageBroker.InsertEmployeesAsync(records);
                ViewBag.Message = $"{records.Count} rows successfully imported.";
            }

            var employees = await this.storageBroker.SelectAllEmployeesAsync();
            return View("Index", employees.OrderBy(e => e.Surname).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var existing = await this.storageBroker.SelectEmployeeByIdAsync(employee.Id);
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

                    await this.storageBroker.UpdateEmployeeAsync(existing);
                    ViewBag.Message = "Employee updated successfully.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}
