using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORR.Protocol.Messages
{
    public class ChatMessage:IgorrMessage
    {
        public string Text;
        public int objID=-1;
        public Vector2 pos;
        public float timeout;

        public ChatMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Chat) { }

        public ChatMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            Text = _incoming.ReadString();
            objID = _incoming.ReadInt32();
            pos = new Vector2(_incoming.ReadSingle(),_incoming.ReadSingle());
            timeout = _incoming.ReadSingle();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(Text);
            _outgoing.Write(objID);
            _outgoing.Write(pos.X);
            _outgoing.Write(pos.Y);
            _outgoing.Write(timeout);
        }
    }
}
