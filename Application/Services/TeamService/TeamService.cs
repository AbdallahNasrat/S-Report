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
            var existTeam = await _uow.TeamRepo.GetFirstOrDefaultAsync(t => t.Name == dto.Name);
            if (existTeam!=null)
            {
                throw new Exception("TeamName is used");
            }

            var newTeam = new Team()
            {
                Name = dto.Name,
                CityId = dto.CityId,
                State = Domain.Enums.TeamState.Available
            };
            _uow.TeamRepo.Add(newTeam);
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<bool> BlockTeam(int id)
        {
            var team = await _uow.TeamRepo.GetByIdAsync(id);
            if (team == null) { throw new Exception("team does not exist"); }
            team.State = TeamState.Blocked;
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<TeamDTO> GetTeamByIdAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public async Task<TeamDTO> GetTeamByName(string teamName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Team>> GetTeams()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTeam(TeamDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
