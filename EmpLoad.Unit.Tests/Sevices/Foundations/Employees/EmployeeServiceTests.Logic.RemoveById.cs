using EmpLoad.Models.Foundations.Employees;
using Moq;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        [Fact]
        public async Task ShouldRemoveEmployeeByIdAsync()
        {
            // given
            int randomId = new Random().Next(1, 1000);
            Employee randomEmployee = CreateRandomEmployee();
            randomEmployee.Id = randomId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectEmployeeByIdAsync(randomId))
                .ReturnsAsync(randomEmployee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteEmployeeAsync(randomEmployee))
                .ReturnsAsync(randomEmployee);

            // when
            Employee deletedEmployee =
                await this.employeeServce.RemoveEmployeeByIdAsync(randomId);

            // then
            Assert.Equal(randomEmployee, deletedEmployee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectEmployeeByIdAsync(randomId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteEmployeeAsync(randomEmployee),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
