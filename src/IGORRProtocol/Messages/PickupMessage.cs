using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class PickupMessage : IgorrMessage
    {
        public int id;

        public PickupMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Pickup) { }

        public PickupMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
        }
    }
}
