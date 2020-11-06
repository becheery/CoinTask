using System;
using System.Collections.Generic;

namespace CoinTask.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public List<UserCoin> Coins { get; set; }
    }
}