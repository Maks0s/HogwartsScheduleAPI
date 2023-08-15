using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int? ProfessorId { get; set; }
        public Professor? Professor { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
