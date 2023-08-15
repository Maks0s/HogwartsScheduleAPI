using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class ProfessorPostDto
    {
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Patronus { get; set; }

        [MaxLength(10)]
        public string? HeadingHouseName { get; set; }

        [MaxLength(100)]
        public string? CourseTaught { get; set; }
    }
}
