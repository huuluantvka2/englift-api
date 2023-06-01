using EngLift.DTO.Base;
using EngLift.DTO.Course;
using EngLift.DTO.Response;
using EngLift.Service.Interfaces;
using EngLift.WebAPI.Controllers.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace EngLift.UnitTest.ControllerTest
{
    public class CourseControllerTests : Controller
    {
        private readonly Mock<ICourseService> _courseServiceMock;
        private readonly Mock<ILogger<CourseController>> _loggerServiceMock;
        private readonly CourseController _controller;

        public CourseControllerTests()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _loggerServiceMock = new Mock<ILogger<CourseController>>();
            _controller = new CourseController(_loggerServiceMock.Object, _courseServiceMock.Object);
        }

        [Fact]
        public async Task GetAllCourse_Returns_Success()
        {
            // Arrange
            var request = new BaseRequest();
            var expectDataList = new DataList<CourseItemDTO>
            {
                Items = new List<CourseItemDTO> {
            new CourseItemDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Course 1",
                Desciption = "Description of Course 1",
                Prior = 1,
                Image = "course1.jpg",
                TotalLesson = 10,
                Active = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "John Doe",
                UpdatedAt = DateTime.Now,
                UpdatedBy = "Jane Smith"
            },
            new CourseItemDTO()
            {
                Id = Guid.NewGuid(),
                Name = "Course 2",
                Desciption = "Description of Course 2",
                Prior = 2,
                Image = "course2.jpg",
                TotalLesson = 8,
                Active = false,
                CreatedAt = DateTime.Now.AddDays(-7),
                CreatedBy = "Alice Johnson",
                UpdatedAt = DateTime.Now.AddDays(-3),
                UpdatedBy = "Bob Smith"
            }},
                TotalRecord = 2,
            };
            _courseServiceMock.Setup(s => s.GetAllCourse(request)).ReturnsAsync(expectDataList);
            // Act
            var result = await _controller.GetAllCourse(request);
            var data = result as JsonResult;
            var serializeJSON = JsonConvert.SerializeObject(data.Value);
            var deserializedData = JsonConvert.DeserializeObject<ResponseData<DataList<CourseItemDTO>>>(serializeJSON);
            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(deserializedData.Data.TotalRecord, 3);
        }
    }
}
