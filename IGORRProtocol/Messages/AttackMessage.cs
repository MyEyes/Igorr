using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace IGORRProtocol.Messages
{
    public class AttackMessage : IgorrMessage
    {
        public int attackID;
        public int attackerID;
        public Vector2 attackDir;
        public int attackInfo;

        public AttackMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.Attack)
        { }

        public AttackMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(attackID);
            _outgoing.Write(attackerID);
            _outgoing.Write(attackDir.X);
            _outgoing.Write(attackDir.Y);
            _outgoing.Write(attackInfo);
        }

        protected override void Decode()
        {
            attackID = _incoming.ReadInt32();
            attackerID = _incoming.ReadInt32();
            attackDir = new Vector2(_incoming.ReadFloat(), _incoming.ReadFloat());
            attackInfo = _incoming.ReadInt32();
        }
    }
}
