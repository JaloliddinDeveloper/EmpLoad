using EmpLoad.Models.Foundations.Employees;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmpLoad.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Employee> Employees { get; set; }

        public ValueTask<Employee> InsertEmployeeAsync(Employee employee) =>
            InsertAsync(employee);

        public ValueTask<IQueryable<Employee>> SelectAllEmployeesAsync() =>
            SelectAllAsync<Employee>();

        public ValueTask<Employee> SelectEmployeeByIdAsync(int employeeId) =>
            SelectAsync<Employee>(employeeId);

        public ValueTask<Employee> UpdateEmployeeAsync(Employee employee) =>
            UpdateAsync(employee);

        public ValueTask<Employee> DeleteEmployeeAsync(Employee employee) =>
            DeleteAsync(employee);
    }
}
