using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Post
{
    public class StudentPostDto
    {
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EnumDataType(typeof(Family))]
        public Family Family { get; set; }
    }
}
