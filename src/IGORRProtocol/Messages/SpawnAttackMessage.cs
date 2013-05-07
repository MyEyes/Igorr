using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class SpawnAttackMessage : IgorrMessage
    {
        public Rectangle position;
        public Vector2 move;
        public int attackID;
        public int id;
        public string info;

        public SpawnAttackMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.SpawnAttack)
        {move = Vector2.Zero; }

        public SpawnAttackMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(position.X);
            _outgoing.Write(position.Y);
            _outgoing.Write(position.Width);
            _outgoing.Write(position.Height);
            _outgoing.Write(move.X);
            _outgoing.Write(move.Y);
            _outgoing.Write(id);
            _outgoing.Write(attackID);
            _outgoing.Write(info);
        }

        protected override void Decode()
        {
            position = new Rectangle(_incoming.ReadInt32(), _incoming.ReadInt32(), _incoming.ReadInt32(), _incoming.ReadInt32());
            move = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            id = _incoming.ReadInt32();
            attackID = _incoming.ReadInt32();
            info = _incoming.ReadString();
        }
    }
}
