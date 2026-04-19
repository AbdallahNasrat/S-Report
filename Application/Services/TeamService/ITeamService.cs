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
        public Task<TeamDTO> GetTeamByIdAsync(int teamId);
        public Task<TeamDTO> GetTeamByName(string teamName);
        public Task<IEnumerable<Team>> GetTeams();
        public Task<bool> BlockTeam(int id);
        public Task<bool> UpdateTeam(TeamDTO dto);
    }
}
