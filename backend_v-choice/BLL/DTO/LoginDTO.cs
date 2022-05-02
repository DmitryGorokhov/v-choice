using System.Collections.Generic;

namespace BLL.DTO
{
    public class LoginDTO
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Error { get; set; }
        public AuthenticatedUserDTO User { get; set; }
    }
}
