using HogwartsScheduleAPI.Data;
using HogwartsScheduleAPI.Mapper;
using HogwartsScheduleAPI.Models;
using HogwartsScheduleAPI.Models.DTO.Get;
using HogwartsScheduleAPI.Models.DTO.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly HogwartsDbContext _context;
        private readonly ILogger<CourseController> _logger;
        private readonly IMapper _mapper;


        public CourseController(ILogger<CourseController> logger,
            HogwartsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CourseGetDto>>> GetAllCourses()
        {
            _logger.LogInformation("Getting all COURSES from DB");
            var allCourses = await _context.Courses.Include(c => c.Professor).ToListAsync();

            if (allCourses is null)
            {
                _logger.LogWarning("COURSE TABLE is empty");
                return NotFound();
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToCoursesDtos(allCourses);
            return Ok(dto);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CourseGetDto>> GetCourseById(int id)
        {
            _logger.LogInformation("Getting COURSE from DB");
            var courseById = await _context.Courses.Include(c => c.Professor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (courseById is null)
            {
                _logger.LogWarning("Requested ID not found");
                return NotFound("Incorrect Id");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToCourseDto(courseById);
            return Ok(dto);
        }

        [HttpGet]
        [Route("/students/{id}")]
        public async Task<ActionResult<CourseStudentsGetDto>> GetCourseAndStudentsById(int id)
        {
            _logger.LogInformation("Getting COURSE from DB");
            var courseAndStudents = await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.Students)
                    .ThenInclude(s => s.House)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (courseAndStudents is null)
            {
                _logger.LogWarning("Requested ID not found");
                return NotFound("Incorrect Id");
            }

            _logger.LogInformation("Mapping to DTO");
            var dto = _mapper.MapToCourseStudentsDTO(courseAndStudents);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> PostCourse([FromBody] CoursePostDto postDto)
        {
            if(postDto is null)
            {
                _logger.LogWarning("Passing data is NULL");
                return BadRequest("Passing data is NULL");
            }

            var createdCourse = await _context.Courses
                .Where(c => 
                    c.Name.Equals(postDto.Name))
                .FirstOrDefaultAsync();

            if(createdCourse != null)
            {
                _logger.LogWarning("Course already exists");
                return BadRequest("Course already exists");
            }

            _logger.LogInformation("Map from DTO");
            createdCourse = _mapper.MapToCourse(postDto);
            
            var professorLastName = postDto.ProfessorFullName.Split(' ')[1];

            var courseProfessor = await _context.Professors
                .FirstOrDefaultAsync(p =>
                    p.LastName.Equals(professorLastName));

            if (courseProfessor is null)
            {
                _logger.LogWarning("Incorrect PROFESSOR");
                return BadRequest("Incorrect PROFESSOR");
            }

            createdCourse.Professor = courseProfessor;

            await _context.AddAsync(createdCourse);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course created {@createdCourse}", postDto);

            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, postDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> UpdateCourse(int id, [FromBody][MaxLength(100)] string updatedName)
        {
            var courseToUpdate = await _context.Courses.FindAsync(id);

            if(courseToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            courseToUpdate.Name = updatedName;

            _logger.LogInformation("Updating course");
            _context.Courses.Update(courseToUpdate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("add-professor-to-course/{courseId:int}")]
        public async Task<ActionResult> AddProfessorToCourse(int courseId,
            [FromBody] int profId)
        {
            var courseToUpdate = await _context.Courses.FindAsync(courseId);

            if (courseToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            var professorToAdd = await _context.Professors.FindAsync(profId);

            if (professorToAdd is null)
            {
                _logger.LogWarning("Requested professor not found");
                return BadRequest("Incorrect professor ID");
            }

            courseToUpdate.Professor = professorToAdd;

            _context.Update(courseToUpdate);
            _logger.LogInformation("Professor added");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("remove-professor-from-course/{courseId:int}")]
        public async Task<ActionResult> RemoveProfessorFromCourse(int courseId)
        {
            var courseToUpdate = await _context.Courses
                    .Include(c => c.Professor)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

            if (courseToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            courseToUpdate.Professor = null;

            _context.Update(courseToUpdate);
            _logger.LogInformation("Professor removed");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("add-students-to-course/{courseId:int}")]
        public async Task<ActionResult> AddStudentsToCourse(int courseId,
            [FromBody] int[] studIds)
        {
            var courseToUpdate = await _context.Courses
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

            if (courseToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            foreach(int id in studIds)
            {
                var studentToAdd = await _context.Students.FindAsync(id);
                if(studentToAdd != null)
                    courseToUpdate.Students.Add(studentToAdd);
            }

            _context.Update(courseToUpdate);
            _logger.LogInformation("Students added");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("remove-students-from-course/{courseId:int}")]
        public async Task<ActionResult> RemoveStudentsFromCourse(int courseId,
            [FromBody] int[] studIds)
        {
            var courseToUpdate = await _context.Courses
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == courseId);

            if (courseToUpdate is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            foreach (int id in studIds)
            {
                var studentToRemove = await _context.Students.FindAsync(id);
                if (studentToRemove != null)
                    courseToUpdate.Students?.Remove(studentToRemove);
            }

            _context.Update(courseToUpdate);
            _logger.LogInformation("Students removed");

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var courseToDelete = await _context.Courses.FindAsync(id);

            if (courseToDelete is null)
            {
                _logger.LogWarning("Requested ID not found");
                return BadRequest("Incorrect Id");
            }

            _context.Courses.Remove(courseToDelete);
            _logger.LogInformation("Deleting course");

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
