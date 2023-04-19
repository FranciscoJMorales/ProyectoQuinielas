using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuinielasApi.Models;
using QuinielasModel;
using QuinielasApi.Utils;
using Microsoft.EntityFrameworkCore;
using QuinielasModel.DTO.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using QuinielasModel.DTO;
using QuinielasModel.DTO.Pools;
using Microsoft.IdentityModel.Tokens;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly QuinielasContext _context;

        public UsersController(ILogger<UsersController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<QuinielasModel.User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return Mapper.ToModel(user!);
        }

        [Route("update/{id}")]
        [HttpPut]
        public async Task<Result> UpdateUser(int id, UpdateUser user)
        {
            var usernameExists = await _context.Users
                .Where(u => u.Username == user.Username && u.Id != id && (bool)u.Active!)
                .FirstOrDefaultAsync();
            if (usernameExists != null)
            {
                return new Result
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al actualizar",
                        AlertIcon = "error",
                        AlertMessage = "El nombre de usuario ya existe"
                    }
                };
            }
            var emailExists = await _context.Users
                .Where(u => u.Email == user.Email && u.Id != id && (bool)u.Active!)
                .FirstOrDefaultAsync();
            if (emailExists != null)
            {
                return new Result
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al actualizar",
                        AlertIcon = "error",
                        AlertMessage = "El correo ya existe"
                    }
                };
            }
            var currentUser = await _context.Users.FindAsync(id);
            currentUser!.Username = user.Username;
            currentUser!.Email = user.Email;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"{user.Username} updated personal information");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Actualizacion correcta",
                    AlertIcon = "success",
                    AlertMessage = "Se ha actualizado tu informacion correctamente",
                    RedirectUrl = "/users/profile"
                }
            };
        }

        [Route("password/{id}")]
        [HttpPut]
        public async Task<Result> ChangePassword(int id, UpdatePassword password)
        {
            var user = await _context.Users.FindAsync(id);
            if (Encryption.ComparePasswords(user!.Password, password.OldPassword))
            {
                user.Password = Encryption.EncryptPassword(password.NewPassword);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{user.Username} updated password");
                return new Result
                {
                    Alert = new AlertInfo
                    {
                        Alert = "Actualizacion correcta",
                        AlertIcon = "success",
                        AlertMessage = "La contrasena se ha actualizado correctamente",
                        RedirectUrl = "/users/profile"
                    }
                };
            }
            return new Result
            {
                HasError = true,
                Alert = new AlertInfo
                {
                    Alert = "Error al actualizar contrasena",
                    AlertIcon = "error",
                    AlertMessage = "La contrasena actual no es la correcta"
                }
            };
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<Result> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user!.Active = false;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {user.Username} deleted");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Lamentamos que te vayas",
                    AlertIcon = "info",
                    AlertMessage = "Todos tus datos se han eliminado. Esperamos que vuelvas pronto!",
                    RedirectUrl = "/"
                }
            };
        }

        [Route("report/{id}")]
        [HttpGet]
        public async Task<Report?> GetUserReport(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return null;
            var report = await _context.Users
                .Include(u => u.Pools)
                .Include(u => u.PoolsNavigation)
                .Include(u => u.Predictions)
                .Where(u => u.Id == id)
                .Select(u => new Report
                {
                    UserId = u.Id,
                    OwnedPools = u.Pools.Where(p => (bool)p.Active!).Count(),
                    ParticipantPools = u.PoolsNavigation.Where(p => (bool)p.Active!).Count(),
                    PredictionsSent = u.Predictions.Where(p => (bool)p.Active!).Count(),
                    TotalScore = (int)u.Predictions.Where(p => (bool)p.Active!).Sum(pr => pr.Score)!
                }).FirstAsync();
            report.Scores = await _context.Pools
                .Include(p => p.Users)
                .Where(p => p.Users.Contains(user))
                .Select(p => new PoolScore
                {
                    Pool = p.Name,
                    Score = (int)p.Users.Where(u => u.Id == id).First().Predictions.Where(pr => p.Games.Contains(pr.Game)).Sum(pr => pr.Score)!
                }).ToListAsync();
            return report;
        }
    }
}
