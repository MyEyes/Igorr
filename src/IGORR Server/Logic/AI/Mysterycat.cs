using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IGORR_Server.Logic.AI
{
    class Mysterycat : NPC
    {
        float flickering;
        bool more = false;

        float speed = 15;
        float maxflicker=5;
        bool _changedPosition = false;
        float baseRadius = 0;
        public Mysterycat(Map map, Rectangle spawnRect, int id)
            : base(map, spawnRect, id)
        {
            _invincible = true;
        }

        public Mysterycat(Map map, string file, Rectangle spawnRect, int id)
            : base(map, file, spawnRect, id)
        {
            _invincible = true;
        }

        public override void Update(Map map, float seconds)
        {
            if (flickering < -maxflicker)
                more = true;
            else if (flickering > maxflicker)
                more = false;
            flickering += more ? speed * seconds : -speed * seconds;
            if (map.GetTrigger("MoveCat1"))
            {
                IGORR.Protocol.Messages.SetGlowMessage sgm = (IGORR.Protocol.Messages.SetGlowMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.SetGlow);
                sgm.id = _id;
                sgm.Position = MidPosition - Vector2.UnitY * 20;
                sgm.radius = baseRadius + flickering;
                sgm.Encode();
                if (baseRadius < 55)
                    baseRadius += 110 * seconds;
                //18,11
                if (map.GetTrigger("MoveCat2") && !_changedPosition)
                {
                    this.Position = new Vector2(18 * 16, 11 * 16);
                    _changedPosition = true;
                }
                if (map.GetTrigger("MoveCat3"))
                {
                    baseRadius -= 220 * seconds;
                    sgm = (IGORR.Protocol.Messages.SetGlowMessage)IGORR.Protocol.ProtocolHelper.NewMessage(IGORR.Protocol.MessageTypes.SetGlow);
                    sgm.id = _id;
                    sgm.Position = MidPosition - Vector2.UnitY * 20;
                    sgm.radius = baseRadius;
                    if (baseRadius < -1)
                    {
                        map.ObjectManager.Remove(this);
                        sgm.radius = -1;
                    }
                    sgm.Encode();
                }
                map.ObjectManager.Server.SendAllMap(map, sgm,false);
            }
            base.Update(map,seconds);
        }
    }
}
