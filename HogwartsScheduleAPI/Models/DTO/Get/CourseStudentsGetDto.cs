namespace HogwartsScheduleAPI.Models.DTO.Get
{
    public class CourseStudentsGetDto
    {
        public string Name { get; set; }
        public string ProfessorFullName { get; set; }
        public ICollection<StudentGetDto> Students { get; set; }
    }
}
