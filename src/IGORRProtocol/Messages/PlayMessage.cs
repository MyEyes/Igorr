using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class PlayMessage : IgorrMessage
    {
        public string SongName;
        public bool Loop;
        public bool Queue;

        public PlayMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Play) { }

        public PlayMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            SongName = _incoming.ReadString();
            Loop = _incoming.ReadBoolean();
            Queue = _incoming.ReadBoolean();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(SongName);
            _outgoing.Write(Loop);
            _outgoing.Write(Queue);
        }
    }
}
