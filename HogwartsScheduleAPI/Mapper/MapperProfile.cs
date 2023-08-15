using HogwartsScheduleAPI.Models;
using HogwartsScheduleAPI.Models.DTO.Get;
using HogwartsScheduleAPI.Models.DTO.Post;
using HogwartsScheduleAPI.Models.DTO.Put;
using Riok.Mapperly.Abstractions;

namespace HogwartsScheduleAPI.Mapper
{
    [Mapper]
    public partial class MapperProfile : IMapper
    {
        #region Student mapper
        public StudentGetDto MapToStudentDto(Student student)
        {
            var dto = ToStudentDto(student);
            dto.StudyYear = DateTime.Now.Year - student.EnrollmentYear.Year;
            return dto;
        }
        [MapProperty(new[] { nameof(Student.House), nameof(House.Name) }, new[] { nameof(StudentGetDto.HouseName) })]
        private partial StudentGetDto ToStudentDto(Student student);
        public ICollection<StudentGetDto> MapToStudentsDtos(ICollection<Student> students)
        {
            var dto = new List<StudentGetDto>();
            foreach (var s in students)
            {
                dto.Add(MapToStudentDto(s));
            }
            return dto;
        }

        public Student MapToStudent(StudentPostDto studentPostDto)
        {
            var student = ToStudent(studentPostDto);
            student.EnrollmentYear = DateTime.Today;
            return student;
        }
        private partial Student ToStudent(StudentPostDto studentPostDto);
        public partial void MapToStudent(StudentPutDto studentPutDto, Student student);
        #endregion

        #region Professor mapper
        [MapProperty(new[] { nameof(Professor.HeadingHouse), nameof(House.Name) }, new[] { nameof(ProfessorGetDto.HeadingHouseName) })]
        [MapProperty(new[] { nameof(Professor.Course), nameof(Course.Name) }, new[] { nameof(ProfessorGetDto.CourseTaught) })]
        public partial ProfessorGetDto MapToProfessorDto(Professor professor);
        public ICollection<ProfessorGetDto> MapToProfessorsDtos(ICollection<Professor> professors)
        {
            var dto = new List<ProfessorGetDto>();
            foreach (var p in professors)
            {
                dto.Add(MapToProfessorDto(p));
            }
            return dto;
        }

        public partial Professor MapToProfessor(ProfessorPostDto professorPostDto);
        public partial void MapToProfessor(ProfessorPutDto professorPutDto, Professor professor);
        #endregion

        #region Course mapper

        public CourseGetDto MapToCourseDto(Course course)
        {
            var dto = ToCourseDto(course);
            dto.ProfessorFullName = $"{course.Professor?.FirstName} {course.Professor?.LastName}";
            return dto;
        }
        private partial CourseGetDto ToCourseDto(Course course);

        public ICollection<CourseGetDto> MapToCoursesDtos(ICollection<Course> courses)
        {
            var dto = new List<CourseGetDto>();
            foreach (var c in courses)
            {
                dto.Add(MapToCourseDto(c));
            }
            return dto;
        }

        public CourseStudentsGetDto MapToCourseStudentsDTO(Course course)
        {
            var dto = ToCourseStudentsDTO(course);
            dto.ProfessorFullName = $"{course.Professor?.FirstName} {course.Professor?.LastName}";
            return dto;
        }
        private partial CourseStudentsGetDto ToCourseStudentsDTO(Course course);

        public partial Course MapToCourse(CoursePostDto coursePostDto);

        #endregion

        #region House mapper
        public HouseGetDto MapToHouseDto(House house)
        {
            var dto = ToHouseDto(house);
            dto.HouseHeadFullName = $"{house.HouseHead?.FirstName} {house.HouseHead?.LastName}";
            return dto;
        }
        private partial HouseGetDto ToHouseDto(House house);
        public ICollection<HouseGetDto> MapToHousesDtos(ICollection<House> houses)
        {
            var dto = new List<HouseGetDto>();
            foreach (var h in houses)
            {
                dto.Add(MapToHouseDto(h));
            }
            return dto;
        }

        public HouseStudentsGetDto MapToHouseStudentsDto(House house)
        {
            var dto = ToHouseStudentsDto(house);
            dto.HouseHeadFullName = $"{house.HouseHead?.FirstName} {house.HouseHead?.LastName}";
            return dto;
        }
        private partial HouseStudentsGetDto ToHouseStudentsDto(House house);

        public partial House MapToHouse(HousePostDto housePostDto);
        #endregion
    }
}
