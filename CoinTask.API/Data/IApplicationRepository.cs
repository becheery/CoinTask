using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoinTask.API.Models;

namespace CoinTask.API.Data
{
    public interface IApplicationRepository
    {
        void Add<T>(T entity) where T: class;
        T Find<T>(T entity, Guid id) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<UserCoin> FindCoinById(User user, string coinId);
        Task<string> FindCoinsByUser(User user);
        Task<bool> SaveAll();
        Task<User> FindUserByRefreshToken(string refreshToken);
    }
}