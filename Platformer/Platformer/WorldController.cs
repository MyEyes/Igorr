using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using IGORRProtocol;
using IGORRProtocol.Messages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

namespace Platformer
{
    static class WorldController
    {
        static Game1 _gameRef;
        static NetClient connection;
        static ObjectManager manager;
        public static ContentManager Content;
        static Thread receiveThread;
        static bool _started = false;
        static Random _random;

        static WorldController()
        {
            _random = new Random();
        }

        public static void Start()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("IGORR");
            //config.SimulatedMinimumLatency = 0.5f;
            //config.SimulatedRandomLatency = 0.5f;
            connection = new NetClient(config);
            connection.Start();
            connection.Connect(Settings.ServerAddress, 5445);
            //connection.Connect("firzen.dyndns.org", 5445);
            System.Threading.SpinWait.SpinUntil(new Func<bool>(delegate { return connection.ServerConnection != null; }), 5000);
            if (connection.ServerConnection == null)
            {
                MessageBox.Show("Could not connect to server.\nAddress: " + Settings.ServerAddress, "Connection Error");
                Thread.CurrentThread.Abort();
            }
            Protocol.SetUp(connection);
            Protocol.RegisterMessageHandler(MessageTypes.Spawn, new MessageHandler(HandleSpawn));
            Protocol.RegisterMessageHandler(MessageTypes.SpawnAttack, new MessageHandler(HandleSpawnAttack));
            Protocol.RegisterMessageHandler(MessageTypes.AssignPlayer, new MessageHandler(HandleAssignPlayer));
            Protocol.RegisterMessageHandler(MessageTypes.Position, new MessageHandler(HandlePosition));
            Protocol.RegisterMessageHandler(MessageTypes.DeSpawn, new MessageHandler(HandleDespawn));
            Protocol.RegisterMessageHandler(MessageTypes.Pickup, new MessageHandler(HandlePickup));
            Protocol.RegisterMessageHandler(MessageTypes.Kill, new MessageHandler(HandleKill));
            Protocol.RegisterMessageHandler(MessageTypes.Damage, new MessageHandler(HandleDamage));
            Protocol.RegisterMessageHandler(MessageTypes.SetAnimation, new MessageHandler(HandleSetAnimation));
            Protocol.RegisterMessageHandler(MessageTypes.SetGlow, new MessageHandler(HandleSetGlow));
            Protocol.RegisterMessageHandler(MessageTypes.ChangeTile, new MessageHandler(HandleChangeTile));
            Protocol.RegisterMessageHandler(MessageTypes.ChangeMap, new MessageHandler(HandleChangeMap));
            Protocol.RegisterMessageHandler(MessageTypes.Play, new MessageHandler(HandlePlay));
            Protocol.RegisterMessageHandler(MessageTypes.Shadow, new MessageHandler(HandleShadows));
            Protocol.RegisterMessageHandler(MessageTypes.SetHP, new MessageHandler(HandleSetHP));
            Protocol.RegisterMessageHandler(MessageTypes.PlayerInfoMessage, new MessageHandler(HandlePlayerInfo));
            Protocol.RegisterMessageHandler(MessageTypes.ExpMessage, new MessageHandler(HandleExp));
            Protocol.RegisterMessageHandler(MessageTypes.Knockback, new MessageHandler(HandleKnockback));
            receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start();
            System.Threading.SpinWait.SpinUntil(new Func<bool>(delegate { return connection.ServerConnection.Status == NetConnectionStatus.Connected; }), 5000);
            JoinMessage m = (JoinMessage)Protocol.NewMessage(MessageTypes.Join);
            m.Name = Settings.LoginName;
            m.Password = Settings.LoginPassword;
            m.Encode();
            SendReliable(m);
            _started = true;
        }

        public static void SetObjectManager(ObjectManager om)
        {
            manager = om;
        }

        public static void SetContent(ContentManager manager)
        {
            Content = manager;
        }

        public static void SetGame(Game1 game)
        {
            _gameRef = game;
        }

