

using Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasData(
                new City { Id = 1, Name = "Cairo" },
    new City { Id = 2, Name = "Giza" },
    new City { Id = 3, Name = "Alexandria" },
    new City { Id = 4, Name = "Dakahlia" },
    new City { Id = 5, Name = "Red Sea" },
    new City { Id = 6, Name = "Beheira" },
    new City { Id = 7, Name = "Fayoum" },
    new City { Id = 8, Name = "Gharbia" },
    new City { Id = 9, Name = "Ismailia" },
    new City { Id = 10, Name = "Monufia" },
    new City { Id = 11, Name = "Minya" },
    new City { Id = 12, Name = "Qalyubia" },
    new City { Id = 13, Name = "New Valley" },
    new City { Id = 14, Name = "Sharqia" },
    new City { Id = 15, Name = "Suez" },
    new City { Id = 16, Name = "Aswan" },
    new City { Id = 17, Name = "Assiut" },
    new City { Id = 18, Name = "Beni Suef" },
    new City { Id = 19, Name = "Port Said" },
    new City { Id = 20, Name = "Damietta" },
    new City { Id = 21, Name = "Sharkia" },
    new City { Id = 22, Name = "South Sinai" },
    new City { Id = 23, Name = "Kafr El Sheikh" },
    new City { Id = 24, Name = "Matrouh" },
    new City { Id = 25, Name = "Luxor" },
    new City { Id = 26, Name = "Qena" },
    new City { Id = 27, Name = "North Sinai" },
    new City { Id = 28, Name = "Sohag" }
                );
        } 
    }
}
