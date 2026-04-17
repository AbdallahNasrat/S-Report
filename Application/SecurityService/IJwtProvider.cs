using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SecurityService
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
