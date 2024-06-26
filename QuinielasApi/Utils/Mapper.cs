﻿using QuinielasApi.Models;
using QuinielasModel.DTO.Games;
using QuinielasModel.DTO.Pools;
using QuinielasModel.DTO.Predictions;

namespace QuinielasApi.Utils;

public static class Mapper
{
    public static QuinielasModel.User ToModel(User user)
    {
        return new QuinielasModel.User
        {
            Id = user.Id,
            Active = user.Active,
            Email = user.Email,
            Password = user.Password,
            Username = user.Username
        };
    }

    public static PoolId ToModel(Pool pool)
    {
        return new PoolId
        {
            Id = pool.Id,
            AdminId = pool.AdminId,
            Name = pool.Name
        };
    }

    public static Pool ToDbModel(QuinielasModel.Pool pool)
    {
        return new Pool
        {
            Id = pool.Id,
            Active = pool.Active,
            AdminId = pool.AdminId,
            Name = pool.Name,
            Password = pool.Password,
            Private = pool.Private,
            UsersLimit = pool.UsersLimit
        };
    }

    public static Game ToDbModel(NewGame game)
    {
        return new Game
        {
            Id = game.Id,
            GameDate = game.GameDate,
            Deadline = game.Deadline,
            PoolId = game.PoolId,
            Team1 = game.Team1,
            Team2 = game.Team2
        };
    }

    public static Prediction ToDbModel(NewPrediction prediction)
    {
        return new Prediction
        {
            GameId = prediction.GameId,
            UserId = prediction.UserId,
            Team1Score = prediction.Team1Score,
            Team2Score = prediction.Team2Score
        };
    }
}
