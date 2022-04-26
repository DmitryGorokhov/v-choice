using BLL.Query;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class PaginationDTO<T>
    {
        public PaginationDTO() { }

        public PaginationDTO(PaginationQueryBase query)
        {
            Page = query.PageNumber;
            OnPageCount = query.OnPageCount;
        }

        public int Page { get; set; }

        public int OnPageCount { get; set; }

        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
