using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class HouseGetDto
    {
        public string Name { get; set; }
        public string HouseHeadFullName { get; set; }
    }
}
