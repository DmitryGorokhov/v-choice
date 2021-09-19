using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class PaginationQuery
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        public int OnPageCount { get; set; }
        public int? GenreId { get; set; }
    }
}
