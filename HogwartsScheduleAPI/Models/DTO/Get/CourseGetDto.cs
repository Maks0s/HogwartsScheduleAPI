using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class CourseGetDto
    {
        public string Name { get; set; }
        public string ProfessorFullName { get; set; }
    }
}
