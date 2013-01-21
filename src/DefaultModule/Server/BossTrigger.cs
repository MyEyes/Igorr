using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace IGORR.Server.Logic
{
    class BossBlobTrigger:EventObject
    {
        float countdown = 2;
        bool active = false;
        AI.BossBlob boss = null;
        bool wasAlive = false;
        bool done = false;
        bool restart = true;

        public BossBlobTrigger(IMap map, Rectangle position, int id)
            : base(map, position, id)
        {
            _objectType = 'f' - 'a';
        }

        public override void Update(float ms)
        {
            try
            {
                if (active && !done)
                {
                    countdown -= ms / 1000f;
                    if (countdown < 0 && boss == null)
                    {
                        boss = new AI.BossBlob(_map, "boss", new Rectangle(1172, 260, 16, 15), map.ObjectManager.getID());
                        _map.ObjectManager.Add(boss);
                        wasAlive = true;

                        _map.ChangeTile(1, this.MidPosition, 12);
                    }
                    if (boss!=null && boss.reset)
                    {
                        _map.ChangeTile(1, this.MidPosition, -1);
                        restart = true;
                    }
                    if (restart && boss!=null && !boss.reset)
                    {
                        restart = false;
                        _map.ChangeTile(1, this.MidPosition, 12);
                    }
                    if (wasAlive && (boss==null ||  boss.defeated || boss.HP<0) )
                    {
                        boss = null;
                        done = true;
                        _map.ChangeTile(1, this.MidPosition, -1);
                        _parent.RemoveChild();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override void Event(Player obj)
        {
            if (obj != null)
            {
                if (!active)
                {
                    IGORR.Protocol.Messages.PlayMessage pm = (IGORR.Protocol.Messages.PlayMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.Play);
                    pm.SongName = "";
                    pm.Encode();
                    server.SendAllMap(_map, pm, true);
                }
                active = true;
                countdown = 2;
            }
        }
    }

}
