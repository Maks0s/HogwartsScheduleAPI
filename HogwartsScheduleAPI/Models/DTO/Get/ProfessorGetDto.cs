using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class ProfessorGetDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HeadingHouseName { get; set; }
        public string CourseTaught { get; set; }
    }
}
