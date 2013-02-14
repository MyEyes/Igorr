using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class SpawnMessage : IgorrMessage
    {
        public Rectangle position;
        public Vector2 move;
        public int objectType;
        public int id;
        public string Info = "";

        public SpawnMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Spawn)
        { move = Vector2.Zero; }

        public SpawnMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(position.X);
            _outgoing.Write(position.Y);
            _outgoing.Write(position.Width);
            _outgoing.Write(position.Height);
            _outgoing.Write(move.X);
            _outgoing.Write(move.Y);
            _outgoing.Write(objectType);
            _outgoing.Write(id);
            _outgoing.Write(Info);
        }

        protected override void Decode()
        {
            position = new Rectangle(_incoming.ReadInt32(), _incoming.ReadInt32(), _incoming.ReadInt32(), _incoming.ReadInt32());
            move = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            objectType = _incoming.ReadInt32();
            id = _incoming.ReadInt32();
            Info = _incoming.ReadString();
        }
    }
}
