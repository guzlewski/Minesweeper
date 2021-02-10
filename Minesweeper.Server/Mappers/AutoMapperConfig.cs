using AutoMapper;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Enums;
using Minesweeper.Server.Data.Entities;
using Minesweeper.Server.Logic;

namespace Minesweeper.Server.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize() =>
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AchievementRow, AchievementDto>();
                cfg.CreateMap<GamemodeDto, Gamemode>();
                cfg.CreateMap<Gamemode, GamemodeDto>();
                cfg.CreateMap<Gamemode, GamemodeRow>();
                cfg.CreateMap<Field, FieldDto>().ForMember(f => f.Value, opt => opt.MapFrom(f => f.State == FieldState.Open ? f.Value : 0));
                cfg.CreateMap<Board, BoardDto>();
                cfg.CreateMap<Game, GameDto>();
            })
            .CreateMapper();
    }
}
