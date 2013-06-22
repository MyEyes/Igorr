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
using IGORR.Client.Logic;
using IGORR.Modules;

namespace IGORR.Client
{
    static class WorldController
    {
        static GameScreen _gameRef;
        static NetClient connection;
        static ObjectManager manager;
        static ProtocolHelper _protocolHelper;
        static Thread receiveThread;
        static bool _started = false;
        static Random _random;
        static bool _enabled=true;
        const int MaxNonUpdateTries = 30;
        static int countdown = MaxNonUpdateTries;

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
                //Thread.CurrentThread.Abort();
                return;
            }

            _protocolHelper = new ProtocolHelper(connection);
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
            ProtocolHelper.RegisterMessageHandler(MessageTypes.DoEffect, new MessageHandler(HandleDoEffect));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Chat, new MessageHandler(HandleChat));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Interact, new MessageHandler(HandleInteract));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Container, new MessageHandler(HandleContainer));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.BodyConfiguration, new MessageHandler(HandleBodyConfiguration));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.AttachAnimation, new MessageHandler(HandleAttachAnimation));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.Stun, new MessageHandler(HandleStun));
            ProtocolHelper.RegisterMessageHandler(MessageTypes.MoveItem, new MessageHandler(HandleMoveItem));
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

        public static void SendReliable(IgorrMessage m)
        {
            if (!_enabled)
                return;
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

        public static void SendPosition(Player obj)
        {
            countdown--;
            if (!_started || !_enabled || !(obj.ChangedMovement))
                return;
            countdown=MaxNonUpdateTries;
            PositionMessage message = (PositionMessage)ProtocolHelper.NewMessage(MessageTypes.Position);
            message.Position = obj.Position;
            message.Move = new Vector3(obj.Movement, obj.Speed.Y);
            message.id = obj.ID;
            Send(message, true);
        }

        public static void SendAttack(int attackID, Vector2 dir, int playerID)
        {
            if (!_enabled)
                return;
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
            if (!_enabled)
                return;
            message.Encode();
            if (connection.ServerConnection != null)
                if (!Sequenced)
                    connection.SendMessage(message.GetMessage(), NetDeliveryMethod.Unreliable);
                else
                    connection.SendMessage(message.GetMessage(), NetDeliveryMethod.UnreliableSequenced);
                //ProtocolHelper.Send(message, connection.ServerConnection);
        }

        public static void HandleChat(IgorrMessage message)
        {
            ChatMessage cm = (ChatMessage)message;
            GameObject obj = null;
            if (cm.objID >= 0)
                obj = manager.GetObject(cm.objID);
            if (obj != null)
                TextManager.Say(obj, cm.Text, cm.timeout);
            else
                TextManager.Say(cm.pos, cm.Text, cm.timeout);
        }

        public static void HandleSpawn(IgorrMessage message)
        {
            SpawnMessage sm = (SpawnMessage)message;
            manager.SpawnObject(sm.position,sm.move, sm.objectType, sm.id, sm.Info);
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
            GameObject obj = ModuleManager.SpawnByIdClient(null, pum.id, -1, Point.Zero, "");
            Player player = manager.GetPlayer(pum.PlayerID);
            if (obj != null && player!=null && obj is PartContainer)
            {
                player.GivePart((obj as PartContainer).Part, pum.autoEquip);
                _gameRef.GUI.UpdateInventoryWindow();
            }
            /*
            PickupMessage pum = (PickupMessage)(message);
            switch (pum.id)
            {
                case 'b' - 'a': manager.Player.GivePart(new Legs(null)); break;
                case 'd' - 'a': manager.Player.GivePart(new Striker(null)); break;
                case 'e' - 'a': manager.Player.GivePart(new Wings(null)); break;
            }
             */
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
            manager.SpawnAttack(sam.position, sam.move, sam.id, sam.info,sam.attackID);
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

        public static void HandleAttachAnimation(IgorrMessage message)
        {
            AttachAnimationMessage aam = (AttachAnimationMessage)message;
            Player player = manager.GetPlayer(aam.playerID);
            if (player != null)
                player.AttachAnimation(aam.animationFile);
        }

        public static void HandleStun(IgorrMessage message)
        {
            StunMessage sm = (StunMessage)message;
            Player player = manager.GetPlayer(sm.id);
            if (player != null)
                player.Stun(sm.stunTime);
        }

        public static void HandleDoEffect(IgorrMessage message)
        {
            DoEffectMessage dem = (DoEffectMessage)message;
            IGORR.Modules.ModuleManager.DoEffect(dem.EffectID,manager.Map,dem.Dir,dem.Position,dem.Info);
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

        public static void HandleMoveItem(IgorrMessage message)
        {
            MoveItemMessage mim = (MoveItemMessage)(message);
            Player player = manager.GetPlayer(mim.PlayerID);
            if (player == null)
                return;
            PartContainer cont = Modules.ModuleManager.SpawnByIdClient(null, mim.id, -1, Point.Zero, null) as PartContainer;
            Logic.Body.BodyPart part = cont != null ? cont.Part : null;
            if (mim.To == MoveTarget.Body)
                player.Body.TryEquip(mim.Slot, part);
            if (mim.From == MoveTarget.Body)
                player.Body.Unequip(part);
        }

        public static void HandleKnockback(IgorrMessage message)
        {
            KnockbackMessage kbm = (KnockbackMessage)message;
            Player player = manager.GetPlayer(kbm.id);
            if (player != null)
                player.Knockback(kbm.Move);
        }

        public static void HandleInteract(IgorrMessage message)
        {
            InteractMessage im = (InteractMessage)message;
            if (im.action == InteractAction.Ask)
            {
                Player player = manager.GetPlayer(im.objectID);
                string[] split = im.sinfo.Split(';');
                List<Choice> choices = new List<Choice>();
                string text = split[0];
                int counter = 1;
                while (counter + 1 < split.Length)
                {
                    choices.Add(new Choice(int.Parse(split[counter]), split[counter + 1]));
                    counter += 2;
                }
                TextManager.Ask(choices.ToArray(), text, player);
            }
        }

        public static void HandleBodyConfiguration(IgorrMessage message)
        {
            BodyConfigurationMessage bcm = (BodyConfigurationMessage)message;
            Player player = manager.GetPlayer(bcm.PlayerID);
            TryEquip(bcm.BaseBodyID, player);
            for(int x=0; x<bcm.AttackIDs.Length; x++)
            {
                TryEquip(bcm.AttackIDs[x], player);
            }
            for (int x = 0; x < bcm.UtilityIDs.Length; x++)
            {
                TryEquip(bcm.UtilityIDs[x], player);
            }
            for (int x = 0; x < bcm.MovementIDs.Length; x++)
            {
                TryEquip(bcm.MovementIDs[x],player);
            }
            for (int x = 0; x < bcm.ArmorIDs.Length; x++)
            {
                TryEquip(bcm.ArmorIDs[x], player);
            }
        }

        private static void TryEquip(int id, Player player)
        {
            GameObject obj = ModuleManager.SpawnByIdClient(null, id, -1, Point.Zero, "");
            PartContainer cont = obj as PartContainer;
            if (cont != null && cont.Part != null)
                player.Body.TryEquip(-1, cont.Part);
        }

        public static void HandleContainer(IgorrMessage m)
        {
            _enabled = false;
            ProtocolHelper.HandleContainer(m);
            _enabled = true;
        }

        public static void Exit()
        {
            if (receiveThread != null)
                receiveThread.Abort();
        }

        public static ObjectManager Manager
        {
            get { return manager; }
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

        public static bool Connected
        {
            get { return !(connection.ConnectionStatus == NetConnectionStatus.Disconnected || connection.ConnectionStatus == NetConnectionStatus.Disconnecting || connection.ConnectionStatus == NetConnectionStatus.None); }
        }

        public static ProtocolHelper ProtocolHelper
        {
            get { return _protocolHelper; }
        }
    }
}
