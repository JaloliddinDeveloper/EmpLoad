using EmpLoad.Models.Foundations.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpLoad.Services.Foundations.Employees
{
    public interface IEmployeeServce
    {
        ValueTask<IEnumerable<Employee>> AddEmployeesAsync(IEnumerable<Employee> employees);
        ValueTask<IQueryable<Employee>> RetrieveAllEmployeesAsync();
        ValueTask<Employee> ModifyEmployeeAsync(Employee employee);
        ValueTask<Employee> RemoveEmployeeByIdAsync(int employeeId);
        ValueTask<Employee> RetrieveEmployeeByIdAsync(int employeeId);
    }
}
