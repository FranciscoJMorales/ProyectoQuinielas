using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuinielasApi.Utils;
using QuinielasModel.DTO;
using QuinielasModel;
using QuinielasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace QuinielasApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PoolsController : ControllerBase
    {
        private readonly ILogger<PoolsController> _logger;
        private readonly QuinielasContext _context;

        public PoolsController(ILogger<PoolsController> logger, QuinielasContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<QuinielaFull> GetPool(int id)
        {
            var poolInfo = await _context.Pools.FindAsync(id);
            var pool = await _context.Pools
                .Include(p => p.Users)
                .Where(p => p.Id == id)
                .Select(p => new QuinielaFull
                {
                    Id = p.Id,
                    Participantes = p.Users.Count,
                    Privada = p.Private,
                    AdminId = p.AdminId,
                    Administrador = p.Admin.Username,
                    Límite = p.UsersLimit,
                    Nombre = p.Name
                }).FirstAsync();
            pool.Users = await _context.Users
                .Where(u => u.PoolsNavigation.Contains(poolInfo!))
                .Select(u => new QuinielasModel.User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    Active = u.Active
                }).ToListAsync();
            pool.UsersScore = await _context.Users
                .Include(u => u.Predictions)
                .Include(u => u.PoolsNavigation)
                .Where(u => u.PoolsNavigation.Contains(poolInfo!))
                .Select(u => new UserScore
                {
                    Usuario = u.Username,
                    Puntuación = (int)u.Predictions.Sum(p => p.Score)!
                }).ToListAsync();
            return pool;
        }

        [Route("other/{userid}")]
        [HttpGet]
        public async Task<IEnumerable<QuinielaView>> GetOtherPools(int userid)
        {
            var user = await _context.Users.FindAsync(userid);
            var pools = await _context.Pools
                .Include(p => p.Users)
                .Where(p => p.Users.Contains(user!) && (bool)p.Active!)
                .Select(p => new QuinielaView
                {
                    Id = p.Id,
                    Participantes = p.Users.Count,
                    Privada = p.Private,
                    Administrador = p.Admin.Username,
                    Límite = p.UsersLimit,
                    Nombre = p.Name
                }).ToListAsync();
            return pools;
        }

        [Route("mine/{userid}")]
        [HttpGet]
        public async Task<IEnumerable<QuinielaView>> GetMyPools(int userid)
        {
            var pools = await _context.Pools
                .Include(p => p.Users)
                .Where(p => p.AdminId == userid && (bool)p.Active!)
                .Select(p => new QuinielaView
                {
                    Id = p.Id,
                    Participantes = p.Users.Count,
                    Privada = p.Private,
                    Administrador = p.Admin.Username,
                    Límite = p.UsersLimit,
                    Nombre = p.Name
                }).ToListAsync();
            return pools;
        }

        [Route("join/{userid}")]
        [HttpGet]
        public async Task<IEnumerable<QuinielaView>> GetNewPools(int userid)
        {
            var user = await _context.Users.FindAsync(userid);
            var pools = await _context.Pools
                .Include(p => p.Users)
                .Where(p => p.Users.Count < p.UsersLimit && !p.Users.Contains(user!) && (bool)p.Active!)
                .Select(p => new QuinielaView
                {
                    Id = p.Id,
                    Participantes = p.Users.Count,
                    Privada = p.Private,
                    Administrador = p.Admin.Username,
                    Límite = p.UsersLimit,
                    Nombre = p.Name
                }).ToListAsync();
            return pools;
        }

        [Route("create")]
        [HttpPost]
        public async Task<Result> Create(QuinielasModel.Pool pool)
        {
            if (!pool.Private)
                pool.Password = null;
            else
            {
                if (string.IsNullOrEmpty(pool.Password))
                {
                    return new Result
                    {
                        HasError = true,
                        Alert = new AlertInfo
                        {
                            Alert = "Error al crear quiniela",
                            AlertIcon = "error",
                            AlertMessage = "Una quiniela privada debe tener contrasena"
                        }
                    };
                }
            }
            var poolModel = Mapper.ToDbModel(pool);
            await _context.Pools.AddAsync(poolModel);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pool {pool.Name} created");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Quiniela creada",
                    AlertIcon = "success",
                    AlertMessage = $"Has creado la quiniela {pool.Name} correctamente",
                    RedirectUrl = $"/quinielas/quiniela/{poolModel.Id}"
                }
            };
        }

        [Route("join")]
        [HttpPost]
        public async Task<Result> Join(UserPool info)
        {
            var user = await _context.Users.FindAsync(info.UserId);
            var pool = await _context.Pools.FindAsync(info.PoolId);
            if (pool!.Private && !pool!.Password!.Equals(info.Password))
            {
                return new Result
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al unirse a la quiniela",
                        AlertIcon = "error",
                        AlertMessage = "La contrasena no es correcta"
                    }
                };
            }
            pool!.Users.Add(user!);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {user!.Username} joined pool {pool.Name}");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "¡Bienvenido!",
                    AlertIcon = "success",
                    AlertMessage = $"Te has unido a la quiniela {pool.Name} correctamente",
                    RedirectUrl = $"/quinielas/quiniela/{pool.Id}"
                }
            };
        }

        [Route("leave")]
        [HttpPost]
        public async Task<Result> Leave(UserPool info)
        {
            var user = await _context.Users
                .Include(u => u.PoolsNavigation)
                .Where(u => u.Id == info.UserId)
                .FirstOrDefaultAsync();
            var pool = await _context.Pools.FindAsync(info.PoolId);
            user!.PoolsNavigation.Remove(pool!);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {user.Username} left pool {pool!.Name}");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Operación exitosa",
                    AlertIcon = "info",
                    AlertMessage = $"Has abandonado la quiniela {pool.Name} correctamente",
                    RedirectUrl = $"/quinielas"
                }
            };
        }
    }
}
