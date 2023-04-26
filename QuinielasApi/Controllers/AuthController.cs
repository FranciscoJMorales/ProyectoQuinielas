using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuinielasApi.Models;
using QuinielasApi.Utils;
using QuinielasModel;
using QuinielasModel.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly QuinielasContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, QuinielasContext context, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Funciona");
        }

        [Route("login")]
        [HttpPost]
        public async Task<UserToken> Login(UserAuth userCreds)
        {
            var user = await _context.Users
                            .Where(u => (u.Username == userCreds.UserEmail || u.Email == userCreds.UserEmail) && (bool)u.Active!)
                            .FirstOrDefaultAsync();
            if (user == null)
            {
                return new UserToken
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Login incorrecto",
                        AlertIcon = "error",
                        AlertMessage = "El usuario no existe"
                    }
                };
            }
            if (Encryption.ComparePasswords(user.Password, userCreds.Password))
            {
                _logger.LogInformation($"{user.Username} logged in succesfully!");
                return new UserToken
                {
                    Id = user.Id,
                    Username = user.Username,
                    Token = CustomTokenJWT(user.Username)
                };
            }
            return new UserToken
            {
                HasError = true,
                Alert = new AlertInfo
                {
                    Alert = "Login incorrecto",
                    AlertIcon = "error",
                    AlertMessage = "La contrasena no coincide"
                }
            };
        }

        [Route("register")]
        [HttpPost]
        public async Task<UserToken> Register(UserRegister userInfo)
        {
            var usernameExists = await _context.Users
                .Where(u => u.Username == userInfo.Username && (bool)u.Active!)
                .FirstOrDefaultAsync();
            if (usernameExists != null)
            {
                return new UserToken
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al registrar",
                        AlertIcon = "error",
                        AlertMessage = "El nombre de usuario ya existe"
                    }
                };
            }
            var emailExists = await _context.Users
                .Where(u => u.Email == userInfo.Email && (bool)u.Active!)
                .FirstOrDefaultAsync();
            if (emailExists != null)
            {
                return new UserToken
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al registrar",
                        AlertIcon = "error",
                        AlertMessage = "El correo ya existe"
                    }
                };
            }
            var user = new Models.User { Username = userInfo.Username, Email = userInfo.Email, Password = Encryption.EncryptPassword(userInfo.Password) };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"{user.Username} registered succesfully!");
            return new UserToken
            {
                Id = user.Id,
                Username = user.Username,
                Token = CustomTokenJWT(user.Username),
                Alert = new AlertInfo
                {
                    Alert = $"¡Bienvenido {user.Username}!",
                    AlertIcon = "success",
                    AlertMessage = "Te has registrado correctamente",
                    RedirectUrl = "/dashboard"
                }
            };
        }

        private string CustomTokenJWT(string username)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!)
            );
            var _signingCredentials = new SigningCredentials(
                _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
            );
            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, username)
            };
            var _Payload = new JwtPayload(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: _Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(2)
            );
            var _Token = new JwtSecurityToken(_Header, _Payload);
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}
