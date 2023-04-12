using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuinielasApi.Utils;
using QuinielasModel.DTO;
using QuinielasModel;
using QuinielasApi.Models;
using Microsoft.EntityFrameworkCore;
using QuinielasModel.DTO.Pools;
using Org.BouncyCastle.Asn1.X509;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [Route("{id}/{userid}")]
        [HttpGet]
        public async Task<QuinielaFull?> GetPool(int id, int userid)
        {
            var poolInfo = await _context.Pools.Where(p => p.Id == id && (bool)p.Active!).FirstOrDefaultAsync();
            if (poolInfo != null)
            {
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
                    .Where(u => u.PoolsNavigation.Contains(poolInfo))
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
                    .Where(u => u.PoolsNavigation.Contains(poolInfo))
                    .Select(u => new UserScore
                    {
                        Usuario = u.Username,
                        Puntuación = (int)u.Predictions.Sum(p => p.Score)!
                    }).OrderByDescending(s => s.Puntuación)
                    .ToListAsync();
                pool.Partidos = await _context.Games
                    .Where(g => g.PoolId == poolInfo.Id)
                    .Select(g => new GamePrediction
                    {
                        Id = g.Id,
                        GameDate = g.GameDate,
                        Team1 = g.Team1,
                        Team2 = g.Team2,
                        Team1Score = g.Team1Score,
                        Team2Score = g.Team2Score
                    }).OrderBy(g => g.GameDate)
                    .ToListAsync();

                //Assign positions
                if (pool.UsersScore.Count > 0)
                {
                    pool.UsersScore[0].Posición = 1;
                    //Allow ties
                    for (int i = 1; i < pool.UsersScore.Count; i++)
                    {
                        if (pool.UsersScore[i].Puntuación < pool.UsersScore[i - 1].Puntuación)
                            pool.UsersScore[i].Posición = i + 1;
                        else
                            pool.UsersScore[i].Posición = pool.UsersScore[i - 1].Posición;
                    }
                }

                //Get scores, predictions and availability
                foreach (var item in pool.Partidos)
                {
                    //Check if game is available
                    var lastDate = new DateTime(item.GameDate.Year, item.GameDate.Month, item.GameDate.Day);
                    if (item.Team1Score == null && DateTime.Now < lastDate)
                        item.Available = true;

                    //Set user prediction and score
                    var prediction = await _context.Predictions
                    .Where(p => p.GameId == item.Id && p.UserId == userid)
                        .FirstOrDefaultAsync();
                    if (prediction != null)
                    {
                        item.Team1Prediction = prediction.Team1Score;
                        item.Team2Prediction = prediction.Team2Score;
                        item.Score = prediction.Score;
                    }
                }

                //Check if user is allowed to see pool
                if (pool.AdminId == userid)
                    pool.IsAdmin = true;
                if (pool.Users!.Find(u => u.Id == userid) != null)
                    pool.IsParticipant = true;

                return pool;
            }
            return null;
        }

        [Route("id/{id}")]
        [HttpGet]
        public async Task<PoolId?> GetPoolId(int id)
        {
            var pool = await _context.Pools.Where(p => p.Id == id && (bool)p.Active!).FirstOrDefaultAsync();
            if (pool == null)
                return null;
            return Mapper.ToModel(pool);
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

        [Route("{id}/isAdmin/{userid}")]
        [HttpGet]
        public async Task<bool?> IsAdmin(int id, int userid)
        {
            var pool = await _context.Pools.FindAsync(id);
            if (pool == null)
                return null;
            return pool.AdminId == userid;
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
            var poolExists = await _context.Pools.Where(p => p.Name == pool.Name && p.AdminId == pool.AdminId && (bool)p.Active!).FirstOrDefaultAsync();
            if (poolExists != null)
            {
                return new Result
                {
                    HasError = true,
                    Alert = new AlertInfo
                    {
                        Alert = "Error al crear quiniela",
                        AlertIcon = "error",
                        AlertMessage = "Ya creaste una quiniela con el mismo nombre"
                    }
                };
            }
            var poolModel = Mapper.ToDbModel(pool);
            await _context.Pools.AddAsync(poolModel);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pool {pool.Name} created");
            if (pool.Join)
            {
                var result = await Join(new UserPool { PoolId = poolModel.Id, UserId = pool.AdminId, Password = pool.Password });
                if (result.HasError)
                {
                    return new Result
                    {
                        Alert = new AlertInfo
                        {
                            Alert = "Quiniela creada",
                            AlertIcon = "info",
                            AlertMessage = "Has creado la quiniela {pool.Name} correctamente, pero ocurrió un error al unirte. Prueba unirte más tarde",
                            RedirectUrl = $"/quinielas/quiniela/{poolModel.Id}"
                        }
                    };
                }
            }
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
                    RedirectUrl = "/quinielas"
                }
            };
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<Result> DeletePool(int id)
        {
            var pool = await _context.Pools.FindAsync(id);
            pool!.Active = false;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Pool {pool.Name} deleted");
            return new Result
            {
                Alert = new AlertInfo
                {
                    Alert = "Operación exitosa",
                    AlertIcon = "info",
                    AlertMessage = $"Se ha eliminado la quiniela {pool.Name} correctamente",
                    RedirectUrl = "/quinielas/mine"
                }
            };
        }
    }
}
