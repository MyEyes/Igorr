using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORR.Protocol.Messages
{
    public class DoEffectMessage : IgorrMessage
    {
        public Vector2 Dir;
        public int EffectID;
        public Point Position;
        public string Info;

        public DoEffectMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.DoEffect)
        { }

        public DoEffectMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(Dir.X);
            _outgoing.Write(Dir.Y);
            _outgoing.Write(EffectID);
            _outgoing.Write(Position.X);
            _outgoing.Write(Position.Y);
            _outgoing.Write(Info);
        }

        protected override void Decode()
        {
            Dir = new Vector2(_incoming.ReadFloat(),_incoming.ReadFloat());
            EffectID = _incoming.ReadInt32();
            Position = new Point(_incoming.ReadInt32(), _incoming.ReadInt32());
            Info = _incoming.ReadString();
        }
    }
}
