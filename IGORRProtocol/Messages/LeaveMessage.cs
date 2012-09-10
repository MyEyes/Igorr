using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class LeaveMessage:IgorrMessage
    {
        public LeaveMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Leave) { }
        public LeaveMessage(IgorrMessage m) : base(m) { }
    }
}
