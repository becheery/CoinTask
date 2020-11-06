using System;
using CoinTask.API.Models;

namespace CoinTask.API.Data
{
    public interface ITokenHelperRepository
    {
         string GenerateRefreshToken();
         string GenerateAccessToken(User user);
         bool ValidateRefreshToken(DateTime refreshTokenExpireTime);
    }
}