using System.ComponentModel.DataAnnotations;

namespace BLL.Query
{
    public class PaginationQueryBase
    {
        [Required]
        public int PageNumber { get; set; }
        
        [Required]
        public int OnPageCount { get; set; }
    }
}
