using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class ChangeMapMessage : IgorrMessage
    {
        public int mapid;

        public ChangeMapMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.ChangeMap)
        {

        }

        public ChangeMapMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            mapid = _incoming.ReadInt32();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(mapid);
        }
    }
}
