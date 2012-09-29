using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class DeSpawnMessage:IgorrMessage
    {
        public int id;

        public DeSpawnMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.DeSpawn) { }

        public DeSpawnMessage(IgorrMessage incoming) : base(incoming) { }

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
