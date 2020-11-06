using System.ComponentModel.DataAnnotations;

namespace CoinTask.API.Dtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}