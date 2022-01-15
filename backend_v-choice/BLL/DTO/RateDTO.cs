using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class RateDTO
    {
        public RateDTO() { }

        public int Id { get; set; }
        public int Value { get; set; }
        public string AuthorId { get; set; }
        public string AuthorEmail { get; set; }
        public int FilmId { get; set; }
    }
}
