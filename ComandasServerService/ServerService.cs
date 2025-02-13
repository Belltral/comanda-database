using ComandasServerService.ServerComponents;
using System;
using System.Net.Sockets;
using System.ServiceProcess;

namespace ComandasServerService
{
    public partial class ServerService : ServiceBase
    {
        private TcpListener _listener;
        private bool running;

        public ServerService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            await ServerListener.ServerConnection();
        }

        protected override void OnStop()
        {
            ServerListener.StopServer();
        }
    }
}
