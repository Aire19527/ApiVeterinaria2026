namespace Veterinaria.Entity.DTO.Tokens
{
    public class TokenDto
    {
        public string Token { get; set; } = null!;
        public double Expiration { get; set; }
        public DateTime DateExpiration { get; set; }
    }
}
