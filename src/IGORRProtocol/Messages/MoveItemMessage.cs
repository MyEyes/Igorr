using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public enum MoveTarget
    {
        None,
        Body,
        Inventory
    }

    public class MoveItemMessage : IgorrMessage
    {
        public int id;
        public int Quantity;
        public int Slot;
        public MoveTarget To;
        public MoveTarget From;

        public MoveItemMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.MoveItem) { }

        public MoveItemMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
            Quantity = _incoming.ReadInt32();
            Slot = _incoming.ReadInt32();
            To = (MoveTarget)_incoming.ReadByte();
            From = (MoveTarget)_incoming.ReadByte();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
            _outgoing.Write(Quantity);
            _outgoing.Write(Slot);
            _outgoing.Write((byte)To);
            _outgoing.Write((byte)From);
        }
    }
}
