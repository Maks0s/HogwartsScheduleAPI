using HogwartsScheduleAPI.Data;
using HogwartsScheduleAPI.Mapper;
using HogwartsScheduleAPI.Models;
using HogwartsScheduleAPI.Models.DTO.Get;
using HogwartsScheduleAPI.Models.DTO.Put;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly HogwartsDbContext _context;
        private readonly ILogger<ProfessorController> _logger;
        private readonly IMapper _mapper;


        public ProfessorController(ILogger<ProfessorController> logger,
            HogwartsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ProfessorGetDto>>> GetAllProfessors()
        {
            _logger.LogInformation("Getting all PROFESSORS from DB");
            var allProfessors = await _context.Professors
                .Include(p => p.HeadingHouse)
                .Include(p => p.Course)
                .ToListAsync();

            if (allProfessors is null)
            {
                _logger.LogWarning("PROFESSOR TABLE is empty");
                return NotFound();
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToProfessorsDtos(allProfessors);
            return Ok(dto);
        }


        [HttpGet]
        [Route("{lastName:alpha:maxlength(10)}")]
        public async Task<ActionResult<StudentGetDto>> GetProfessorByLastName(string lastName)
        {
            _logger.LogInformation("Getting PROFESSOR from DB");
            var professorByLastName = await _context.Professors
                .Include(p => p.HeadingHouse)
                .Include(p => p.Course)
                .FirstOrDefaultAsync(p => p.LastName.Equals(lastName));

            if (professorByLastName is null)
            {
                _logger.LogWarning("Requested LAST NAME not found");
                return NotFound("Incorrect Last Name");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToProfessorDto(professorByLastName);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> PostProfessor(ProfessorPostDto postDto)
        {
            if (postDto is null)
            {
                _logger.LogWarning("Passing data is NULL");
                return BadRequest("Passing data is NULL");
            }

            var createdProfessor = await _context.Professors
                .Where(p =>
                    p.FirstName.Equals(postDto.FirstName) &&
                    p.LastName.Equals(postDto.LastName))
                .FirstOrDefaultAsync();

            if (createdProfessor != null)
            {
                _logger.LogWarning("Professor already exists");
                return BadRequest("Professor already exists");
            }
               

            var headingHouseName = postDto.HeadingHouseName;
            var courseTaughtName = postDto.CourseTaught;

            _logger.LogInformation("Map from DTO");
            createdProfessor = _mapper.MapToProfessor(postDto);

            if (headingHouseName != null)
            {
                var headingHouse = await _context.Houses.FirstOrDefaultAsync(h => h.Name.Equals(headingHouseName));
                if (headingHouse != null)
                {
                    createdProfessor.HeadingHouse = headingHouse;
                }
                else
                {
                    postDto.HeadingHouseName = null;
                }
            }

            if (courseTaughtName != null)
            {
                var courseTaught = await _context.Courses.FirstOrDefaultAsync(c => c.Name.Equals(courseTaughtName));
                if (courseTaught is null)
                    courseTaught = new Course { Name = courseTaughtName };

                createdProfessor.Course = courseTaught;
            }

            await _context.AddAsync(createdProfessor);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Professor created {@createdProfessor}", postDto);

            return CreatedAtAction(nameof(GetProfessorByLastName), new { lastName = createdProfessor.LastName }, postDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdateProfessor(int id, [FromBody] ProfessorPutDto putDto)
        {
            var professorToUpdate = await _context.Professors.FindAsync(id);

            if (professorToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            _mapper.MapToProfessor(putDto, professorToUpdate);

            _logger.LogInformation("Updating professor");
            _context.Professors.Update(professorToUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("add-course-to-professor/{profId:int}")]
        public async Task<ActionResult> AddCourseToProfessor(int profId, 
            [FromBody][MaxLength(100)] string courseName)
        {
            var professorToUpdate = await _context.Professors.FindAsync(profId);

            if (professorToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            var courseToAdd = await _context.Courses
                .FirstOrDefaultAsync(c => c.Name.Equals(courseName));

            if (courseToAdd is null)
            {
                _logger.LogWarning("Requested course not found");
                return BadRequest("Incorrect course NAME");
            }

            professorToUpdate.Course = courseToAdd;

            _context.Update(professorToUpdate);
            _logger.LogInformation("Course added");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("remove-course-from-professor/{profId:int}")]
        public async Task<ActionResult> RemoveCourseFromProfessor(int profId)
        {
            var professorToUpdate = await _context.Professors
                    .Include(p => p.Course)
                    .FirstOrDefaultAsync(p => p.Id == profId);

            if (professorToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            professorToUpdate.Course = null;

            _context.Update(professorToUpdate);
            _logger.LogInformation("Course removed");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteProfessor(int id)
        {
            var professorToDelete = await _context.Professors.FindAsync(id);

            if (professorToDelete is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            _logger.LogInformation("Deleting professor");
            _context.Professors.Remove(professorToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
