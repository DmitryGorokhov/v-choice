using System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using backend_v_choice.Controllers;
using BLL.Interface;
using BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace UnitTests
{
    public class FilmContorllerTests
    {
        private readonly Mock<ICrudService> crudServiceStub = new();
        private readonly Mock<IPaginationService> paginationServiceStub = new();
        private readonly Mock<ILogger<FilmController>> loggerStub = new();

        [Fact]
        public async Task FilmContorller_GetFilm_ReturnsBadRequest_IfIdIsInvalid()
        {
            int testId = -1;
            var controller = new FilmController(
                crudServiceStub.Object,
                paginationServiceStub.Object,
                loggerStub.Object);

            Assert.IsType<BadRequestObjectResult>(await controller.GetFilm(testId));
        }

        [Fact]
        public async Task FilmContorller_GetFilm_ReturnsNotFoundAsync()
        {
            int testId = 0;
            
            crudServiceStub.Setup(cs => cs.GetFilmAsync(It.IsAny<int>()))
                .ReturnsAsync((FilmDTO)null);

            var controller = new FilmController(
                crudServiceStub.Object, 
                paginationServiceStub.Object,
                loggerStub.Object);

            Assert.IsType<NotFoundResult>(await controller.GetFilm(testId));
        }

        [Fact]
        public async Task FilmContorller_GetFilm_ReturnsExpectedAsync()
        {
            int testId = 1;
            FilmDTO testFilmDTO = CreateFilmDTOTestInstance(testId);
            
            crudServiceStub.Setup(cs => cs.GetFilmAsync(It.IsAny<int>()))
                .ReturnsAsync(testFilmDTO);

            var controller = new FilmController(
                crudServiceStub.Object,
                paginationServiceStub.Object,
                loggerStub.Object);

            var result = await controller.GetFilm(testId);

            Assert.IsType<OkObjectResult>(result);
            (result as OkObjectResult).Value.Should().BeEquivalentTo(testFilmDTO);
        }

        private FilmDTO CreateFilmDTOTestInstance(int testId)
        {
            return new FilmDTO()
            {
                Id = testId,
                Title = "Test title",
                Year = 2021,
                Description = "Some test description"
            };
        }
    }
}
