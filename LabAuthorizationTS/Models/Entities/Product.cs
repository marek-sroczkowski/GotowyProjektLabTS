namespace LabAuthorizationTS.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool OnlyForAdults { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}