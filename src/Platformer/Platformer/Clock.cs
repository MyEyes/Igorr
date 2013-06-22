using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace IGORR.Client.Logic
{
    internal class Clock
    {
        long _startTime;
        long _lastTime;
        long _lastStampTime;
        TimeSpan _totalTime;
        TimeSpan _elapsedTime;
        TimeSpan _elapsedSinceStamp;

        public static long TimeStamp
        {
            get { return Stopwatch.GetTimestamp(); }
        }

        public static long Frequency
        {
            get { return Stopwatch.Frequency; }
        }

        public Clock()
        {
            Reset();
        }

        public TimeSpan TotalTime { get { return _totalTime; } }
        public TimeSpan ElapsedTime { get { return _elapsedTime; } }
        public TimeSpan ElapsedStampTime { get { return _elapsedSinceStamp; } }

        public void Reset()
        {
            _startTime=TimeStamp;
            _lastTime=0;
            _totalTime=TimeSpan.Zero;
            _elapsedTime=TimeSpan.Zero;
        }

        public void Stamp()
        {
            _lastStampTime=TimeStamp;
        }

        public void Tick()
        {
            long currentTime = TimeStamp;
            if (_lastTime == 0)
                _lastTime = currentTime;
            _totalTime = TimestampToTimeSpan(currentTime - _startTime);
            _elapsedTime = TimestampToTimeSpan(currentTime - _lastTime);
            _elapsedSinceStamp = TimestampToTimeSpan(currentTime - _lastStampTime);
            _lastTime = currentTime;
        }

        public static TimeSpan TimestampToTimeSpan(long timestamp)
        {
            return TimeSpan.FromTicks(
                (timestamp * TimeSpan.TicksPerSecond) / Frequency);
        }

    }

    internal class ClockTrigger
    {
        Clock _clock;
        public delegate void Update(TimeSpan ts);
        Update _event;
        int _interval; //interval in ms
        Thread _thread;
        public bool Alive { get; set; }

        public ClockTrigger(Update eve, int interval)
        {
            _event=eve;
            _clock = new Clock();
            _clock.Stamp();
            _interval=interval;
            Alive = true;
        }

        public void Run()
        {
            _thread = new Thread(InternalRun);
            _thread.Start();
        }

        private void InternalRun()
        {
            while (Alive)
            {
                _clock.Tick();
                if (_clock.ElapsedStampTime.Milliseconds >= _interval)
                {
                    _clock.Stamp();
                    _event.Invoke(_clock.ElapsedStampTime);
                }
                else if(_clock.ElapsedStampTime.Milliseconds<_interval/2)
                {
                    Thread.Sleep((_interval - _clock.ElapsedStampTime.Milliseconds) / 2);
                }
            }
        }

        public void Stop()
        {
            _thread.Abort();              
        }
    }
}
