using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class PlayerInfoMessage:IgorrMessage
    {
        public string Text;
        public int playerID;

        public PlayerInfoMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.PlayerInfoMessage) { }

        public PlayerInfoMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            playerID = _incoming.ReadInt32();
            Text = _incoming.ReadString();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(playerID);
            _outgoing.Write(Text);
        }
    }
}
