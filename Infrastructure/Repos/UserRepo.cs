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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
