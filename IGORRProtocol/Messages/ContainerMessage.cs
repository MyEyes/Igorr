using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace IGORRProtocol.Messages
{
    class ContainerMessage:IgorrMessage
    {
        int _numMessages = 0;
        List<IgorrMessage> _messages;
        public NetOutgoingMessage outgoing;

        public ContainerMessage(NetOutgoingMessage outgoing, long timestamp) : base(outgoing, timestamp, MessageTypes.Container)
        { _messages = new List<IgorrMessage>(); this.outgoing = _outgoing; }

        public ContainerMessage(IgorrMessage incoming) : base(incoming) {}

        protected override void Decode()
        {
            _numMessages = _incoming.ReadInt32();
            _messages = new List<IgorrMessage>(); 
            for (int x = 0; x < _numMessages; x++)
            {
                try
                {
                    IgorrMessage m = new IgorrMessage(_incoming);
                    _messages.Add(Protocol.DecodeMessage(m));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public override void Encode()
        {
            base.Encode();
            _outgoing.Write(_messages.Count);
            for (int x=0; x < _messages.Count; x++)
                _messages[x].Encode();
        }

        public void AddMessage(IgorrMessage message)
        {
            _messages.Add(message);
        }

        public List<IgorrMessage> Messages
        {
            get { return _messages; }
        }
    }
}
