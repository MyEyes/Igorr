using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class ObjectInfoMessage:IgorrMessage
    {       
        public string info;
        public int objectID;

        public ObjectInfoMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.ObjectInfo)
        { }

        public ObjectInfoMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(info);
            _outgoing.Write(objectID);
        }

        protected override void Decode()
        {
            info = _incoming.ReadString();
            objectID=_incoming.ReadInt32();
        }
    }
}
