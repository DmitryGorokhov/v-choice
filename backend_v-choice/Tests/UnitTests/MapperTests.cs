using BLL.DTO;
using BLL.Utils;
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
    public class MapperTests
    {
        private readonly Mock<ILogger<Mapper>> loggerStub = new();

        private FilmDTO CreateFilmDTO()
        {
            return new FilmDTO()
            {
                Id = 100,
                Title = "Test film title",
                Year = 2021,
                Description = "Film for test",
                Genres = new HashSet<GenreDTO>()
                {
                    new GenreDTO() { Id = 10001, Value = "Genre 1" },
                    new GenreDTO() { Id = 10002, Value = "Genre 2" },
                    new GenreDTO() { Id = 10003, Value = "Genre 3" }
                },
            };
        }

        [Fact]
        public void Mapper_FilmDTOtoModel_ReturnsExpected()
        {
            FilmDTO testFilmDTO = CreateFilmDTO();
            
            Film film = new Mapper(loggerStub.Object).FilmDTOtoModel(testFilmDTO);
            
            Assert.NotNull(film);
            Assert.Equal(testFilmDTO.Id, film.Id);
            Assert.Equal(testFilmDTO.Description, film.Description);
            Assert.Equal(testFilmDTO.Title, film.Title);
            Assert.Equal(testFilmDTO.Year, film.Year);
            Assert.Equal(testFilmDTO.Genres.Count, film.Genres.Count);
        }
    }
}
