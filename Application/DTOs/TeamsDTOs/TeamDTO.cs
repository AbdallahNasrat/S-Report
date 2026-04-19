using Domain.Enums;


namespace Application.DTOs.TeamsDTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public TeamState State { get; set; }

    }
}
