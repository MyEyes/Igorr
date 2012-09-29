using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class SetPlayerStatusMessage:IgorrMessage
    {       
        public int maxHP;
        public int currentHP;
        public int playerID;
        public int Level;
        public int Exp;
        public int nextLevelExp;
        public int lastLevelExp;

        public SetPlayerStatusMessage(NetOutgoingMessage outgoing, long timestamp)
            : base(outgoing, timestamp, MessageTypes.SetHP)
        { }

        public SetPlayerStatusMessage(IgorrMessage m) : base(m) { }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(maxHP);
            _outgoing.Write(currentHP);
            _outgoing.Write(playerID);
            _outgoing.Write(Level);
            _outgoing.Write(Exp);
            _outgoing.Write(nextLevelExp);
            _outgoing.Write(lastLevelExp);
        }

        protected override void Decode()
        {
            maxHP = _incoming.ReadInt32();
            currentHP = _incoming.ReadInt32();
            playerID = _incoming.ReadInt32();
            Level = _incoming.ReadInt32();
            Exp = _incoming.ReadInt32();
            nextLevelExp = _incoming.ReadInt32();
            lastLevelExp = _incoming.ReadInt32();
        }
    }
}
