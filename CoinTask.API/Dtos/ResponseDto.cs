using System.Collections.Generic;

namespace CoinTask.API.Dtos
{
    public class ResponseDto
    {
        public IEnumerable<CoinDto> Data { get; set; }
    }
}