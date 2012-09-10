using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class ExpMessage : IgorrMessage
    {
        public int exp;

        public ExpMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.ExpMessage) { }

        public ExpMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            exp = _incoming.ReadInt32();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(exp);
        }
    }
}
