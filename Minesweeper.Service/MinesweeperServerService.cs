﻿using System.ServiceProcess;
using Minesweeper.Server.Interfaces;

namespace Minesweeper.Service
{
    partial class MinesweeperServerService : ServiceBase
    {
        private readonly IServer _server;

        public MinesweeperServerService(IServer server)
        {
            _server = server;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _server.Start();
        }

        protected override void OnStop()
        {
            _server.Stop();
        }
    }
}
