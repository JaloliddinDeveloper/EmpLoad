using EmpLoad.Controllers;
using EmpLoad.Models.Foundations.Employees;
using EmpLoad.Services.Foundations.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tynamix.ObjectFiller;

namespace EmpLoad.Services.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IEmployeeServce> employeeServiceMock;
        private readonly HomeController homeController;

        public HomeControllerTests()
        {
            this.employeeServiceMock = new Mock<IEmployeeServce>();
            this.homeController = new HomeController(this.employeeServiceMock.Object);
        }

        [Fact]
        public async Task ShouldReturnViewWithSortedEmployeesOnIndex()
        {
            var employees = CreateRandomEmployees().ToList();

            var expectedEmployees = employees.OrderBy(e => e.Surname).ToList();

            this.employeeServiceMock.Setup(service =>
                    service.RetrieveAllEmployeesAsync())
                        .ReturnsAsync(employees.AsQueryable());

            // given
            IActionResult result = await this.homeController.Index();

            // when, then
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);

            Assert.Equal(expectedEmployees.Select(e => e.Surname)
                    , model.Select(e => e.Surname));

            this.employeeServiceMock.Verify(service =>
                service.RetrieveAllEmployeesAsync(), Times.Once);

            this.employeeServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldImportCsvAndReturnSortedIndexView()
        {
            // given
            string csvContent = "Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date\n" +
                                "COOP08,John ,William,26/01/1955,12345678,987654231,12 Foreman road,London,GU12 6JW,nomadic20@hotmail.co.uk,18/04/2013\n" +
                                "JACK13,Jerry,Jackson,11/5/1974,2050508,6987457,115 Spinney Road,Luton,LU33DF,gerry.jackson@bt.com,18/04/2013";

            IFormFile file = CreateMockFormFile(csvContent);

            var importedEmployees = new List<Employee>
                    {
                        new Employee
                        {
                            PayrollNumber = "COOP08",
                            Forenames = "John",
                            Surname = "William",
                            DateOfBirth = DateTime.Parse("26/01/1955"),
                            Telephone = "12345678",
                            Mobile = "987654231",
                            Address = "12 Foreman road",
                            Address2 = "London",
                            Postcode = "GU12 6JW",
                            EmailHome = "nomadic20@hotmail.co.uk",
                            StartDate = DateTime.Parse("18/04/2013")
                        },
                        new Employee
                        {
                            PayrollNumber = "JACK13",
                            Forenames = "Jerry",
                            Surname = "Jackson",
                            DateOfBirth = DateTime.Parse("11/5/1974"),
                            Telephone = "2050508",
                            Mobile = "6987457",
                            Address = "115 Spinney Road",
                            Address2 = "Luton",
                            Postcode = "LU33DF",
                            EmailHome = "gerry.jackson@bt.com",
                            StartDate = DateTime.Parse("18/04/2013")
                        }
                    };

            var employeeServiceMock = new Mock<IEmployeeServce>();
            employeeServiceMock.Setup(s => s.AddEmployeesAsync(It.IsAny<IEnumerable<Employee>>()))
                .ReturnsAsync(importedEmployees);

            employeeServiceMock.Setup(s => s.RetrieveAllEmployeesAsync())
                .ReturnsAsync(importedEmployees.AsQueryable());

            var controller = new HomeController(employeeServiceMock.Object);

            // when
            var result = await controller.Import(file);

            // then
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);

            var model = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);
            Assert.Equal(2, model.Count);
            Assert.Equal("Jackson", model.First().Surname);

            employeeServiceMock.Verify(s => s.AddEmployeesAsync(It.IsAny<IEnumerable<Employee>>()), Times.Once);
            employeeServiceMock.Verify(s => s.RetrieveAllEmployeesAsync(), Times.Once);
        }

        private IFormFile CreateMockFormFile(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", "test.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };
        }

        public IEnumerable<Employee> CreateRandomEmployees()
        {
            return CreateEmployeeFiller().Create(3);
        }

        private static Filler<Employee> CreateEmployeeFiller()
        {
            var filler = new Filler<Employee>();

            return filler;
        }
    }
}
