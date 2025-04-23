using EmpLoad.Brokers.Storages;
using EmpLoad.Models.Foundations.Employees;
using EmpLoad.Services.Foundations.Employees;
using Moq;
using Tynamix.ObjectFiller;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IEmployeeServce employeeServce;


        public EmployeeServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();

            this.employeeServce = new EmployeeServce
                (storageBroker: this.storageBrokerMock.Object);
        }

        private Employee CreateRandomEmployee()
        {
            return CreateEmployeeFiller().Create();
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
