using EmpLoad.Models.Foundations.Employees;
using Moq;

namespace EmpLoad.Unit.Tests.Sevices.Foundations.Employees
{
    public partial class EmployeeServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveEmployeeByIdAsync()
        {
            //given
            int randomId = new Random().Next(1, 1000);
            Employee randomEmployee = CreateRandomEmployee();
            randomEmployee.Id = randomId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectEmployeeByIdAsync(randomId))
                .ReturnsAsync(randomEmployee);

            // when
            Employee actualEmployee =
                await this.employeeServce.RetrieveEmployeeByIdAsync(randomId);

            // then
            Assert.Equal(randomEmployee, actualEmployee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectEmployeeByIdAsync(randomId),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
