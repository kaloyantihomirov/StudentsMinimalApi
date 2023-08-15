using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Reflection;

namespace StudentsMinimalApi.Tests
{
    public class StudentsTests
    {
        [Fact]
        public async Task MapPost_Should_Successfully_Create_A_New_Student()
        {
            await using var application = new WebApplicationFactory<Program>();

            using HttpClient? client = application.CreateClient();

            HttpResponseMessage? resultFromPost = await client.PostAsJsonAsync(
                "/student/s1", 
                new Student("Kaloyan", "Kolev", "Maths"));
            HttpResponseMessage? resultFromGet = await client.GetAsync("/student/s1");

            Assert.Equal(HttpStatusCode.Created, resultFromPost.StatusCode);
            Assert.Equal(HttpStatusCode.OK, resultFromGet.StatusCode);

            string? contentAsString = await resultFromGet.Content.ReadAsStringAsync();

            var contentAsStudentObject =
                JsonConvert.DeserializeObject<Student>(contentAsString);

            Assert.Equal(HttpStatusCode.Created, resultFromPost.StatusCode);
            Assert.Equal(HttpStatusCode.OK, resultFromGet.StatusCode);

            Assert.NotNull(contentAsStudentObject);

            Assert.Equal("Kaloyan", contentAsStudentObject.FirstName);
            Assert.Equal("Kolev", contentAsStudentObject.LastName);
            Assert.Equal("Maths", contentAsStudentObject.FavouriteSubject);
        }

        [Fact]
        public async Task MapPost_Should_Throw_An_Exception_When_Trying_To_Create_A_Student_With_Id_That_Already_Exists()
        {
            await using var application = new WebApplicationFactory<Program>();

            var a = application.GetType().GetField("_students");

            
        }

        [Fact]
        public async Task MapGet_Should_Return_ProblemDetailsNotFound_When_Trying_To_Access_A_NonExisting_Student()
        {
            await using var application = new WebApplicationFactory<Program>();

            using HttpClient? client = application.CreateClient();

            HttpResponseMessage? result = await client.GetAsync("/student/s1");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task MapGet_Should_Return_the_Correct_Student_When_They_Exist_With_200OK()
        {
            //Arrange
            await using var application = new WebApplicationFactory<Program>();
            using HttpClient? client = application.CreateClient();

            //Act
            await client.PostAsJsonAsync("/student/s1", new Student("Kaloyan", "Kolev", "Maths"));

            HttpResponseMessage? result = await client.GetAsync("/student/s1");

            string? contentAsString = await result.Content.ReadAsStringAsync();

            var contentAsStudentObject =
                JsonConvert.DeserializeObject<Student>(contentAsString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            Assert.NotNull(contentAsStudentObject);

            Assert.Equal("Kaloyan", contentAsStudentObject.FirstName);
            Assert.Equal("Kolev", contentAsStudentObject.LastName);
            Assert.Equal("Maths", contentAsStudentObject.FavouriteSubject);

            Assert.Equal("application/json; charset=utf-8", result.Content.Headers.GetValues("Content-Type").First());
        }
    }
}