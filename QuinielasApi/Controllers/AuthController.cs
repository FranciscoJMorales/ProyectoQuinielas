using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuinielasApi.Models;
using QuinielasApi.Utils;
using QuinielasModel;
using QuinielasModel.DTO;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly QuinielasContext _context;

        public AuthController(ILogger<AuthController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        public async Task<UserId> Login(UserAuth userCreds)
        {
            var user = await _context.Users
                            .Where(u => (u.Username == userCreds.UserEmail || u.Email == userCreds.UserEmail) && (bool)u.Active!)
                            .FirstOrDefaultAsync();
            if (user == null)
            {
                return new UserId
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
                return new UserId
                {
                    Id = user.Id,
                    Username = user.Username
                };
            }
            return new UserId
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
        public async Task<UserId> Register(UserRegister userInfo)
        {
            var usernameExists = await _context.Users
                .Where(u => u.Username == userInfo.Username && (bool)u.Active!)
                .FirstOrDefaultAsync();
            if (usernameExists != null)
            {
                return new UserId
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
                return new UserId
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
            return new UserId
            {
                Id = user.Id,
                Username = user.Username,
                Alert = new AlertInfo
                {
                    Alert = $"¡Bienvenido {user.Username}!",
                    AlertIcon = "success",
                    AlertMessage = "Te has registrado correctamente",
                    RedirectUrl = "/dashboard"
                }
            };
        }
    }
}
