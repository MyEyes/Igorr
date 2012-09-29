using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class ChatMessage:IgorrMessage
    {
        public string Text;

        public ChatMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Chat) { }

        public ChatMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            Text = _incoming.ReadString();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(Text);
        }
    }
}
