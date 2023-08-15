using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models
{
    public class Professor
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(25)]
        public string Patronus { get; set; }
        public House? HeadingHouse { get; set; }
        public Course? Course { get; set; }
    }
}
