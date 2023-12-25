using DevOpsFinalProject.Models;

namespace DevOpsFinalProject.Services;

public interface IHouseRentDBService
{
    Task<List<House>> GetByPage(int pageNumber);

    Task Create(House house);

    Task CreateMany(List<House> houses);

    Task DeleteAll();
}
