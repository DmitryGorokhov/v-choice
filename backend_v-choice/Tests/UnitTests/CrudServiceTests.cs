using BLL.DTO;
using BLL.Interface;
using BLL.Service;
using BLL.Utils;
using DAL.Interface;
using DAL.Model;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class CrudServiceTests
    {
        private readonly Mock<IFilmRepository> filmRepositoryStub = new();
        private readonly Mock<IGenreRepository> genreRepositoryStub = new();
        private readonly Mock<ICommentsRepository> commentsRepositoryStub = new();
        private readonly Mock<ILogger<CrudService>> loggerStub = new();
        private readonly Mock<ILogger<Mapper>> mapperLoggerStub = new();
        private readonly Mock<IMapper> mapperStub = new();

        [Fact]
        public async Task CrudService_CreateFilmAsync_ReturnsNull_IfNotCreated()
        {
            FilmDTO testFilm = new();

            filmRepositoryStub.Setup(fr => fr.CreateFilmAsync(It.IsAny<Film>()))
                .ReturnsAsync((Film)null);

            var service = new CrudService(
                filmRepositoryStub.Object,
                genreRepositoryStub.Object,
                commentsRepositoryStub.Object,
                loggerStub.Object,
                new Mapper(mapperLoggerStub.Object));

            Assert.Null(await service.CreateFilmAsync(testFilm));
        }

        [Fact]
        public async Task CrudService_CreateFilmAsync_ReturnsFilmDTO()
        {
            FilmDTO testFilm = new();

            filmRepositoryStub.Setup(fr => fr.CreateFilmAsync(It.IsAny<Film>()))
                .ReturnsAsync(new Film());
            
            mapperStub.Setup(m => m.FilmDTOtoModel(It.IsAny<FilmDTO>()))
                .Returns(new Film());
            mapperStub.Setup(m => m.FilmModelToDTO(It.IsAny<Film>()))
                .Returns(new FilmDTO());

            var service = new CrudService(
                filmRepositoryStub.Object,
                genreRepositoryStub.Object,
                commentsRepositoryStub.Object,
                loggerStub.Object,
                mapperStub.Object);

            var result = await service.CreateFilmAsync(testFilm);
            
            Assert.IsType<FilmDTO>(result);
            Assert.NotNull(result);
        }
    }
}
