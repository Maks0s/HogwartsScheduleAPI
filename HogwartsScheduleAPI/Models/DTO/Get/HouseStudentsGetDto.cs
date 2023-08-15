namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class HouseStudentsGetDto
    {
        public string Name { get; set; }
        public string HouseHeadFullName { get; set; }
        public ICollection<StudentGetDto> Students { get; set; }
    }
}
