using EmpLoad.Models.Foundations.Employees;
using Moq;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        [Fact]
        public async Task ShouldAddEmployeesAsync()
        {
            //given
            IEnumerable<Employee> randomEmployees = CreateRandomEmployees();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertEmployeesAsync(randomEmployees))
                .ReturnsAsync(randomEmployees);

           //when
            var addedEmployees = await this.employeeServce.AddEmployeesAsync(randomEmployees);

            //then
            Assert.Equal(randomEmployees, addedEmployees);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertEmployeesAsync(randomEmployees),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
