using Application.DTOs.EmployeeDTOs;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class EmployeeRepo : GenericRepo<Employee>, IEmployeeRepository
    {
        public EmployeeRepo(Context context) : base(context)
        {

        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(int? cityId = null)
        {
            var query = _context.Employees.Include(e => e.User).AsNoTracking().AsQueryable();
            if (cityId.HasValue)
            {
                query = query.Where(c => c.User.CityId == cityId);
            }

            return await query.ToListAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);
            return employee;

        }


    }
}