        public static void SendText(string value)
        {
            if (!_started)
                return;
            ChatMessage message = (ChatMessage)Protocol.NewMessage(MessageTypes.Chat);
            message.Text = value; 
            SendReliable(message);
        }

        public static void SendReliable(IgorrMessage m)
        {
            m.Encode();
            if (connection.ServerConnection != null)
                    connection.SendMessage(m.GetMessage(), NetDeliveryMethod.ReliableOrdered);
        }

        public static void Receive()
        {
            while (true)
            {
                connection.MessageReceivedEvent.WaitOne();
                NetIncomingMessage msg;
                while ((msg = connection.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                            Console.WriteLine(msg.MessageType.ToString() + ": " + msg.ReadString());
                            break;
                        case NetIncomingMessageType.Data: try { Protocol.HandleMessage(msg, -1); }
                            catch (Exception e) {MessageBox.Show(e.ToString(),"ERROR"); } break;
                        default: Console.WriteLine("Unhandled Message Type: " + msg.MessageType.ToString());
                            break;

                    }
                    connection.Recycle(msg);
                }
            }
        
        }

        public static void Leave()
        {
            if (!_started)
                return;
            LeaveMessage message = (LeaveMessage)Protocol.NewMessage(MessageTypes.Leave);
            message.Encode();
            SendReliable(message);
        }

        public static void SendPosition(GameObject obj)
        {
            if (!_started)
                return;
            PositionMessage message = (PositionMessage)Protocol.NewMessage(MessageTypes.Position);
            message.Position = obj.Position;
            if (obj is Player)
                message.Move = (obj as Player).Speed;
            message.id = obj.ID;
            Send(message, true);
        }

        public static void SendAttack(int attackID, int playerID)
        {
            AttackMessage am = (AttackMessage)Protocol.NewMessage(MessageTypes.Attack);
            am.attackerID = playerID;
            am.attackID = attackID;
            am.Encode();
            Send(am, false);
        }

        public static void Send(IgorrMessage message, bool Sequenced)
        {
            message.Encode();
            if (connection.ServerConnection != null)
                if (Sequenced)
                    connection.SendMessage(message.GetMessage(), NetDeliveryMethod.Unreliable);
                else
                    connection.SendMessage(message.GetMessage(), NetDeliveryMethod.UnreliableSequenced);
                //Protocol.Send(message, connection.ServerConnection);
        }

        public static void HandleSpawn(IgorrMessage message)
        {
            SpawnMessage sm = (SpawnMessage)message;
            manager.SpawnObject(sm.position,sm.move, sm.objectType, sm.id, sm.Name, sm.CharName);
        }

        public static void HandleAssignPlayer(IgorrMessage message)
        {
            AssignPlayerMessage apm = (AssignPlayerMessage)message;
            manager.SelectPlayer(apm.objectID);
        }

        public static void HandlePosition(IgorrMessage message)
        {
            PositionMessage pm = (PositionMessage)(message);
            manager.SetPosition(pm.Position, pm.Move,pm.id, pm.TimeStamp);
        }

        public static void HandleDespawn(IgorrMessage message)
        {
            DeSpawnMessage dsm = (DeSpawnMessage)(message);
            manager.RemoveObject(dsm.id);
        }

        public static void HandlePickup(IgorrMessage message)
        {
            PickupMessage pum = (PickupMessage)(message);
            switch (pum.id)
            {
                case 'b' - 'a': manager.Player.GivePart(new Legs(null)); break;
                case 'd' - 'a': manager.Player.GivePart(new Striker(null)); break;
                case 'e' - 'a': manager.Player.GivePart(new Wings(null)); break;
            }
        }

        public static void HandleShadows(IgorrMessage message)
        {
            ShadowMessage sm = (ShadowMessage)message;
            _gameRef.Shadows = sm.shadows;
        }

