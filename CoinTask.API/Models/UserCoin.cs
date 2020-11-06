namespace CoinTask.API.Models
{
    public class UserCoin
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string CoinId { get; set; }
    }
}