using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IGORR.Client.Logic.Body;

namespace IGORR.Client.Logic
{
    public class PartContainer:EventObject
    {
        BodyPart _part;
        public PartContainer(IMap map, BodyPart part, Rectangle rect, int id)
            : base(map, part.Texture, rect, id)
        {
            _part = part;
        }

        public BodyPart Part
        {
            get { return _part; }
        }
    }
}
