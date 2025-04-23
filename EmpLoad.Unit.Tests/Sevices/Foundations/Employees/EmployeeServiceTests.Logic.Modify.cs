using EmpLoad.Models.Foundations.Employees;
using Moq;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        [Fact]
        public async Task ShouldModifyEmployeeAsync()
        {
            // given
            Employee randomEmployee = CreateRandomEmployee();

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateEmployeeAsync(randomEmployee))
                .ReturnsAsync(randomEmployee);

            // when
            Employee updatedEmployee =
                await this.employeeServce.ModifyEmployeeAsync(randomEmployee);

            // then
            Assert.Equal(randomEmployee, updatedEmployee);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateEmployeeAsync(randomEmployee),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
