using Vueapi.Model;

namespace Vueapi.Dtos.UserDto

{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}
