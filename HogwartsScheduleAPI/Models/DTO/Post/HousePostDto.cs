using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Post
{
    public class HousePostDto
    {
        [Required]
        [MaxLength(80)]
        public string HouseHeadFullName { get; set; }
    }
}
