using EmpLoad.Models.Foundations.Employees;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpLoad.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Employee> Employees { get; set; }

        public async ValueTask<IEnumerable<Employee>> InsertEmployeesAsync(IEnumerable<Employee> employees)
        {
            foreach (var employee in employees)
            {
                this.Entry(employee).State = EntityState.Added;
            }

            await this.SaveChangesAsync();

            foreach (var employee in employees)
            {
                DetachEntity(employee);
            }

            return employees;
        }


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
