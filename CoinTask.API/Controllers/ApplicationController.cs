using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CoinTask.API.Data;
using CoinTask.API.Dtos;
using CoinTask.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoinTask.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationController(IConfiguration configuration, IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetCoins()
        {
            string url = _configuration.GetSection("AppSettings:CoinCapAPI").Value;
            var jsonObject = fetchDataFromCoinApi(url);
            return Ok(jsonObject.Data);
        }

        [HttpPost("mark-coin")]
        public async Task<IActionResult> MarkCoin([FromBody] UserCoinDto userCoinDto)
        {
            var user =  _applicationRepository.Find(new User(), userCoinDto.UserId);
            if (user == null) 
            {
                return BadRequest("Can not find user");
            }
            UserCoin userCoin = await _applicationRepository.FindCoinById(user, userCoinDto.CoinId);
            if(userCoin == null) 
            {
                userCoin = new UserCoin {
                    User = user,
                    CoinId = userCoinDto.CoinId
                };

                _applicationRepository.Add(userCoin);
            } else {
                _applicationRepository.Delete(userCoin);
            }

            await _applicationRepository.SaveAll();
            return Ok(userCoin);
        }

        [HttpGet("users/{id}/coins")]
        public async Task<IActionResult> GetUserCoinCaps(Guid id)
        {
            var user = _applicationRepository.Find(new User(), id);
            if (user == null) 
                return BadRequest("Could not find user");
            
            var coins = await _applicationRepository.FindCoinsByUser(user);

            if (coins.Length == 0) 
                return BadRequest("This user has no favorite coins");

            string url = _configuration.GetSection("AppSettings:CoinCapAPI").Value;
            url += "?ids=" + coins;
            var jsonObject = fetchDataFromCoinApi(url);
            return Ok(jsonObject.Data);
        
        }

        private ResponseDto fetchDataFromCoinApi(string url)
        {
           var coins = new List<CoinDto>();
            ResponseDto jsonObject;
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                var json = response.Content.ReadAsStringAsync().Result;
                jsonObject = JsonConvert.DeserializeObject<ResponseDto>(json);

            }
            return jsonObject; 
        }
    }
}