using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using IGORR.Protocol;
using IGORR.Protocol.Messages;

namespace IGORR.Server.Logic
{
    class MessageBoard:EventObject
    {
        public enum MessageMode
        {
            CurrentScore=0,
            CurrentDifference=1,
            LastWinner=2
        }

        string _text = "";
        public static string LastWinner = "";
        public static int Rounds = 0;
        int lastRounds = 0;
        float _countdown=10000;
        MessageMode _mode;

        public MessageBoard(IMap map, Rectangle targetRect, int id)
            : base(map, targetRect, id)
        {
            _objectType = 24;
            _mode = MessageMode.CurrentDifference;
        }

        public override void Update(float ms)
        {
            _countdown -= ms;
            if (_countdown <= 0)
            {
                _mode = (MessageMode)(((int)_mode + 1) % 3);
                switch (_mode)
                {
                    case MessageMode.CurrentDifference:
                        if (ScorePost.Progress == 0)
                            SetText("0");
                        else if (ScorePost.Progress > 0)
                            SetText("Blue + " + ScorePost.Progress.ToString());
                        else
                            SetText("Red + " + (-ScorePost.Progress).ToString());
                        break;
                    case MessageMode.CurrentScore: 
                        if (ScorePost.RedScore > ScorePost.BlueScore)
                            SetText("Red leads with " + ((ScorePost.RedScore - ScorePost.BlueScore)/100).ToString());
                        else 
                            SetText("Blue leads with " + ((ScorePost.BlueScore - ScorePost.RedScore)/100).ToString());
                        break;
                    case MessageMode.LastWinner: SetText(LastWinner); LastWinner = "Fight it out!"; break;
                }
                _countdown = 5000;
            }
            if (Rounds > lastRounds)
            {
                _mode = MessageMode.LastWinner;
                _text = LastWinner;
                _countdown = 10000;
            }
            lastRounds = Rounds;
            base.Update(ms);
        }

        private void SetText(string s)
        {
            _text = s;
            ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.NewMessage(MessageTypes.ObjectInfo);
            oim.objectID = _id;
            oim.info = s;
            oim.Encode();
            int chan = map.ObjectManager.Server.Channel;
            map.ObjectManager.Server.SetChannel(0);
            map.ObjectManager.Server.SendAllMapReliable(_map, oim, false);
            map.ObjectManager.Server.SetChannel(chan);
        }

        public override void SendInfo(Lidgren.Network.NetConnection connection)
        {
            ObjectInfoMessage oim = (ObjectInfoMessage)ProtocolHelper.GetContainerMessage(MessageTypes.ObjectInfo, connection);
            oim.objectID = _id;
            oim.info = _text;
            ProtocolHelper.SendContainer(oim, connection);
        }

        public override void Event(Player obj)
        {
        }
        
    }
}
