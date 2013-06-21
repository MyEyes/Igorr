using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class PickupMessage : IgorrMessage
    {
        public int PlayerID;
        public int id;
        public bool autoEquip = true;

        public PickupMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Pickup) { }

        public PickupMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            PlayerID = _incoming.ReadInt32();
            id = _incoming.ReadInt32();
            autoEquip = _incoming.ReadBoolean();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(PlayerID);
            _outgoing.Write(id);
            _outgoing.Write(autoEquip);
        }
    }
}
