using Application.Constants;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class UserRepo : GenericRepo<User>, IUserRepository
    {
        public UserRepo(Context context) : base(context)
        {

        }

        public async Task<IEnumerable<User>> GetUsers(int page, int size, int? cityId = null)
        {
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            IQueryable<User> query =  _context.Users.Include(u => u.Role).AsNoTracking();
            if (cityId.HasValue)
                query = query.Where(u => u.CityId == cityId);
            query = query.Where(u => u.RoleId == AppRoles.User);
            var users =
                await query.OrderBy(u => u.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return users;
        }
        

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Set<User>().Include(u => u.Role).Include(u => u.EmployeeProfile).AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

    }
}
