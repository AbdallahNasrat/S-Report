using Application.DTOs.LookupDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GeneralServices
{
    public interface ILookupsService
    {
        public Task<IEnumerable<LookupDto>> GetCitiesAsync();
    }
}
