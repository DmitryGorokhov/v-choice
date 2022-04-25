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
        Task<FilmDTO> CreateFilmAsync(FilmDTO film, IWebHostEnvironment _appEnvironment);
        Task<string> UpdateFilmAsync(int id, FilmDTO film, IWebHostEnvironment _appEnvironment);
        Task DeleteFilmAsync(int id);
        Task<CommentDTO> CreateCommentAsync(CommentDTO comment, ClaimsPrincipal user);
        Task UpdateCommentAsync(int id, CommentDTO comment);
        Task DeleteCommentAsync(int id);
        IEnumerable<GenreDTO> GetAllGenres();
        Task<RateDTO> CreateRateAsync(RateDTO rate, ClaimsPrincipal user);
        Task UpdateRateAsync(int id, RateDTO rate);
        Task DeleteRateAsync(int id);
        Task<RateDTO> GetFilmRate(int filmId, ClaimsPrincipal user);
        Task<GenreDTO> CreateGenreAsync(GenreDTO genre);
        Task UpdateGenreAsync(int id, GenreDTO genre);
        Task DeleteGenreAsync(int id);
    }
}
