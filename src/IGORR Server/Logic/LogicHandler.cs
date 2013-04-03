using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using IGORR.Content;

namespace IGORR.Server.Logic
{
    static class LogicHandler
    {
        static Server _server;
        //static System.Timers.Timer _updateTimer;
        //static DateTime lastTime;
        static float timeRemainder;
        static SemaphoreSlim _sem;
        static ClockTrigger _clock;

        public static void SetUp(Server server)
        {
            _server = server;
            MapManager.LoadMaps(server);
            //lastTime = DateTime.Now;
            _sem = new SemaphoreSlim(1);
            _clock = new ClockTrigger(Update, 16);
            _clock.Run();
            /*
            _updateTimer = new System.Timers.Timer(16);
            _updateTimer.Elapsed += new ElapsedEventHandler(Update);
            _updateTimer.Start();
             */
        }

        public static void Update(TimeSpan elapsedTime)
        {
            _sem.Wait();
            //Figure out time since last time
            /*
            TimeSpan span = args.SignalTime - lastTime;
            span = (DateTime.Now - lastTime);
            lastTime = DateTime.Now;
             */
            float ms = (float)elapsedTime.TotalMilliseconds + timeRemainder;
            try
            {
                while (ms >= 16)
                {
                    //Don't send packets if this isn't the last frame we calculate this step
                    if (ms >= 32)
                        _server.Disable();
                    //Enable packet sending otherwise
                    else
                        _server.Enable();
                    IGORR.Protocol.ProtocolHelper.Update(16);
                    MapManager.Update(16);
                    ms -= 16;
                }
                //Store the remaining time since last call
                timeRemainder = ms;
                //Debug output
                /*
                TimeSpan timetaken = DateTime.Now - lastTime;
                if (span.Milliseconds > 50 || timetaken.TotalMilliseconds > 7)
                {
                    Console.WriteLine(span.Milliseconds.ToString() + " delay since last update");
                    Console.WriteLine("Current Update took: " + timetaken.TotalMilliseconds.ToString());
                }
                 */
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _sem.Release();
            }
        }
    }
}
