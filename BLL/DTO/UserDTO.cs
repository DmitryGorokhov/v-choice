using DAL.Model;

namespace BLL.DTO
{
    public class UserDTO
    {
        public UserDTO()
        { }

        public UserDTO(User user)
        {
            UserName = user.UserName;
            Email = user.Email;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
    }
}