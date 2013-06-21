using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class StunMessage : IgorrMessage
    {
        public int id;
        public float stunTime;

        public StunMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Stun)
        {

        }

        public StunMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
            stunTime = _incoming.ReadFloat();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
            _outgoing.Write(stunTime);
        }
    }
}
