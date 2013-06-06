using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Client.Logic
{
    class PartPickup:PartContainer
    {
        Body.BodyPart _bodyPart;
        float counter = 0;
        Vector2 offset;
        bool up = false;

        public PartPickup(Body.BodyPart part, IMap map, Rectangle rect, int id)
            : base(map, part, rect, id)
        {
            _bodyPart = part;
            offset.Y = (float)(-8 * (0.5f+0.5f*Math.Sin(_rect.X / 32.0f)));
            Move(offset);
        }

        public override void Update(float ms)
        {
            counter += ms/1000;
            if (!up && offset.Y>-8)
            {
                offset += -12 * Vector2.UnitY * ms / 1000;
                Move(-12 * Vector2.UnitY * ms / 1000);
                if (offset.Y <= -8)
                    up = true;
            }
            if (up && offset.Y < 0)
            {
                offset += 12 * Vector2.UnitY * ms / 1000;
                Move(12 * Vector2.UnitY * ms / 1000);
                if (offset.Y >= 0)
                    up = false;
            }
        }

        public override void Event(Player plr)
        {
            //plr.GivePart(_bodyPart);
            //TextManager.AddText(plr.MidPosition, "Acquired "+_bodyPart.GetName(), 4500, Color.Green, Color.Transparent);
            //_parent.RemoveChild();
            //_map.TimeSpawn(_bodyPart.GetID(), _bodyPart.GetSpawnTime());
            //WorldController.SendText("Picked up " + _bodyPart.GetName()+"!");
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_bodyPart.Texture, _rect, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.45f);
        }
    }
}
