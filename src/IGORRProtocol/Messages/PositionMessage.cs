using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class PositionMessage : IgorrMessage
    {
        public int id;
        public Vector2 Position;
        public Vector3 Move;

        public PositionMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Position)
        {

        }

        public PositionMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            id = _incoming.ReadInt32();
            Position = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            Move = new Vector3(_incoming.ReadFloat(), _incoming.ReadFloat(),_incoming.ReadFloat());
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(id);
            _outgoing.Write(Position.X);
            _outgoing.Write(Position.Y);
            _outgoing.Write(Move.X);
            _outgoing.Write(Move.Y);
            _outgoing.Write(Move.Z);
        }
    }
}
