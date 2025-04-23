using EmpLoad.Models.Foundations.Employees;
using Moq;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllEmployeesAsync()
        {
            //given
            IQueryable<Employee> randomEmployees = CreateRandomEmployees().AsQueryable();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEmployeesAsync())
                .ReturnsAsync(randomEmployees);

            // when
            IQueryable<Employee> actualEmployees =
                await this.employeeServce.RetrieveAllEmployeesAsync();

            // then
            Assert.Equal(randomEmployees, actualEmployees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllEmployeesAsync(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
