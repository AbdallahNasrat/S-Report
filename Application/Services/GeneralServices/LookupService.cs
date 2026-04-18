using Application.DTOs.LookupDtos;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.GeneralServices
{
    public class LookupService : ILookupsService
    {
        private readonly IUnitOfWork _uow;
        public LookupService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<LookupDto>> GetCitiesAsync()
        {
            var cities = await _uow.CitiesRepo.GetAllAsync();
            if (cities == null || !cities.Any())
                throw new Exception("Cities cannot be loaded");

            var result = cities.Select(c => new LookupDto()
            {
                Id = c.Id,
                Name = c.Name
            });
            return result;
        }
             
    }
}

