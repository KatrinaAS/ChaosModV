using Fleck;
using Newtonsoft.Json;
using Serilog;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TwitchChatVotingProxy.ChaosPipe;

namespace TwitchChatVotingProxy.VotingReceiver
{
    class WebsiteVotingReceiver : IVotingReceiver
    {

        private readonly ChaosPipeClient m_ChaosPipe;
        private readonly ILogger m_Logger = Log.Logger.ForContext<WebsiteVotingReceiver>();
        private int m_Port;
        private int m_WebsocketPort;
        private readonly Dictionary<IWebSocketConnection, string> connections = new();
        private List<IVoteOption> options;
        private bool percentage;

        public event EventHandler<OnMessageArgs>? OnMessage = null;

        public WebsiteVotingReceiver(OptionsFile config, ChaosPipeClient chaosPipe)
        {
            m_ChaosPipe = chaosPipe;
            m_Port = config.ReadValueInt("WebsiteVotingPort", 8081);
            m_WebsocketPort = config.ReadValueInt("WebsiteWebsocketPort", 8082);
        }


        public async Task<bool> Init()
        {
            try
            {
                var WSS = new WebSocketServer($"ws://0.0.0.0:{m_WebsocketPort}");
                // Set the websocket listeners
                WSS.Start(connection =>
                {
                    connection.OnOpen += () => OnWsConnectionOpen(connection);
                    connection.OnClose += () => OnWSConnectionClose(connection);
                    connection.OnMessage += (message) => OnWSMessage(message, connection);
                });
            }
            catch (Exception e)
            {
                m_Logger.Fatal(e, "Failed so start websocket server");
                return false;
            }
            m_Logger.Information("Initialized Website voting");
            return true;
        }



        public async Task SendMessage(string message)
        {
            m_Logger?.Debug("message: "+message);
            foreach (var kvp in connections)
            {
                var connection = kvp.Key;
                await connection.Send(message);

            }
        }
        private void OnWSMessage(string message, IWebSocketConnection connection)
        {
            string[] command = message.Split('~');
            m_Logger.Information(" incoming message: " + message);
            switch (command[0].ToLower())
            {
                case "username":
                    if (command.Length > 1)
                        connections[connection] = message.Substring(message.IndexOf(" ") + 1);
                    else
                    {
                        m_Logger.Error("Error setting Username");
                    }
                    break;
                case "vote":
                    if (command.Length > 1)
                    {
                        m_Logger.Information("Voting for " + command[1]);
                        OnMessage?.Invoke(this, new OnMessageArgs()
                        {
                            Username = connections[connection],
                            Message = command[1],
                            ClientId = connection.ConnectionInfo.Id.ToString()
                        });
                    }
                    break;
            }

        }

        private async Task OnWsConnectionOpen(IWebSocketConnection connection)
        {
            try
            {
                m_Logger.Information($"New websocket client {connection.ConnectionInfo.ClientIpAddress}");
                connections.Add(connection, "");
                await SendJSON();
            }
            catch (Exception e)
            {
                m_Logger.Error(e, "Error occurred as client connected");
            }
        }

        private void OnWSConnectionClose(IWebSocketConnection connection)
        {
            try
            {
                m_Logger.Information($"Websocket client disconnected {connection.ConnectionInfo.ClientIpAddress}");
                connections.Remove(connection);
            }
            catch (Exception e)
            {
                m_Logger.Error(e, "Error occurred as client disconnected");
            }
        }

        public bool IsDataBased() => true;
        public async Task<bool> SendData(List<IVoteOption> options, bool percentage)
        {
            this.options = options;
            this.percentage = percentage;
            await SendJSON();
            return true;
        }

        public async Task EndVoting() {
            var data = new { type = "endVotes"};
            string jsonData = JsonConvert.SerializeObject(data);
            foreach (var kvp in connections)
            {
                var connection = kvp.Key;
                await connection.Send(jsonData);
            }
        }

        public async Task NoVotingRound() {
            var data = new { type = "noVotes" };
            string jsonData = JsonConvert.SerializeObject(data);
            foreach (var kvp in connections)
            {
                var connection = kvp.Key;
                await connection.Send(jsonData);
            }
        }
       
        public async Task UpdateVoting(List<IVoteOption> votes) {
            this.options = votes;
            await SendJSON(false);
        }

        private async Task SendJSON(bool newVotes=true)
        {
            var data = new { type= newVotes?"newVotes": "updateVotes" ,percentage = percentage, options = options };
            string jsonData = JsonConvert.SerializeObject(data);
            foreach (var kvp in connections)
            {
                var connection = kvp.Key;
                await connection.Send(jsonData);
            }
        }
    }
}