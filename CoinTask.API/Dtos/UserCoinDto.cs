using System;
using System.ComponentModel.DataAnnotations;

namespace CoinTask.API.Dtos
{
    public class UserCoinDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string CoinId { get; set; }
    }
}