using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class ExpMessage : IgorrMessage
    {
        public int exp;
        public Vector2 startPos;

        public ExpMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.ExpMessage) { }

        public ExpMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            exp = _incoming.ReadInt32();
            startPos.X = _incoming.ReadFloat();
            startPos.Y = _incoming.ReadFloat();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(exp);
            _outgoing.Write(startPos.X);
            _outgoing.Write(startPos.Y);
        }
    }
}
