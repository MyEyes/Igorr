using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Lidgren.Network;

namespace IGORRProtocol
{
    public class IgorrMessage
    {
        protected NetIncomingMessage _incoming;
        protected NetOutgoingMessage _outgoing;

        bool _inc;
        bool _encoded;
        bool _decoded;

        long _timeStamp = 0;
        MessageTypes _type;
        public int clientID = -1;
        public NetConnection SenderConnection;

        public IgorrMessage(NetOutgoingMessage outgoing, long timestamp, MessageTypes type)
        {
            _outgoing = outgoing;
            _inc = false;
            _type = type;
            _encoded = false;
            _timeStamp = timestamp;
        }

        public IgorrMessage(NetIncomingMessage incoming)
        {
            _inc = true;
            _incoming = incoming;
            try
            {
                _type = (MessageTypes)_incoming.ReadInt16();
                _timeStamp = _incoming.ReadInt64();
                SenderConnection = incoming.SenderConnection;
                Decode();
            }
            catch (NetException ne)
            {
                Console.WriteLine("Error occured when decoding " + _type.ToString() + " message:" + ne.Message);
            }
        }

        public IgorrMessage(IgorrMessage incoming)
        {
            _inc = true;
            _incoming = incoming._incoming;
            _type = incoming._type;
            _timeStamp = incoming._timeStamp;
            /*
            //If we don't actually get a message of the correct type but a container, we need to read the type ourselves
            if (incoming is Messages.ContainerMessage)
            {
                _type = (MessageTypes)_incoming.ReadInt16();
                _timeStamp = _incoming.ReadInt64();
            }
             */
            clientID = incoming.clientID;
            SenderConnection = incoming.SenderConnection;
            Decode();
        }

        protected virtual void Decode()
        {
        }

        public virtual void Encode()
        {
            _outgoing.Write((short)_type);
            _outgoing.Write((long)_timeStamp);
            _encoded = true;
        }

        public NetOutgoingMessage GetMessage()
        {
            return _outgoing;
        }

        public MessageTypes MessageType
        {
            get { return _type; }
        }

        public MessageTypes ReReadType
        {
            get
            {
                FieldInfo members = _outgoing.GetType().GetField("m_data", BindingFlags.NonPublic |
                         BindingFlags.Instance);
                byte[] bytes = (byte[])members.GetValue(_outgoing);
                return (MessageTypes)bytes[0] + bytes[1] * 256;
            }
        }

        public bool Encoded
        {
            get { return _encoded; }
        }

        public long TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}
