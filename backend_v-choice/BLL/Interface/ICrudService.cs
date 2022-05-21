using BLL.DTO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICrudService
    {
        Task<FilmDTO> GetFilmAsync(int id);
        Task<FilmDTO> CreateFilmAsync(FilmDTO film, IWebHostEnvironment appEnvironment);
        Task<string> UpdateFilmAsync(int id, FilmDTO film, IWebHostEnvironment appEnvironment);
        Task DeleteFilmAsync(int id, IWebHostEnvironment appEnvironment);
        Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, CommentDTO comment);
        Task DeleteCommentAsync(int id);
        IEnumerable<GenreDTO> GetAllGenres();
        Task<RateDTO> CreateRateAsync(RateDTO rate, ClaimsPrincipal user);
        IEnumerable<StudioDTO> GetAllStudios();
        Task UpdateRateAsync(int id, RateDTO rate);
        Task DeleteRateAsync(int id);
        Task<RateDTO> GetFilmRate(int filmId, ClaimsPrincipal user);
        Task<GenreDTO> CreateGenreAsync(GenreDTO genre);
        Task UpdateGenreAsync(int id, GenreDTO genre);
        Task DeleteGenreAsync(int id);
        Task<PersonDTO> CreatePersonAsync(PersonDTO person, IWebHostEnvironment appEnvironment);
        Task<string> UpdatePersonAsync(int id, PersonDTO person, IWebHostEnvironment appEnvironment);
        Task<StudioDTO> CreateStudioAsync(StudioDTO studio);
        Task DeletePersonAsync(int id, IWebHostEnvironment appEnvironment);
        Task UpdateStudioAsync(int id, StudioDTO studio);
        Task DeleteStudioAsync(int id);
        IEnumerable<PersonDTO> GetAllPersons();
    }
}
