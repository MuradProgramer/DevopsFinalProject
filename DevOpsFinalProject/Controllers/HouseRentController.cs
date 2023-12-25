using DevOpsFinalProject.Models;
using DevOpsFinalProject.Models.DataTransferObjs;
using DevOpsFinalProject.Services;
using DevOpsFinalProject.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsFinalProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HouseRentController : Controller
    {
        private readonly ICacheService _cacheService;

        private readonly IHouseRentDBService _houseRentDBService;

        public HouseRentController(ICacheService cacheService, IHouseRentDBService houseRentDBService)
        {
            _cacheService = cacheService;
            _houseRentDBService = houseRentDBService;
        }

        [HttpGet("GetHousesByPage")]
        public async Task<IActionResult> GetHousesByPage([FromQuery] int pageNumber)
        {
            //Stopwatch stopwatch = new();

            if (await _cacheService.IsKeyExist($"page_{pageNumber}"))
            {
                // await Console.Out.WriteLineAsync("From Cache");
                //stopwatch.Start();
                var cachedHouses = await _cacheService.GetAsync<List<House>>($"page_{pageNumber}");
                //stopwatch.Stop();
                //await Console.Out.WriteLineAsync($"From Cahce: {stopwatch.ElapsedMilliseconds}");
                return Ok(cachedHouses);
            }

            //stopwatch.Start();
            var houses = await _houseRentDBService.GetByPage(pageNumber);
            //stopwatch.Stop();
            //await Console.Out.WriteLineAsync($"From DB: {stopwatch.ElapsedMilliseconds}");

            await _cacheService.SetAsync($"page_{pageNumber}", houses, TimeSpan.FromMinutes(1));
            // await Console.Out.WriteLineAsync("Cached");

            return Ok(houses);
        }


        [HttpPost("CreateHouseRent")]
        public async Task<IActionResult> CreateHouseRent(HouseDto house)
        {
            HouseValidator validationRules = new();
            var result = validationRules.Validate(house);

            if (!result.IsValid)
            {
                List<string> errors = new();
                result.Errors.ForEach(e => errors.Add(e.ErrorMessage));
                return BadRequest(errors);
            }

            House newHouse = new() { HostName = house.HostName, ImageUrl = house.ImageUrl, IsForRent = house.IsForRent };

            await _houseRentDBService.Create(newHouse);

            return Ok("House rent succesfuly added!");
        }

        [HttpPost("CreateHouseRentMany")]
        public async Task<IActionResult> CreateHouseRentMany(List<HouseDto> houses)
        {
            HouseValidator validationRules = new();
            List<House> newHouses = new();

            foreach (var house in houses)
            {
                var result = validationRules.Validate(house);

                if (!result.IsValid)
                {
                    List<string> errors = new();
                    result.Errors.ForEach(e => errors.Add(e.ErrorMessage));
                    return BadRequest(errors);
                }

                newHouses.Add(new House() { HostName = house.HostName, ImageUrl = house.ImageUrl, IsForRent = house.IsForRent });
            }

            await _houseRentDBService.CreateMany(newHouses);

            return Ok("House rents succesfuly added!");
        }

        [HttpPost("DeleteAllHouseRents")]
        public async Task<IActionResult> DeleteAllHouseRents()
        {
            await _houseRentDBService.DeleteAll();
            return Ok("All house rents deleted succesfuly");
        }
    }
}
