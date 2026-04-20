using Application.DTOs.TeamsDTOs;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.TeamService
{
    public interface ITeamService
    {
        public Task<bool> AddTeamAsync(CreateTeamDTO dto);
        public Task<TeamDTO?> GetTeamByIdAsync(int teamId);
        public Task<TeamDTO?> GetTeamByNameAsync(string teamName);
        public Task<IEnumerable<TeamDTO>?> GetTeamsAsync();
        public Task<IEnumerable<TeamDTO>?> GetTeamsByCityIdAsync(int cityId);
        public Task<bool> BlockTeamAsync(int id);
        public Task<bool> UpdateTeamAsync(TeamDTO dto);
        public Task<bool> UpdateTeamStateAsync(UpdateTeamStateDTO dto);
        public Task<bool> AssignEmployeeToTeamAsync(AssignEmployeeToTeamDTO dto);
    }
}
