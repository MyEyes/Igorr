using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol
{
    using System.IO;
    public static class Logger
    {
        static bool newRun = true;
        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = null;
            if (!newRun)
                sw = System.IO.File.AppendText(
                    "Log.txt");
            else
            {
                sw = System.IO.File.CreateText(
                    "Log.txt");
                newRun = false;
            }
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }

    public enum MessageTypes
    {
        Chat,
        Position,
        Join,
        Leave,
        Spawn,
        AssignPlayer,
        DeSpawn,
        Pickup,
        Kill,
        Damage,
        Attack,
        SetAnimation,
        ChangeTile,
        ChangeMap,
        Container,
        Play,
        SetGlow,
        Shadow,
        SetHP,
        PlayerInfoMessage,
        ExpMessage,
        Knockback,
        SpawnAttack,
        ObjectInfo
    }

    public delegate void MessageHandler(IgorrMessage message);

    public static class Protocol
    {
        static NetPeer _connection;
        static long _timeStamp;
        static Dictionary<MessageTypes, MessageHandler> _handlers;
        static Dictionary<NetConnection, Messages.ContainerMessage> _containers;
        static Messages.ContainerMessage _allcontainer;
        static Protocol()
        {
            _handlers = new Dictionary<MessageTypes, MessageHandler>();
            _containers = new Dictionary<NetConnection, Messages.ContainerMessage>();
            RegisterMessageHandler(MessageTypes.Container, new MessageHandler(HandleContainer));
        }

        public static void SetUp(NetPeer connection)
        {
            _connection = connection;
        }

        public static void RegisterMessageHandler(MessageTypes type, MessageHandler handler)
        {
            if (!_handlers.ContainsKey(type))
                _handlers.Add(type, handler);
            else
                _handlers[type] = handler;
        }

        public static IgorrMessage GetContainerMessage(MessageTypes type, NetConnection target)
        {
            if (target == null)
            {
                if(_allcontainer==null)
                    _allcontainer = (Messages.ContainerMessage)NewMessage(MessageTypes.Container);
                return NewMessage(type, _allcontainer.outgoing);
            }
            else if (_containers.ContainsKey(target))
                return NewMessage(type, _containers[target].outgoing);
            else
            {
                Messages.ContainerMessage ctm = (Messages.ContainerMessage)NewMessage(MessageTypes.Container);
                _containers.Add(target, ctm);
                return NewMessage(type, _containers[target].outgoing);
            }
        }

        public static IgorrMessage NewMessage(MessageTypes type)
        {
            return NewMessage(type, _connection.CreateMessage());
        }
        private static IgorrMessage NewMessage(MessageTypes type, NetOutgoingMessage outgoing)
        {
            IgorrMessage message = null;
            switch (type)
            {
                case MessageTypes.Chat:
                    Console.WriteLine("I am sending a chat message");
                    message = new Messages.ChatMessage(outgoing, _timeStamp); break;
                case MessageTypes.Position: message = new Messages.PositionMessage(outgoing, _timeStamp); break;
                case MessageTypes.Leave: message = new Messages.LeaveMessage(outgoing, _timeStamp); break;
                case MessageTypes.Join: message = new Messages.JoinMessage(outgoing, _timeStamp); break;
                case MessageTypes.Spawn: message = new Messages.SpawnMessage(outgoing, _timeStamp); break;
                case MessageTypes.SpawnAttack: message = new Messages.SpawnAttackMessage(outgoing, _timeStamp); break;
                case MessageTypes.AssignPlayer: message = new Messages.AssignPlayerMessage(outgoing, _timeStamp); break;
                case MessageTypes.DeSpawn: message = new Messages.DeSpawnMessage(outgoing, _timeStamp); break;
                case MessageTypes.Pickup: message = new Messages.PickupMessage(outgoing, _timeStamp); break;
                case MessageTypes.Kill: message = new Messages.KillMessage(outgoing, _timeStamp); break;
                case MessageTypes.Damage: message = new Messages.DamageMessage(outgoing, _timeStamp); break;
                case MessageTypes.Attack: message = new Messages.AttackMessage(outgoing, _timeStamp); break;
                case MessageTypes.SetAnimation: message = new Messages.SetAnimationMessage(outgoing, _timeStamp); break;
                case MessageTypes.ChangeTile: message = new Messages.ChangeTileMessage(outgoing, _timeStamp); break;
                case MessageTypes.ChangeMap: message = new Messages.ChangeMapMessage(outgoing, _timeStamp); break;
                case MessageTypes.Container: message = new Messages.ContainerMessage(outgoing, _timeStamp); break;
                case MessageTypes.Play: message = new Messages.PlayMessage(outgoing, _timeStamp); break;
                case MessageTypes.SetGlow: message = new Messages.SetGlowMessage(outgoing, _timeStamp); break;
                case MessageTypes.Shadow: message = new Messages.ShadowMessage(outgoing, _timeStamp); break;
                case MessageTypes.SetHP: message = new Messages.SetPlayerStatusMessage(outgoing, _timeStamp); break;
                case MessageTypes.PlayerInfoMessage: message = new Messages.PlayerInfoMessage(outgoing, _timeStamp); break;
                case MessageTypes.ExpMessage: message = new Messages.ExpMessage(outgoing, _timeStamp); break;
                case MessageTypes.Knockback: message = new Messages.KnockbackMessage(outgoing, _timeStamp); break;
                case MessageTypes.ObjectInfo: message = new Messages.ObjectInfoMessage(outgoing, _timeStamp); break;
            }
             
            return message;
        }

        public static void Update(int ms)
        {
            _timeStamp += ms;
        }

        public static IgorrMessage DecodeMessage(IgorrMessage m)
        {
            IgorrMessage message = null;
            switch (m.MessageType)
            {
                case MessageTypes.Chat:
                    Logger.LogMessageToFile("This should never happen: I got a Chat Message");
                    message = new Messages.ChatMessage(m); 
                    break;
                case MessageTypes.Position: message = new Messages.PositionMessage(m); break;
                case MessageTypes.Leave: message = new Messages.LeaveMessage(m); break;
                case MessageTypes.Join: message = new Messages.JoinMessage(m); break;
                case MessageTypes.Spawn: message = new Messages.SpawnMessage(m); break;
                case MessageTypes.SpawnAttack: message = new Messages.SpawnAttackMessage(m); break;
                case MessageTypes.AssignPlayer: message = new Messages.AssignPlayerMessage(m); break;
                case MessageTypes.DeSpawn: message = new Messages.DeSpawnMessage(m); break;
                case MessageTypes.Pickup: message = new Messages.PickupMessage(m); break;
                case MessageTypes.Kill: message = new Messages.KillMessage(m); break;
                case MessageTypes.Damage: message = new Messages.DamageMessage(m); break;
                case MessageTypes.Attack: message = new Messages.AttackMessage(m); break;
                case MessageTypes.SetAnimation: message = new Messages.SetAnimationMessage(m); break;
                case MessageTypes.ChangeTile: message = new Messages.ChangeTileMessage(m); break;
                case MessageTypes.ChangeMap: message = new Messages.ChangeMapMessage(m); break;
                case MessageTypes.Container: message = new Messages.ContainerMessage(m); break;
                case MessageTypes.Play: message = new Messages.PlayMessage(m); break;
                case MessageTypes.SetGlow: message = new Messages.SetGlowMessage(m); break;
                case MessageTypes.Shadow: message = new Messages.ShadowMessage(m); break;
                case MessageTypes.SetHP: message = new Messages.SetPlayerStatusMessage(m); break;
                case MessageTypes.PlayerInfoMessage: message = new Messages.PlayerInfoMessage(m); break;
                case MessageTypes.ExpMessage: message = new Messages.ExpMessage(m); break;
                case MessageTypes.Knockback: message = new Messages.KnockbackMessage(m); break;
                case MessageTypes.ObjectInfo: message = new Messages.ObjectInfoMessage(m); break;
            }
            return message;
        }
        /*
        public static void Send(IgorrMessage message, NetConnection target)
        {
            if (!message.Encoded)
                message.Encode();
            _connection.SendMessage(message.GetMessage(), target, NetDeliveryMethod.ReliableSequenced);
        }
         */
        
        public static void SendContainer(IgorrMessage message, NetConnection target)
        {
            if (target == null)
            {
                _allcontainer.AddMessage(message);
            }
            else if (_containers.ContainsKey(target))
                _containers[target].AddMessage(message);
        }
         

        public static void FlushContainer(NetConnection target, int channel)
        {
            //Console.WriteLine("Sending Container to: " + target.ToString());
            if (target == null &&_allcontainer!=null)
            {
                _allcontainer.Encode();
                if (_connection is NetServer && _connection.ConnectionsCount>0)
                    (_connection as NetServer).SendToAll(_allcontainer.GetMessage(), null,NetDeliveryMethod.ReliableOrdered,channel);
                //Console.WriteLine(_allcontainer.GetMessage().LengthBytes);
                _allcontainer = null;
            }
            else if (target != null && _containers.ContainsKey(target))
            {
                _containers[target].Encode();
                _connection.SendMessage(_containers[target].GetMessage(), target, NetDeliveryMethod.ReliableOrdered, channel);
                //Console.WriteLine("Sending Container Message: " + _containers[target].ReReadType.ToString() + " " + _containers[target].outgoing.ToString()+ " " +target.RemoteEndpoint.ToString());
                //Console.WriteLine(target.Status.ToString());
                for (int x = 0; x < _containers[target].Messages.Count; x++)
                {
                   // Console.WriteLine("\t\t" + _containers[target].Messages[x].MessageType.ToString());
                }
                _containers.Remove(target);
            }
            else if (target != null)
            {
                Console.WriteLine("ERROR: could not send container message to: " + target.ToString());
            }
        }

        public static void HandleMessage(NetIncomingMessage incoming, int clientID)
        {
            IgorrMessage message = null;

            message = new IgorrMessage(incoming);
            message.clientID = clientID;
            HandleMessage(DecodeMessage(message));
        }

        public static void HandleMessage(IgorrMessage message)
        {
            //Logger.LogMessageToFile("Trying to handle " + message.MessageType.ToString() + " message!");
            if (_handlers.ContainsKey(message.MessageType))
                _handlers[message.MessageType](message);
            else
                Logger.LogMessageToFile("Could not handle " + message.MessageType.ToString() + " message!");
        }

        private static void HandleContainer(IgorrMessage message)
        {
            Messages.ContainerMessage ctm = (Messages.ContainerMessage)(message);
            for (int x = 0; x < ctm.Messages.Count; x++)
            {
                //Logger.LogMessageToFile("\t\t" + message.MessageType.ToString() + " message!");
                HandleMessage(ctm.Messages[x]);
            }
        }
    }
}