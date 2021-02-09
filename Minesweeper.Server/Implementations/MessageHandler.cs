using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Requests;
using Minesweeper.Server.Interfaces;
using Minesweeper.Server.Logic;

namespace Minesweeper.Server.Implementations
{
    public class MessageHandler : IMessageHandler
    {
        private readonly Random _random;
        private readonly IMapper _mapper;
        private readonly List<Gamemode> _gamemodes;

        public Game Game { get; set; }

        public MessageHandler(Random random, IMapper mapper, IOptionsSnapshot<List<Gamemode>> gamemodes)
        {
            _random = random;
            _mapper = mapper;
            _gamemodes = gamemodes.Value;
        }

        public object GetResponse(Request request)
        {
            return request switch
            {
                CreateGame createGame => CreateGame(createGame),
                GetRanking getRanking => GetRanking(getRanking),
                Handshake handshake => Handshake(handshake),
                PlayGame playGame => PlayGame(playGame),
                _ => throw new ArgumentException()
            };
        }

        private GameDto CreateGame(CreateGame request)
        {
            var gamemode = _mapper.Map<Gamemode>(request.Gamemode);
            Game = new Game(_random, gamemode);

            return _mapper.Map<GameDto>(Game);
        }

        private object GetRanking(GetRanking request)
        {
            throw new NotImplementedException();
        }

        private object Handshake(Handshake request)
        {
            return _mapper.Map<IEnumerable<GamemodeDto>>(_gamemodes);
        }

        private GameDto PlayGame(PlayGame request)
        {
            Game.Play(request.Row, request.Column, request.Action);
            return _mapper.Map<GameDto>(Game);
        }
    }
}
