using HogwartsScheduleAPI.Data;
using HogwartsScheduleAPI.Mapper;
using HogwartsScheduleAPI.Models;
using HogwartsScheduleAPI.Models.DTO.Get;
using HogwartsScheduleAPI.Models.DTO.Post;
using HogwartsScheduleAPI.Models.DTO.Put;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HogwartsScheduleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly HogwartsDbContext _context;
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;


        public StudentController(ILogger<StudentController> logger,
            HogwartsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<StudentGetDto>>> GetAllStudents()
        {
            _logger.LogInformation("Getting all STUDENTS from DB");
            var allStudents = await _context.Students
                .Include(s => s.House).ToListAsync();

            if (allStudents is null)
            {
                _logger.LogWarning("STUDENT TABLE is empty");
                return NotFound();
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToStudentsDtos(allStudents);
            return Ok(dto);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<StudentGetDto>> GetStudentById(int id)
        {
            _logger.LogInformation("Getting STUDENT from DB");
            var studentById = await _context.Students
                .Include(s => s.House)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentById is null)
            {
                _logger.LogWarning("Requested ID not found");
                return NotFound("Incorrect Id");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToStudentDto(studentById);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> PostStudent(StudentPostDto postDto)
        {
            if (postDto is null)
            {
                _logger.LogWarning("Passing data is NULL");
                return BadRequest("Passing data is NULL");
            }

            var createdStudent = await _context.Students
                .Where(s =>
                    s.FirstName.Equals(postDto.FirstName) &&
                    s.LastName.Equals(postDto.LastName))
                .FirstOrDefaultAsync();

            if(createdStudent != null)
            {
                _logger.LogWarning("Student already exists");
                return BadRequest("Student already exists");
            }

            _logger.LogInformation("Map from DTO");
            createdStudent = _mapper.MapToStudent(postDto);

            _logger.LogInformation("Choosing the house");
            var houses = await _context.Houses.ToArrayAsync();

            var HatChoice = new Random().Next(4);

            createdStudent.House = houses[HatChoice];

            await _context.AddAsync(createdStudent);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Students created {@createdStudent}", postDto);

            return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, postDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdateStudents(int id, [FromBody] StudentPutDto putDto)
        {
            var studentToUpdate = await _context.Students.FindAsync(id);

            if (studentToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            _mapper.MapToStudent(putDto, studentToUpdate);

            _logger.LogInformation("Updating student");
            _context.Students.Update(studentToUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var studentToDelete = await _context.Students.FindAsync(id);

            if (studentToDelete is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            _logger.LogInformation("Deleting student");
            _context.Students.Remove(studentToDelete);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
