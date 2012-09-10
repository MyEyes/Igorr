using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class SetGlowMessage : IgorrMessage
    {
        public int id;
        public Vector2 Position;
        public float radius;
        public bool shadows=false;

        public SetGlowMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.SetGlow)
        {

        }

        public SetGlowMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
            Position = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            radius = _incoming.ReadFloat();
            shadows = _incoming.ReadBoolean();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
            _outgoing.Write(Position.X);
            _outgoing.Write(Position.Y);
            _outgoing.Write(radius);
            _outgoing.Write(shadows);
        }
    }
}
