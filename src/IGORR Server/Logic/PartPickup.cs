using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORRProtocol;
using IGORRProtocol.Messages;

namespace IGORR_Server.Logic
{
    public class PartPickup:EventObject
    {
        BodyPart _bodyPart;
        bool _respawn = true;
        public bool Respawn { get { return _respawn; } }

        public PartPickup(BodyPart part, Map map, Rectangle rect, int id, bool respawn)
            : base(map, rect, id)
        {
            _bodyPart = part;
            _objectType = _bodyPart.GetID();
            _name = _bodyPart.GetName();
            _respawn = respawn;
        }

        public override void Update(float ms)
        {
            /*
            counter += ms/1000;
            if (counter < 0.75)
            {
                Move(-12 * Vector2.UnitY * ms / 1000);
            }
            else if (counter < 1.5)
            {
                Move(12 * Vector2.UnitY * ms / 1000);
            }
            else
                counter -= 1.5f;
             */
        }

        public override void Event(Player plr)
        {
            if (!plr.GivePart(_bodyPart))
                return;
            _parent.RemoveChild();
            if (_respawn)
                _map.TimeSpawn(_bodyPart.GetID(), _bodyPart.GetSpawnTime());
            PickupMessage pum = (PickupMessage)Protocol.NewMessage(MessageTypes.Pickup);
            pum.id = _bodyPart.GetID();
            pum.Encode();
            _map.ObjectManager.Server.SendClient(plr, pum);
            Console.WriteLine("Player " + plr.ID.ToString() + " picked up " + _bodyPart.GetName());
            _map.ObjectManager.Remove(this);
        }
    }
}
