using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class ChangeTileMessage : IgorrMessage
    {
        public int x;
        public int y;
        public int layer;
        public int tileID;

        public ChangeTileMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.ChangeTile)
        {

        }

        public ChangeTileMessage(IgorrMessage incoming) : base(incoming) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(x);
            _outgoing.Write(y);
            _outgoing.Write(layer);
            _outgoing.Write(tileID);
        }

        protected override void Decode()
        {
            x = _incoming.ReadInt32();
            y = _incoming.ReadInt32();
            layer = _incoming.ReadInt32();
            tileID = _incoming.ReadInt32();
        }
    }
}
