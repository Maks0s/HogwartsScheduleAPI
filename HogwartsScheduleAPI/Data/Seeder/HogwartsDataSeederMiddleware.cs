using HogwartsScheduleAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HogwartsScheduleAPI.Data.Seeder
{
    public class HogwartsDataSeederMiddleware
    {
        private readonly RequestDelegate _next;


        public HogwartsDataSeederMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync (HttpContext httpContext, HogwartsDbContext dbContext)
        {
            dbContext.Courses.ExecuteDelete();
            dbContext.Houses.ExecuteDelete();
            dbContext.Professors.ExecuteDelete();
            dbContext.Students.ExecuteDelete();
            SeedData(dbContext);
            await _next.Invoke(httpContext);
        }

        private void SeedData(HogwartsDbContext context)
        {
            // Create Houses
            var gryffindor = new House { Name = "Gryffindor" };
            var slytherin = new House { Name = "Slytherin" };
            var hufflepuff = new House { Name = "Hufflepuff" };
            var ravenclaw = new House { Name = "Ravenclaw" };

            // Create Students
            var harry = new Student { FirstName = "Harry", LastName = "Potter", EnrollmentYear = new DateTime(1991, 9, 1), Family = Family.Half, House = gryffindor };
            var hermione = new Student { FirstName = "Hermione", LastName = "Granger", EnrollmentYear = new DateTime(1991, 9, 1), Family = Family.Muggle, House = gryffindor };
            var draco = new Student { FirstName = "Draco", LastName = "Malfoy", EnrollmentYear = new DateTime(1991, 9, 1), Family = Family.Pure, House = slytherin };
            var luna = new Student { FirstName = "Luna", LastName = "Lovegood", EnrollmentYear = new DateTime(1992, 9, 1), Family = Family.Half, House = ravenclaw };
            var cedric = new Student { FirstName = "Cedric", LastName = "Diggory", EnrollmentYear = new DateTime(1990, 9, 1), Family = Family.Half, House = hufflepuff };
            var pansy = new Student { FirstName = "Pansy", LastName = "Parkinson", EnrollmentYear = new DateTime(1991, 9, 1), Family = Family.Pure, House = slytherin };
            var cho = new Student { FirstName = "Cho", LastName = "Chang", EnrollmentYear = new DateTime(1990, 9, 1), Family = Family.Half, House = ravenclaw };
            var zacharias = new Student { FirstName = "Zacharias", LastName = "Smith", EnrollmentYear = new DateTime(1991, 9, 1), Family = Family.Muggle, House = hufflepuff };

            // Create Professors
            var dumbledore = new Professor { FirstName = "Albus", LastName = "Dumbledore", Patronus = "Phoenix" };
            var snape = new Professor { FirstName = "Severus", LastName = "Snape", Patronus = "Doe" };
            var mcgonagall = new Professor { FirstName = "Minerva", LastName = "McGonagall", Patronus = "Cat" };
            var sprout = new Professor { FirstName = "Pomona", LastName = "Sprout", Patronus = "Badger" };
            var vector = new Professor { FirstName = "Septima", LastName = "Vector", Patronus = "Dog" };

            // Create Courses
            var transfiguration = new Course { Name = "Transfiguration", Professor = mcgonagall };
            var potions = new Course { Name = "Potions", Professor = snape };
            var herbology = new Course { Name = "Herbology", Professor = sprout };
            var arithmancy = new Course { Name = "Arithmancy", Professor = vector };

            // Assign Students to Courses
            harry.Courses = new List<Course> { transfiguration };
            hermione.Courses = new List<Course> { transfiguration };
            draco.Courses = new List<Course> { potions };
            luna.Courses = new List<Course> { arithmancy };
            cedric.Courses = new List<Course> { herbology };
            pansy.Courses = new List<Course> { potions };
            cho.Courses = new List<Course> { arithmancy };
            zacharias.Courses = new List<Course> { herbology };

            // Assign Head of Houses
            gryffindor.HouseHead = mcgonagall;
            slytherin.HouseHead = snape;
            hufflepuff.HouseHead = sprout;
            ravenclaw.HouseHead = vector;

            // Assign Students to Houses
            gryffindor.Students = new List<Student> { harry, hermione };
            slytherin.Students = new List<Student> { draco, pansy };
            hufflepuff.Students = new List<Student> { cedric, zacharias };
            ravenclaw.Students = new List<Student> { luna, cho };

            // Add entities to context and save changes
            context.Houses.AddRange(gryffindor, slytherin, hufflepuff, ravenclaw);
            context.Professors.Add(dumbledore);
            context.Courses.AddRange(transfiguration, potions, herbology, arithmancy);

            context.SaveChanges();
        }
    }
}
