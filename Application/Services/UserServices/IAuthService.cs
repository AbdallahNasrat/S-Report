using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.UserServices
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(string email , string password);

    }
}
