using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinTask.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoinTask.API.Data
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly DataContext _context;
        
        public ApplicationRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public T Find<T>(T entity, Guid id) where T : class
        {
            T obj = (T)_context.Find(entity.GetType(), id);
            return obj;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public async Task<UserCoin> FindCoinById(User user, string coinId) {
            var userCoin = await _context.UserCoins.FirstOrDefaultAsync(x => x.User == user && x.CoinId == coinId);
            return userCoin;
        }

        public async Task<string> FindCoinsByUser(User user)
        {
            List<string> coins = await _context.UserCoins.Where(x => x.User == user).Select(y => y.CoinId).ToListAsync();       
            return string.Join(",", coins);
        }

        public async Task<User> FindUserByRefreshToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            return user;
        }
    }
}