using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class KillMessage : IgorrMessage
    {
        public int killerID;
        public int deadID;

        public KillMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Kill)
        { }

        public KillMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(killerID);
            _outgoing.Write(deadID);
        }

        protected override void Decode()
        {
            killerID = _incoming.ReadInt32();
            deadID = _incoming.ReadInt32();
        }
    }
}