        public static void HandleKill(IgorrMessage message)
        {
            KillMessage km = (KillMessage)(message);
            Player deadguy = manager.GetPlayer(km.deadID);
            Player killer = manager.GetPlayer(km.killerID);
            if (killer == null && deadguy != null)
                TextManager.AddInfo(deadguy.Name + " just killed himself!");
            else if (killer != null && deadguy != null)
                TextManager.AddInfo(deadguy.Name + " was just killed by " + killer.Name + "!");
            manager.RemoveObject(km.deadID);
        }

        public static void HandleAttack(IgorrMessage message)
        {

        }

        public static void HandleSpawnAttack(IgorrMessage message)
        {
            SpawnAttackMessage sam = (SpawnAttackMessage)message;
            manager.SpawnAttack(sam.position, sam.move, sam.id);
        }

        public static void HandleDamage(IgorrMessage message)
        {
            DamageMessage dm = (DamageMessage)(message);
            manager.DealDamage(dm.damage,new Vector2(dm.posX,dm.posY), dm.playerID);
        }

        public static void HandleSetAnimation(IgorrMessage message)
        {
            SetAnimationMessage sam = (SetAnimationMessage)(message);
            Player p = manager.GetPlayer(sam.objectID);
            if (p != null)
            {
                p.SetAnimation(sam.force, sam.animationNumber);
            }
        }

        public static void HandleChangeTile(IgorrMessage message)
        {
            ChangeTileMessage ctm = (ChangeTileMessage)(message);
            manager.Map.ChangeTile(ctm.x, ctm.y, ctm.layer, ctm.tileID);
        }

        public static void HandleChangeMap(IgorrMessage message)
        {
            ChangeMapMessage cmm = (ChangeMapMessage)message;
            manager.Clear();
            _gameRef.Light.Clear();
            _gameRef.LoadMap(cmm.mapid);
        }

        public static void HandlePlay(IgorrMessage message)
        {
            PlayMessage pm = (PlayMessage)(message);
            MusicPlayer.PlaySong(pm.SongName, pm.Loop, pm.Queue);
        }

        public static void HandleSetGlow(IgorrMessage message)
        {
            SetGlowMessage sgm = (SetGlowMessage)message;
            if (_gameRef.Map != null)
                _gameRef.Light.SetGlow(sgm.id, sgm.Position, sgm.radius, sgm.shadows);
        }

        public static void HandleSetHP(IgorrMessage message)
        {
            SetPlayerStatusMessage shpm = (SetPlayerStatusMessage)message;
            Player player = manager.GetPlayer(shpm.playerID);
            if (player != null)
            {
                player.MaxHP = shpm.maxHP;
                player.HP = shpm.currentHP;
                player.Exp = shpm.Exp;
                player.LastLevelExp = shpm.lastLevelExp;
                player.NextLevelExp = shpm.nextLevelExp;
            }
        }

        public static void HandlePlayerInfo(IgorrMessage message)
        {
            PlayerInfoMessage pim = (PlayerInfoMessage)message;
            Player player=manager.GetPlayer(pim.playerID);
            if (player != null)
                TextManager.AddText(player.Position, 0, pim.Text, 5000, Microsoft.Xna.Framework.Color.Yellow, Microsoft.Xna.Framework.Color.Transparent);
        }

        public static void HandleExp(IgorrMessage message)
        {
            ExpMessage xpm=(ExpMessage)message;
            if (manager.Player != null)
            {
                manager.Player.GetExp(xpm.exp);
                Particles.ExpParticles(xpm.exp, xpm.startPos, manager.Player);
            }
        }

        public static void HandleKnockback(IgorrMessage message)
        {
            KnockbackMessage kbm = (KnockbackMessage)message;
            Player player = manager.GetPlayer(kbm.id);
            if (player != null)
                player.Knockback(kbm.Move);
        }

        public static void Exit()
        {
            receiveThread.Abort();
        }

        public static ParticleManager Particles
        {
            get { return _gameRef.Particles; }
        }

        public static Camera Camera
        {
            get { return _gameRef.Cam; }
        }

        public static Random Random
        {
            get { return _random; }
        }
    }
}
