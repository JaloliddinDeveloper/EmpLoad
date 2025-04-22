using EmpLoad.Models.Foundations.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpLoad.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<IEnumerable<Employee>> InsertEmployeesAsync(IEnumerable<Employee> employees);
        ValueTask<IQueryable<Employee>> SelectAllEmployeesAsync();
        ValueTask<Employee> SelectEmployeeByIdAsync(int employeeId);
        ValueTask<Employee> UpdateEmployeeAsync(Employee employee);
        ValueTask<Employee> DeleteEmployeeAsync(Employee employee);

    }
}
