using System;

namespace BLL.DTO
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public string AuthorId { get; set; }
        public int FilmId { get; set; }
    }
}
