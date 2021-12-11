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
    public class CommentControllerTests
    {
        private readonly Mock<ICrudService> crudServiceStub = new();
        private readonly Mock<IPaginationService> paginationServiceStub = new();
        private readonly Mock<ILogger<CommentController>> loggerStub = new();

        [Fact]
        public async Task CommentController_GetCommentsPagination_ReturnsBadRequest_IfModelStateIsInvalid()
        {
            PaginationQuery paginationQuery = null;
            var controller = new CommentController(
                crudServiceStub.Object,
                paginationServiceStub.Object,
                loggerStub.Object);

            Assert.IsType<BadRequestObjectResult>(await controller.GetCommentsPagination(paginationQuery));
        }

        [Fact]
        public async Task CommentController_GetCommentsPagination_ReturnsCode500()
        {
            PaginationQuery paginationQuery = new PaginationQuery()
            {
                OnPageCount = 0,
                PageNumber = 100
            };

            paginationServiceStub.Setup(cs => cs.GetCommentsPagination(It.IsAny<PaginationQuery>()))
                .ReturnsAsync((PaginationDTO<CommentDTO>)null);

            var controller = new CommentController(
                crudServiceStub.Object,
                paginationServiceStub.Object,
                loggerStub.Object);

            Assert.IsType<StatusCodeResult>(await controller.GetCommentsPagination(paginationQuery));
        }

        [Fact]
        public async Task CommentController_GetCommentsPagination_ReturnsExpected()
        {
            PaginationQuery paginationQuery = new PaginationQuery()
            {
                OnPageCount = 2,
                PageNumber = 2
            };

            PaginationDTO<CommentDTO> testAnswer = new PaginationDTO<CommentDTO>()
            {
                OnPageCount = 2,
                Page = 2,
                Items = null,
                TotalCount = 10
            };

            paginationServiceStub.Setup(cs => cs.GetCommentsPagination(It.IsAny<PaginationQuery>()))
                .ReturnsAsync(testAnswer);

            var controller = new CommentController(
                crudServiceStub.Object,
                paginationServiceStub.Object,
                loggerStub.Object);

            var result = await controller.GetCommentsPagination(paginationQuery);

            Assert.IsType<OkObjectResult>(result);
            (result as OkObjectResult).Value.Should().BeEquivalentTo(testAnswer);
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
