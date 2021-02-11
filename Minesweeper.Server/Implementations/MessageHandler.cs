using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Options;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Requests;
using Minesweeper.Server.Data;
using Minesweeper.Server.Interfaces;
using Minesweeper.Server.Logic;

namespace Minesweeper.Server.Implementations
{
    public class MessageHandler : IMessageHandler
    {
        private readonly Random _random;
        private readonly IMapper _mapper;
        private readonly List<Gamemode> _gamemodes;
        private readonly DatabaseService _database;
        private string _nickname;

        public SaveableGame Game { get; set; }

        public MessageHandler(Random random, IMapper mapper, IOptionsSnapshot<List<Gamemode>> gamemodes, DatabaseService database)
        {
            _random = random;
            _mapper = mapper;
            _gamemodes = gamemodes.Value;
            _database = database;
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
            if (_nickname == null)
            {
                throw new InvalidOperationException();
            }

            if (request.Gamemode.Bombs < GamemodeDto.MinBombs || request.Gamemode.Bombs > GamemodeDto.MaxBombs)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Gamemode.Bombs));
            }

            if (request.Gamemode.Height < GamemodeDto.MinHeight || request.Gamemode.Height > GamemodeDto.MaxHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Gamemode.Height));
            }

            if (request.Gamemode.Width < GamemodeDto.MinWidth || request.Gamemode.Width > GamemodeDto.MaxWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(request.Gamemode.Width));
            }

            var gamemode = _mapper.Map<Gamemode>(request.Gamemode);
            Game = new SaveableGame(_gamemodes, _random, gamemode, _database, _nickname);

            return _mapper.Map<GameDto>(Game);
        }

        private object GetRanking(GetRanking _)
        {
            return _database.GetRanking(_gamemodes, 10);
        }

        private object Handshake(Handshake request)
        {
            _nickname = request.Nickname;
            return _mapper.Map<IEnumerable<GamemodeDto>>(_gamemodes);
        }

        private GameDto PlayGame(PlayGame request)
        {
            if (Game == null)
            {
                throw new InvalidOperationException();
            }

            Game.Play(request.Row, request.Column, request.Action);
            return _mapper.Map<GameDto>(Game);
        }
    }
}
