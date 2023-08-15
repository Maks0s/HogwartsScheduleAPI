using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class StudentGetDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudyYear { get; set; }
        public string HouseName { get; set; }
    }
}
