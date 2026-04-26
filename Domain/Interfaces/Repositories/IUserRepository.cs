using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> GetUserByEmailAsync(string email);
        public Task<IEnumerable<User>> GetUsers(int page, int size, int? cityId = null);
    }
}
