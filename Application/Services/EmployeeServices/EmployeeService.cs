using Application.Constants;
using Application.DTOs.EmployeeDTOs;
using Application.DTOs.UserDTOs;
using Domain.Entites;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _uow;

        public EmployeeService(IUnitOfWork uow )
        {
            _uow = uow;
        }
       

      
    }
}
