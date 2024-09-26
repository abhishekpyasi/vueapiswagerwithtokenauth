using Microsoft.AspNetCore.Identity;

namespace Vueapi.Model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }

        public string Email { get; set; }

        public DateTime? CreatedOn { get; set; }
    }




}

