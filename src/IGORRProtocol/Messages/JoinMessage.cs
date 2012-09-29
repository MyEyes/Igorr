using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class JoinMessage:IgorrMessage
    {
        public string Name;
        public string Password;

        public JoinMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Join)
        { }

        public JoinMessage(IgorrMessage m) : base(m) { }

        protected override void Decode()
        {
            Name = _incoming.ReadString();
            Password = _incoming.ReadString();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(Name);
            _outgoing.Write(Password);
        }
    }
}
