﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Lidgren.Network;
using IGORRProtocol;
using IGORRProtocol.Messages;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace IGORR_Server
{
    public class Server
    {
        NetServer connection;
        Thread receiveThread;
        Dictionary<long, int> _clientids;
        Dictionary<int, Client> _clients;
        List<NetConnection> _connections;
        DateTime _startTime;
        DateTime _lastTime;
        int lastSent=0;
        int lastRecv=0;
        bool _enableSend;
        int currentChannel = 0;
        //ObjectManager manager;

        public Server()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("IGORR");
            config.Port = 5445;
            //config.SimulatedMinimumLatency = 2f;
            //config.SimulatedRandomLatency = 1f;
            //config.LocalAddress = new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 });
            connection = new NetServer(config);
            connection.Start();
            Protocol.SetUp(connection);
            Protocol.RegisterMessageHandler(MessageTypes.Chat, new MessageHandler(HandleChat));
            Protocol.RegisterMessageHandler(MessageTypes.Position, new MessageHandler(HandlePosition));
            Protocol.RegisterMessageHandler(MessageTypes.Leave, new MessageHandler(HandleLeave));
            Protocol.RegisterMessageHandler(MessageTypes.Join, new MessageHandler(HandleJoin));
            Protocol.RegisterMessageHandler(MessageTypes.Attack, new MessageHandler(HandleAttack));
            _clientids = new Dictionary<long, int>();
            _clients = new Dictionary<int, Client>();
            _connections = new List<NetConnection>();
            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start();
            Logic.EventObject.server = this;
            Logic.LogicHandler.SetUp(this);
            _startTime = DateTime.Now;
            _lastTime = DateTime.Now;
        }

        public void Exit()
        {
            receiveThread.Abort();
        }

        public void Enable()
        {
            _enableSend = true;
        }

        public void Disable()
        {
            _enableSend = false;
        }

        void ReceiveMessage()
        {
            while (true)
            {
                connection.MessageReceivedEvent.WaitOne();
                NetIncomingMessage msg;
                while ((msg = connection.ReadMessage()) != null)
                {
                    int id = -1;
                    if (msg.SenderConnection != null)
                        if (!_clientids.TryGetValue(msg.SenderConnection.RemoteUniqueIdentifier, out id))
                            id = -1;
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine(msg.MessageType.ToString() + ": " + msg.ReadString());
                            break;
                        case NetIncomingMessageType.Data: 
                            Protocol.HandleMessage(msg, id); break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            if (status == NetConnectionStatus.Disconnected)
                            {
                                Console.WriteLine("Connection timed out: " + msg.SenderEndpoint.ToString());
                                Console.WriteLine("ClientID: " + id.ToString());
                                RemoveClient(id);
                            }
                            break;
                    }
                    connection.Recycle(msg);
                }
            }
        }

        public void ChangePlayerMap(Logic.Player player, int mapID, Vector2 position)
        {
            Client client = getClient(player);
            Logic.Map map = Logic.MapManager.GetMapByID(mapID);
            if (map != null && client != null)
                client.SetMap(map, position);
        }

        public void SendClient(Client client, IgorrMessage message)
        {
            connection.SendMessage(message.GetMessage(), client.Connection, NetDeliveryMethod.ReliableOrdered, currentChannel);
        }

        public void SetChannel(int channel)
        {
            currentChannel = channel;
        }

        public void SendClient(Logic.Player player, IgorrMessage message)
        {
            if (!message.Encoded)
            {
                Console.WriteLine("WARNING: Message not encoded: " + message.MessageType.ToString());
                message.Encode();
            }
            if (message.MessageType == MessageTypes.Chat)
                Console.WriteLine("I am sending a strange message");
            foreach (Client client in _clients.Values)
                if (client.PlayerID == player.ID)
                    connection.SendMessage(message.GetMessage(), client.Connection, NetDeliveryMethod.ReliableOrdered,currentChannel);
        }

        public Client getClient(Logic.Player player)
        {
            foreach (Client client in _clients.Values)
                if (client.PlayerID == player.ID)
                    return client;
            return null;
        }

        /*
        public void SendAll(IgorrMessage message)
        {
            if (connection.ConnectionsCount > 0)
                connection.SendToAll(message.GetMessage(), NetDeliveryMethod.ReliableSequenced);
        }
        */ 

        public void SendAllMap(Logic.Map map, IgorrMessage message, bool Reliable)
        {
            List<NetConnection> recipients = new List<NetConnection>();
            recipients.AddRange(_connections);
            for (int x = 0; x < recipients.Count; x++)
            {
                if (_clients[_clientids[recipients[x].RemoteUniqueIdentifier]].CurrentMap != map)
                {
                    recipients.RemoveAt(x);
                    x--;
                }
            }
            if (!message.Encoded)
            {
                Console.WriteLine("WARNING: Message not encoded: " + message.MessageType.ToString());
                message.Encode();
            }
            if (message.MessageType == MessageTypes.Chat || message.ReReadType == MessageTypes.Chat || message.ReReadType != message.MessageType)
                Console.WriteLine("I am sending a strange message");
            if (recipients.Count > 0 && Reliable)
                connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.ReliableSequenced, currentChannel);
            else if (recipients.Count > 0 && !Reliable && _enableSend)
                connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.UnreliableSequenced, currentChannel);
        }

        public void SendAllMapReliable(Logic.Map map, IgorrMessage message, bool ordered)
        {
            List<NetConnection> recipients = new List<NetConnection>();
            recipients.AddRange(_connections);
            for (int x = 0; x < recipients.Count; x++)
            {
                if (_clients[_clientids[recipients[x].RemoteUniqueIdentifier]].CurrentMap != map)
                {
                    recipients.RemoveAt(x);
                    x--;
                }
            }
            if (!message.Encoded)
            {
                Console.WriteLine("WARNING: Message not encoded: " + message.MessageType.ToString());
                message.Encode();
            }
            if (message.MessageType == MessageTypes.Chat || message.ReReadType == MessageTypes.Chat || message.ReReadType != message.MessageType)
                Console.WriteLine("I am sending a strange message");
            if (recipients.Count > 0)
            {
                if (ordered)
                    connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.ReliableOrdered, currentChannel);
                else
                    connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.ReliableUnordered, currentChannel);
            }
        }
        public void SendAllExcept(Logic.Map map, Client client, IgorrMessage message, bool Reliable)
        {
            if (_connections.Count > 1)
            {
                List<NetConnection> recipients = new List<NetConnection>();
                recipients.AddRange(_connections);
                recipients.Remove(client.Connection);
                for (int x = 0; x < recipients.Count; x++)
                {
                    if (_clients[_clientids[recipients[x].RemoteUniqueIdentifier]].CurrentMap != map)
                    {
                        recipients.RemoveAt(x);
                        x--;
                    }
                }
                if (!message.Encoded)
                {
                    Console.WriteLine("WARNING: Message not encoded: " + message.MessageType.ToString());
                    message.Encode();
                } 
                if (recipients.Count > 0 && Reliable)
                    connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.ReliableSequenced, currentChannel);
                else if (recipients.Count > 0 && !Reliable && !_enableSend)
                    connection.SendMessage(message.GetMessage(), recipients, NetDeliveryMethod.Unreliable, currentChannel);
            }
        }
         

        void HandleChat(IgorrMessage message)
        {
            ChatMessage cm = (ChatMessage)(message);
            Console.WriteLine("Chat: "+ cm.Text);
        }

        void HandlePosition(IgorrMessage message)
        {
            PositionMessage pm = (PositionMessage)(message);
            //Console.WriteLine("Position: id: " + pm.id.ToString() + " Position: " + pm.Position.ToString());
            Client client;
            if (_clients.TryGetValue(message.clientID, out client))
            {
                PositionMessage pm2 = (PositionMessage)Protocol.NewMessage(MessageTypes.Position);
                pm2.id = pm.id;
                pm2.Position = pm.Position;
                pm2.Move = pm.Move;
                pm2.TimeStamp = pm.TimeStamp;
                pm2.Encode();
                if (client.CurrentMap.ObjectManager.UpdatePosition(pm.Position, pm.Move, pm.id, pm.TimeStamp))
                    SendAllExcept(client.CurrentMap, client, pm2, true);
            }
        }

        void HandleAttack(IgorrMessage message)
        {
            AttackMessage am = (AttackMessage)(message);
            _clients[message.clientID].CurrentMap.ObjectManager.SpawnAttack(am.attackerID, am.attackID);
        }

        void HandleJoin(IgorrMessage message)
        {
            JoinMessage jm = (JoinMessage)(message);

            if (Management.LoginData.CheckLogin(jm.Name, jm.Password))
            {
                int id = ObjectManager.getID();

                Client client = new Client(message.SenderConnection, jm.Name);
                client.PlayerID = id;
                _clientids.Add(client.Connection.RemoteUniqueIdentifier, client.ID);
                _clients.Add(client.ID, client);
                _connections.Add(client.Connection);

                Logic.Map targetMap = Logic.MapManager.GetMapByID(0);
                client.SetMap(targetMap, Vector2.Zero);

                /*
                SpawnMessage sm = (SpawnMessage)Protocol.NewMessage(MessageTypes.Spawn);
                sm.position = new Vector2(400, 440);
                sm.objectType = 'a' - 'a';
                sm.id = id;
                sm.Encode();
                SendAll(sm);
                 */

                /*
                ChatMessage cm = (ChatMessage)Protocol.GetContainerMessage(MessageTypes.Chat, client.Connection);
                Protocol.SendContainer(cm, client.Connection);
                Protocol.FlushContainer(client.Connection);
                 */
                Point spawnPoint = targetMap.getRandomSpawn();
                targetMap.ObjectManager.Add(new Vector2(spawnPoint.X, spawnPoint.Y), id, client.Name);

                AssignPlayerMessage apm = (AssignPlayerMessage)Protocol.NewMessage(MessageTypes.AssignPlayer);
                apm.objectID = id;
                apm.Encode();
                SendClient(client, apm);

                Console.WriteLine(client.Name + " joined");
            }
            else
                Console.WriteLine("Invalid Login: Name: " + jm.Name + " Password: " + jm.Password);
        }

        void HandleLeave(IgorrMessage message)
        {
            LeaveMessage lm = (LeaveMessage)(message);
            Console.WriteLine("Leave Message: ClientID: " + message.clientID);
            RemoveClient(message.clientID);
        }

        void RemoveClient(int clientID)
        {
            Client client;
            if (_clients.TryGetValue(clientID, out client))
            {
                _clientids.Remove(client.Connection.RemoteUniqueIdentifier);
                Console.WriteLine("Removing: " + client.Connection.RemoteEndpoint.ToString());
                _connections.Remove(client.Connection);
                DeSpawnMessage dsm = (DeSpawnMessage)Protocol.NewMessage(MessageTypes.DeSpawn);
                dsm.id = client.PlayerID;
                _clients[client.ID].CurrentMap.ObjectManager.Remove(client.PlayerID);
                _clients.Remove(client.ID);
                dsm.Encode();
                SendAllMap(client.CurrentMap, dsm, true);
                client.Connection.Disconnect("Ciao");
                Console.WriteLine(clientID.ToString() + " left");
            }
        }

        public void Status(string par)
        {
            TimeSpan totalTime = DateTime.Now - _lastTime;
            Console.WriteLine("Active Maps: " + IGORR_Server.Logic.MapManager.ActiveMaps().ToString());
            Console.WriteLine();
            Console.WriteLine("Sent Bytes: "+(connection.Statistics.SentBytes).ToString());
            Console.WriteLine("Sent Messages: " + (connection.Statistics.SentMessages ).ToString());
            Console.WriteLine("Sent Packets: " + (connection.Statistics.SentPackets ).ToString());
            Console.WriteLine("Sent Bytes/s: " + ((connection.Statistics.SentBytes -lastSent) / totalTime.TotalSeconds).ToString());
            Console.WriteLine();
            Console.WriteLine("Recv Bytes: " + (connection.Statistics.ReceivedBytes ).ToString());
            Console.WriteLine("Recv Messages: " + (connection.Statistics.ReceivedMessages ).ToString());
            Console.WriteLine("Recv Packets: " + (connection.Statistics.ReceivedPackets ).ToString());
            Console.WriteLine("Recv Bytes/s: " + ((connection.Statistics.ReceivedBytes -lastRecv) / totalTime.TotalSeconds).ToString());
            lastSent = connection.Statistics.SentBytes;
            lastRecv = connection.Statistics.ReceivedBytes;
            _lastTime = DateTime.Now;
        }

        public void StatusTotal(string par)
        {
            TimeSpan totalTime = DateTime.Now - _startTime;
            Console.WriteLine("Active Maps: " + IGORR_Server.Logic.MapManager.ActiveMaps().ToString());
            Console.WriteLine();
            Console.WriteLine("Sent Bytes: " + connection.Statistics.SentBytes.ToString());
            Console.WriteLine("Sent Messages: " + connection.Statistics.SentMessages.ToString());
            Console.WriteLine("Sent Packets: " + connection.Statistics.SentPackets.ToString());
            Console.WriteLine("Sent Bytes/s: " + (connection.Statistics.SentBytes / totalTime.TotalSeconds).ToString());
            Console.WriteLine();
            Console.WriteLine("Recv Bytes: " + connection.Statistics.ReceivedBytes.ToString());
            Console.WriteLine("Recv Messages: " + connection.Statistics.ReceivedMessages.ToString());
            Console.WriteLine("Recv Packets: " + connection.Statistics.ReceivedPackets.ToString());
            Console.WriteLine("Recv Bytes/s: " + (connection.Statistics.ReceivedBytes / totalTime.TotalSeconds).ToString());
            _startTime = DateTime.Now;
        }

        public bool Enabled
        {
            get { return _enableSend; }
        }
    }
}
