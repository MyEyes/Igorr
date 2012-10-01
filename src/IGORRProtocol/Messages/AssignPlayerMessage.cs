using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class AssignPlayerMessage : IgorrMessage
    {
        public int objectID;
        public AssignPlayerMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.AssignPlayer)
        {

        }

        public AssignPlayerMessage(IgorrMessage incoming) : base(incoming) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(objectID);
        }

        protected override void Decode()
        {
            objectID = _incoming.ReadInt32();
        }
    }
}
