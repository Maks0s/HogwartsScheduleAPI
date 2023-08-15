using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Put
{
    public class StudentPutDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? EnrollmentYear { get; set; }
        public Family? Family { get; set; }
    }
}
