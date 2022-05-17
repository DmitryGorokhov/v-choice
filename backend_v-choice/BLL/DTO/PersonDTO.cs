using Microsoft.AspNetCore.Http;

namespace BLL.DTO
{
    public class PersonDTO
    { 
        public int? Id { get; set; }
        public string FullName { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }
    }
}
