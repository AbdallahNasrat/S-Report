using Application.DTOs.TeamsDTOs;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Services.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _uow;
        public TeamService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<bool> AddTeamAsync(CreateTeamDTO dto)
        {
            var existTeam = await _uow.TeamRepo.GetFirstOrDefaultAsync(t => t.Name == dto.Name,false);
            if (existTeam!=null)
            {
                return false;
            }

            var newTeam = new Team()
            {
                Name = dto.Name,
                CityId = dto.CityId,
                State = TeamState.Available
            };
            _uow.TeamRepo.Add(newTeam);
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<bool> BlockTeamAsync(int id)
        {
            var team = await _uow.TeamRepo.GetByIdAsync(id);
            if (team == null) { throw new Exception("team does not exist"); }
            team.State = TeamState.Blocked;
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<TeamDTO?> GetTeamByIdAsync(int teamId)
        {
            var team = await _uow.TeamRepo.GetByIdAsync(teamId,false);
            if (team == null) { return null; }
            var dto = new TeamDTO() {
                Id = team.Id,
                Name = team.Name,
                CityId = team.CityId,
                State = team.State,
            };
            return dto;
        }

        public async Task<TeamDTO?> GetTeamByNameAsync(string teamName)
        {
            var team = await _uow.TeamRepo.GetFirstOrDefaultAsync(t => t.Name == teamName, false);
            if (team == null) return null;
            var dto = new TeamDTO()
            {
                Id = team.Id,
                Name = team.Name,
                CityId = team.CityId,
                State = team.State,
            };
            return dto;
        }

        public async Task<IEnumerable<TeamDTO>?> GetTeamsByCityIdAsync(int cityId)
        {
            var teams = await _uow.TeamRepo.GetAllAsync(t => t.CityId == cityId);
            if(teams == null) return [];
            var result = teams.Select(t => new TeamDTO()
            {
                Id = t.Id,
                Name = t.Name,
                CityId = t.CityId,
                State = t.State,
            });
            return result;
        }
        public async Task<IEnumerable<TeamDTO>?> GetTeamsAsync()
        {
            var teams = await _uow.TeamRepo.GetAllAsync();
            if(teams == null) return [];
            var result = teams.Select(t => new TeamDTO()
            {
                Id = t.Id,
                Name = t.Name,
                CityId = t.CityId,
                State = t.State,
            });
            return result;
        }

        public async Task<bool> UpdateTeamAsync(TeamDTO dto)
        {
            var team = await _uow.TeamRepo.GetByIdAsync(dto.Id);
            if (team == null) return false;
            team.Name = dto.Name;
            team.CityId = dto.CityId;
            return await _uow.SaveChangesAsync()>0;
            
        }
        public async Task<bool> UpdateTeamStateAsync(UpdateTeamStateDTO dto)
        {
            var team = await _uow.TeamRepo.GetByIdAsync(dto.TeamId);
            if (team == null) return false;
            team.State = dto.State;
            return await _uow.SaveChangesAsync()>0;
            
        }
        public async Task<bool> AssignEmployeeToTeamAsync(AssignEmployeeToTeamDTO dto)
        {
            
            var employee = await _uow.EmployeesRepo.GetByIdAsync(dto.EmployeeId);
            if (employee == null) throw new Exception("The employee is not present.");

   
            var teamExists = await _uow.TeamRepo.GetFirstOrDefaultAsync(t => t.Id == dto.TeamId, tracked: false);
            if (teamExists == null) throw new Exception("The team is not present.");
            
            employee.TeamId = dto.TeamId;
            
            return await _uow.SaveChangesAsync() > 0;
        }

    }
}
