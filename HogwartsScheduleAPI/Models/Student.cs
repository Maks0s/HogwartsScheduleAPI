using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime EnrollmentYear { get; set; }
        [Required]
        [EnumDataType(typeof(Family))]
        public Family Family { get; set; }
        public int? HouseId { get; set; }
        public House? House { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }

    public enum Family
    {
        Pure,
        Half,
        Muggle
    }
}
