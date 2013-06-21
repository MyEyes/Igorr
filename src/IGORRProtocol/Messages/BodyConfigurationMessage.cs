using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORR.Protocol.Messages
{
    public class BodyConfigurationMessage : IgorrMessage
    {
        public int PlayerID;
        public int BaseBodyID;
        public int[] AttackIDs;
        public int[] ArmorIDs;
        public int[] UtilityIDs;
        public int[] MovementIDs;

        public BodyConfigurationMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.BodyConfiguration) { }

        public BodyConfigurationMessage(IgorrMessage incoming) : base(incoming) { }

        protected override void Decode()
        {
            PlayerID = _incoming.ReadInt32();
            BaseBodyID=_incoming.ReadInt32();
            AttackIDs=new int[_incoming.ReadInt32()];
            for(int x=0; x<AttackIDs.Length; x++)
                AttackIDs[x]=_incoming.ReadInt32();
            ArmorIDs = new int[_incoming.ReadInt32()];
            for (int x = 0; x < ArmorIDs.Length; x++)
                ArmorIDs[x] = _incoming.ReadInt32();
            UtilityIDs = new int[_incoming.ReadInt32()];
            for (int x = 0; x < UtilityIDs.Length; x++)
                UtilityIDs[x] = _incoming.ReadInt32();
            MovementIDs = new int[_incoming.ReadInt32()];
            for (int x = 0; x < MovementIDs.Length; x++)
                MovementIDs[x] = _incoming.ReadInt32();
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(PlayerID);
            _outgoing.Write(BaseBodyID);
            _outgoing.Write(AttackIDs.Length);
            for(int x=0; x<AttackIDs.Length; x++)
                _outgoing.Write(AttackIDs[x]);
            
            _outgoing.Write(ArmorIDs.Length);
            for(int x=0; x<ArmorIDs.Length; x++)
                _outgoing.Write(ArmorIDs[x]);
            
            _outgoing.Write(UtilityIDs.Length);
            for(int x=0; x<UtilityIDs.Length; x++)
                _outgoing.Write(UtilityIDs[x]);
            
            _outgoing.Write(MovementIDs.Length);
            for(int x=0; x<MovementIDs.Length; x++)
                _outgoing.Write(MovementIDs[x]);
        }
    }
}
