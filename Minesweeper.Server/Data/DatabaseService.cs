using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LiteDB;
using Microsoft.Extensions.Options;
using Minesweeper.Common.DTO;
using Minesweeper.Server.Data.Entities;
using Minesweeper.Server.Logic;

namespace Minesweeper.Server.Data
{
    public class DatabaseService
    {
        private readonly LiteDatabase _liteDatabase;
        private readonly LiteDBSettings _settings;
        private readonly IMapper _mapper;

        public DatabaseService(IOptions<LiteDBSettings> options, IMapper mapper)
        {
            _settings = options.Value;
            _liteDatabase = new LiteDatabase(_settings.Path);
            _mapper = mapper;

            var gamemodes = _liteDatabase.GetCollection<GamemodeRow>(_settings.GamemodesTable);
            gamemodes.EnsureIndex(g => g.Width);
            gamemodes.EnsureIndex(g => g.Height);
            gamemodes.EnsureIndex(g => g.Bombs);
        }

        public void Add(Gamemode gamemode, string player, TimeSpan time, DateTimeOffset date)
        {
            var achievements = _liteDatabase.GetCollection<AchievementRow>(_settings.AchievementsTable);
            var gamemodes = _liteDatabase.GetCollection<GamemodeRow>(_settings.GamemodesTable);

            var gamemodeId = gamemodes.Query()
                .Where(gm =>
                    gamemode.Name == gm.Name &&
                    gamemode.Width == gm.Width &&
                    gamemode.Height == gm.Height &&
                    gamemode.Bombs == gm.Bombs)
                .Select(gm => gm.GamemodeId)
                .FirstOrDefault();

            if (gamemodeId == 0)
            {
                gamemodeId = gamemodes.Insert(_mapper.Map<GamemodeRow>(gamemode));
            }

            var achivmentEntity = new AchievementRow
            {
                Date = date,
                GamemodeId = gamemodeId,
                Player = player,
                Time = time
            };

            achievements.Insert(achivmentEntity);
        }

        public List<RankingDto> GetRanking(IEnumerable<Gamemode> dtos, int max)
        {
            var achievements = _liteDatabase.GetCollection<AchievementRow>(_settings.AchievementsTable);
            var gamemodes = _liteDatabase.GetCollection<GamemodeRow>(_settings.GamemodesTable);
            var pages = new List<RankingDto>();

            foreach (var dto in dtos)
            {
                var gamemodeId = gamemodes.Query()
                    .Where(gamemode => gamemode.Bombs == dto.Bombs &&
                        gamemode.Height == dto.Height &&
                        gamemode.Name == dto.Name &&
                        gamemode.Width == dto.Width)
                    .Select(gamemode => gamemode.GamemodeId)
                    .FirstOrDefault();

                if (gamemodeId == 0)
                {
                    continue;
                }

                var achievementsList = achievements.Query()
                     .Where(achievement => achievement.GamemodeId == gamemodeId)
                     .ToList();

                var page = new RankingDto
                {
                    Achievements = _mapper.Map<IEnumerable<AchievementDto>>(achievementsList.Take(max)),
                    Gamemode = _mapper.Map<GamemodeDto>(dto),
                    Total = achievementsList.Count
                };

                pages.Add(page);
            }

            return pages;
        }
    }
}
