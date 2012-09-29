using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class ShadowMessage:IgorrMessage
    {       
        public bool shadows;

        public ShadowMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Shadow)
        { }

        public ShadowMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(shadows);
        }

        protected override void Decode()
        {
            shadows = _incoming.ReadBoolean();
        }
    }
}
