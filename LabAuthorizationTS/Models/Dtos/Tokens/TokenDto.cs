namespace LabAuthorizationTS.Models.Dtos.Tokens
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}