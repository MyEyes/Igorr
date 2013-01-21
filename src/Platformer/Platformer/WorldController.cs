using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Client
{
    static class WorldController
    {
        static GameScreen _gameRef;
        static NetClient connection;
        static ObjectManager manager;
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
            ProtocolHelper.SetUp(connection);
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Spawn, new MessageHandler(HandleSpawn));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.SpawnAttack, new MessageHandler(HandleSpawnAttack));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.AssignPlayer, new MessageHandler(HandleAssignPlayer));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Position, new MessageHandler(HandlePosition));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.DeSpawn, new MessageHandler(HandleDespawn));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Pickup, new MessageHandler(HandlePickup));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Kill, new MessageHandler(HandleKill));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Damage, new MessageHandler(HandleDamage));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.SetAnimation, new MessageHandler(HandleSetAnimation));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.SetGlow, new MessageHandler(HandleSetGlow));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.ChangeTile, new MessageHandler(HandleChangeTile));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.ChangeMap, new MessageHandler(HandleChangeMap));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Play, new MessageHandler(HandlePlay));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Shadow, new MessageHandler(HandleShadows));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.SetHP, new MessageHandler(HandleSetHP));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.PlayerInfo, new MessageHandler(HandlePlayerInfo));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.ExpMessage, new MessageHandler(HandleExp));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Knockback, new MessageHandler(HandleKnockback));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.ObjectInfo, new MessageHandler(HandleObjectInfo));
            receiveThread = new Thread(new ThreadStart(Receive));
            receiveThread.Start();
            System.Threading.SpinWait.SpinUntil(new Func<bool>(delegate { return connection.ServerConnection.Status == NetConnectionStatus.Connected; }), 5000);
            JoinMessage m = (JoinMessage)ProtocolHelper.NewMessage(MessageTypes.Join);
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


        public static void SetGame(GameScreen game)
        {
            _gameRef = game;
        }

        public static void SendText(string value)
        {
            if (!_started)
                return;
            ChatMessage message = (ChatMessage)ProtocolHelper.NewMessage(MessageTypes.Chat);
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
                        case NetIncomingMessageType.Data: try { ProtocolHelper.HandleMessage(msg, -1); }
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
            LeaveMessage message = (LeaveMessage)ProtocolHelper.NewMessage(MessageTypes.Leave);
            message.Encode();
            SendReliable(message);
        }

        public static void SendPosition(GameObject obj)
        {
            if (!_started)
                return;
            PositionMessage message = (PositionMessage)ProtocolHelper.NewMessage(MessageTypes.Position);
            message.Position = obj.Position;
            if (obj is Player)
                message.Move = (obj as Player).Speed;
            message.id = obj.ID;
            Send(message, true);
        }

        public static void SendAttack(int attackID, Vector2 dir, int playerID)
        {
            AttackMessage am = (AttackMessage)ProtocolHelper.NewMessage(MessageTypes.Attack);
            am.attackerID = playerID;
            dir.Normalize();
            am.attackDir = dir;
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
                //ProtocolHelper.Send(message, connection.ServerConnection);
        }

        public static void HandleSpawn(IgorrMessage message)
        {
            SpawnMessage sm = (SpawnMessage)message;
            manager.SpawnObject(sm.position,sm.move, sm.objectType,sm.groupID, sm.id, sm.Name, sm.CharName);
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

        public static void HandleObjectInfo(IgorrMessage message)
        {
            ObjectInfoMessage oim = (ObjectInfoMessage)message;
            manager.GetObjectInfo(oim.objectID, oim.info);
        }

        public static void HandleSetGlow(IgorrMessage message)
        {
            SetGlowMessage sgm = (SetGlowMessage)message;
            if (_gameRef.Map != null)
                _gameRef.Light.SetGlow(sgm.id, sgm.Position, sgm.Color, sgm.radius, sgm.shadows);
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
            if (receiveThread != null)
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
