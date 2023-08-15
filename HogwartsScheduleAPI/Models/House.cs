using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models
{
    public class House
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        public int? HouseHeadId { get; set; }
        public Professor? HouseHead { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
