using System.ComponentModel.DataAnnotations;

namespace HogwartsScheduleAPI.Models.DTO.Put
{
    public class ProfessorPutDto
    {
        [MaxLength(25)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(25)]
        public string? Patronus { get; set; }

    }
}
