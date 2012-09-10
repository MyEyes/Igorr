using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    public class SpawnAttackMessage : IgorrMessage
    {
        public Vector2 position;
        public Vector2 move;
        public int id;

        public SpawnAttackMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.SpawnAttack)
        {move = Vector2.Zero; }

        public SpawnAttackMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(position.X);
            _outgoing.Write(position.Y);
            _outgoing.Write(move.X);
            _outgoing.Write(move.Y);
            _outgoing.Write(id);
        }

        protected override void Decode()
        {
            position = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            move = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            id = _incoming.ReadInt32();
        }
    }
}
