using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Task<Employee> GetEmployeeByIdAsync(int id);
        public Task<IEnumerable<Employee>> GetAllEmployeesAsync();

    }
}
