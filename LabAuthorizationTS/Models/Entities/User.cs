using LabAuthorizationTS.Models.Enums;

namespace LabAuthorizationTS.Models.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime BirthDate { get; set; }
        public UserRole Role { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}