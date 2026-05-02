using Application.DTOs;
using Application.DTOs.VolunteerDTOs;


public interface IVolunteerService
{
    
    Task<List<NearbyMissionResponseDto>> GetNearbyMissions(int cityId, decimal userLat, decimal userLng);
    Task<bool> AcceptMission(int reportId, int volunteerId);
    Task<bool> CompleteMission(int reportId, int volunteerId);
    Task<List<LeaderboardDto>> GetTopVolunteers();
    Task<VolunteerProfileDto> GetVolunteerProfile(int volunteerId);
    Task CheckAndAssignAchievements(int volunteerId);
}