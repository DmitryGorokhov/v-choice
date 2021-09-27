using System.Collections.Generic;

namespace BLL.DTO
{
    public class PaginationDTO<T>
    {
        public PaginationDTO() { }

        public PaginationDTO(PaginationQuery query)
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
