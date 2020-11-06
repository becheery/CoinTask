using System.Threading.Tasks;
using CoinTask.API.Data;
using CoinTask.API.Models;
using Microsoft.AspNetCore.Mvc;
using CoinTask.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.AspNetCore.Authorization;

namespace CoinTask.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ITokenHelperRepository _tokenHelperRepository;
        public AuthController(IAuthRepository repo, IConfiguration config, 
            IApplicationRepository applicationRepository, ITokenHelperRepository tokenHelperRepository)
        {
            _config = config;
            _repo = repo;
            _applicationRepository = applicationRepository;
            _tokenHelperRepository = tokenHelperRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Email))
                return BadRequest("Email already exists");

            var userToCreate = new User
            {
                Firstname = userForRegisterDto.Firstname,
                Lastname = userForRegisterDto.Lastname,
                Email = userForRegisterDto.Email
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Email.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var token = _tokenHelperRepository.GenerateAccessToken(userFromRepo);

            if (userFromRepo.RefreshToken == null || !_tokenHelperRepository.ValidateRefreshToken(userFromRepo.RefreshTokenExpireTime))
            {
                userFromRepo.RefreshToken = _tokenHelperRepository.GenerateRefreshToken();
                userFromRepo.RefreshTokenExpireTime = DateTime.Now.AddMinutes(10);
            }

            _applicationRepository.Update(userFromRepo);
            await _applicationRepository.SaveAll();

            return Ok(new
            {
                token = token,
                refreshToken = userFromRepo.RefreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var user = await _applicationRepository.FindUserByRefreshToken(refreshTokenDto.RefreshToken);
            if (user == null)
                return BadRequest("No user matches to that token");
            
            if (!_tokenHelperRepository.ValidateRefreshToken(user.RefreshTokenExpireTime))
                return BadRequest("Your refresh token has expired");
            
            user.RefreshToken = _tokenHelperRepository.GenerateRefreshToken();
            user.RefreshTokenExpireTime = DateTime.Now.AddMinutes(10);

            _applicationRepository.Update(user);
            await _applicationRepository.SaveAll();

            var token = _tokenHelperRepository.GenerateAccessToken(user);

            return Ok(new {
                token = token,
                refreshToken = user.RefreshToken
            });
        }
    }
}