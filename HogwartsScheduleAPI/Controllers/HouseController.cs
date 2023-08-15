using HogwartsScheduleAPI.Data;
using HogwartsScheduleAPI.Mapper;
using HogwartsScheduleAPI.Models.DTO.Get;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsScheduleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly HogwartsDbContext _context;
        private readonly ILogger<HouseController> _logger;
        private readonly IMapper _mapper;


        public HouseController(ILogger<HouseController> logger,
            HogwartsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<HouseGetDto>>> GetAllHouses()
        {
            _logger.LogInformation("Getting all HAUSES from DB");
            var allHouses = await _context.Houses.Include(c => c.HouseHead).ToListAsync();

            if (allHouses is null)
            {
                _logger.LogWarning("HAUSE TABLE is empty");
                return NotFound();
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToHousesDtos(allHouses);
            return Ok(dto);
        }

        [HttpGet]
        [Route("/students/{houseName:alpha:maxlength(10)}")]
        public async Task<ActionResult<HouseStudentsGetDto>> GetHouseAndStudents(string houseName)
        {
            _logger.LogInformation("Getting HOUSES from DB");
            var houseAndStudents = await _context.Houses
                .Include(h => h.HouseHead)
                .Include(h => h.Students)
                .FirstOrDefaultAsync(h => h.Name.Equals(houseName));

            if (houseAndStudents is null)
            {
                _logger.LogWarning("Requested NAME not found");
                return NotFound("Incorrect Name");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToHouseStudentsDto(houseAndStudents);

            return Ok(dto);
        }

        [HttpGet]
        [Route("{houseName:alpha:maxlength(10)}")]
        public async Task<ActionResult<HouseGetDto>> GetHouseByName(string houseName)
        {
            _logger.LogInformation("Getting HOUSES from DB");
            var houseByName = await _context.Houses.Include(h => h.HouseHead)
                .FirstOrDefaultAsync(h => h.Name.Equals(houseName));

            if (houseByName is null)
            {
                _logger.LogWarning("Requested NAME not found");
                return NotFound("Incorrect Name");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToHouseDto(houseByName);

            return Ok(dto);
        }

        [HttpPut]
        [Route("add-professor-to-house/{houseName:alpha:maxlength(10)}")]
        public async Task<ActionResult> AddProfessorToHouse(string houseName,
            [FromBody] int profId)
        {
            var houseToUpdate = await _context.Houses
                .FirstOrDefaultAsync(h => h.Name.Equals(houseName));

            if (houseToUpdate is null)
            {
                _logger.LogWarning("Requested NAME not found");
                return NotFound("Incorrect Name");
            }

            var professorToAdd = await _context.Professors.FindAsync(profId);

            if (professorToAdd is null)
            {
                _logger.LogWarning("Requested professor not found");
                return BadRequest("Incorrect professor ID");
            }

            houseToUpdate.HouseHead = professorToAdd;

            _context.Update(houseToUpdate);
            _logger.LogInformation("House head added");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("remove-professor-from-house/{houseName:alpha:maxlength(10)}")]
        public async Task<ActionResult> RemoveProfessorFromHouse(string houseName)
        {
            var houseToUpdate = await _context.Houses
                .Include(h => h.HouseHead)
                .FirstOrDefaultAsync(h => h.Name.Equals(houseName));

            if (houseToUpdate is null)
            {
                _logger.LogWarning("Requested NAME not found");
                return NotFound("Incorrect Name");
            }

            houseToUpdate.HouseHead = null;

            _context.Update(houseToUpdate);
            _logger.LogInformation("House head removed");

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
