using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class KnockbackMessage:IgorrMessage
    {
        public int id;
        public Vector2 Move;

        public KnockbackMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Knockback)
        {

        }

        public KnockbackMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
            Move = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
            _outgoing.Write(Move.X);
            _outgoing.Write(Move.Y);
        }
    
    }
}
