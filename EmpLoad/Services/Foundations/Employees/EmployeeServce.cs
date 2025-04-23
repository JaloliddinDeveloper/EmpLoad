using EmpLoad.Brokers.Storages;
using EmpLoad.Models.Foundations.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpLoad.Services.Foundations.Employees
{
    public class EmployeeServce: IEmployeeServce
    {
        private readonly IStorageBroker storageBroker;

        public EmployeeServce(IStorageBroker storageBroker)=>
            this.storageBroker = storageBroker;

        public async ValueTask<IEnumerable<Employee>> AddEmployeesAsync(IEnumerable<Employee> employees)=>
            await this.storageBroker.InsertEmployeesAsync(employees);

        public async ValueTask<IQueryable<Employee>> RetrieveAllEmployeesAsync() =>
           await this.storageBroker.SelectAllEmployeesAsync();

        public async ValueTask<Employee> RetrieveEmployeeByIdAsync(int employeeId) =>
           await this.storageBroker.SelectEmployeeByIdAsync(employeeId);

        public async ValueTask<Employee> ModifyEmployeeAsync(Employee employee)=>
            await this.storageBroker.UpdateEmployeeAsync(employee);

        public async ValueTask<Employee> RemoveEmployeeByIdAsync(int employeeId)
        {
            Employee maybeEmployee = await this.storageBroker
                .SelectEmployeeByIdAsync(employeeId);

            return await this.storageBroker
                .DeleteEmployeeAsync(maybeEmployee);
        }
    }
}
