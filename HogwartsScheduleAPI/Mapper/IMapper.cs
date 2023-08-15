using HogwartsScheduleAPI.Models;
using HogwartsScheduleAPI.Models.DTO.Get;
using HogwartsScheduleAPI.Models.DTO.Post;
using HogwartsScheduleAPI.Models.DTO.Put;

namespace HogwartsScheduleAPI.Mapper
{
    public interface IMapper
    {
        StudentGetDto MapToStudentDto(Student student);
        ICollection<StudentGetDto> MapToStudentsDtos(ICollection<Student> students);
        Student MapToStudent(StudentPostDto studentPostDto);
        void MapToStudent(StudentPutDto studentPutDto, Student student);


        CourseGetDto MapToCourseDto(Course course);
        CourseStudentsGetDto MapToCourseStudentsDTO(Course course);
        ICollection<CourseGetDto> MapToCoursesDtos(ICollection<Course> courses);
        Course MapToCourse(CoursePostDto coursePostDto);


        ProfessorGetDto MapToProfessorDto(Professor professor);
        ICollection<ProfessorGetDto> MapToProfessorsDtos(ICollection<Professor> professors);
        Professor MapToProfessor(ProfessorPostDto professorPostDto);
        void MapToProfessor(ProfessorPutDto professorPostDto, Professor professor);


        HouseGetDto MapToHouseDto(House house);
        HouseStudentsGetDto MapToHouseStudentsDto(House house);
        ICollection<HouseGetDto> MapToHousesDtos(ICollection<House> houses);
        House MapToHouse(HousePostDto housePostDto);

    }
}
