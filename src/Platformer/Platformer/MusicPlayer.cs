using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace IGORR.Game
{

    class SongQueueItem
    {
        public Song song;
        public bool loop;
    }

    enum MusicEffect
    {
        FadeIn,
        FadeOut,
        None
    }

    static class MusicPlayer
    {
        static Dictionary<string,Song> _songs;
        static List<SongQueueItem> _queue;
        static ContentManager _content;
        static MusicEffect _effect = MusicEffect.None;
        static string currentName = "";

        static float maxVolume = 0f;
        static Mutex _mutex;
        static bool Restarted = false;

        static MusicPlayer()
        {
            _songs = new Dictionary<string,Song>();
            _queue = new List<SongQueueItem>();
            _mutex = new Mutex();
            //MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(SongStopped);
            MediaPlayer.Volume = maxVolume;
        }

        public static void SetContent(ContentManager content)
        {
            _content = content;
            PlaySong("Level01", true, false);
        }

        public static void PlaySong(string name, bool loop, bool queue)
        {
            return;
            _mutex.WaitOne();
            if (string.IsNullOrWhiteSpace(name))
            {
                _effect = MusicEffect.FadeOut;
                _mutex.ReleaseMutex();
                return;
            }
            else if (name == currentName && !queue)
            {
                _mutex.ReleaseMutex();
                return;
            }
            lock (_queue)
            {
                Song song = null;
                if (_songs.ContainsKey(name))
                {
                    song = _songs[name];
                }
                else
                {
                    try
                    {
                        song = _content.Load<Song>(name);
                        if (song != null)
                            _songs.Add(name, song);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                if (queue)
                {
                    SongQueueItem sqi = new SongQueueItem();
                    sqi.song = song;
                    sqi.loop = loop;
                    _queue.Add(sqi);
                }
                else
                {
                    
                    MediaPlayer.Volume = 0;
                    _effect = MusicEffect.FadeIn;
                    MediaPlayer.IsRepeating = loop;
                    MediaPlayer.Stop();
                    MediaPlayer.Play(song);
                    Restarted = true;
                    currentName = name;
                }
            }
            _mutex.ReleaseMutex();
        }

        public static void Update(float ms)
        {
            return;
            _mutex.WaitOne();
            lock (_queue)
            {
                switch (_effect)
                {
                    case MusicEffect.FadeIn: MediaPlayer.Volume += ms / 2000f * maxVolume; if (MediaPlayer.Volume >= maxVolume) { MediaPlayer.Volume = maxVolume; _effect = MusicEffect.None; } break;
                    case MusicEffect.FadeOut: MediaPlayer.Volume -= ms / 2000f * maxVolume; if (MediaPlayer.Volume <= 0) { MediaPlayer.Volume = 0; MediaPlayer.Stop(); _effect = MusicEffect.None; } break;
                }
                FrameworkDispatcher.Update();
                if (!Restarted && MediaPlayer.State == MediaState.Stopped)
                {
                    if (_queue.Count > 0)
                    {
                        MediaPlayer.IsRepeating = _queue[0].loop;
                        MediaPlayer.Stop();
                        MediaPlayer.Play(_queue[0].song);
                        currentName = _queue[0].song.Name;
                        if (MediaPlayer.Volume == 0)
                        {
                            _effect = MusicEffect.FadeIn;
                        }
                        _queue.RemoveAt(0);
                        Restarted = true;
                    }
                }
                else if (Restarted && MediaPlayer.State == MediaState.Playing)
                {
                    Restarted = false;
                }

            }
            _mutex.ReleaseMutex();
        }

        static void SongStopped(object state, EventArgs args)
        {
            return;
            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (_queue.Count > 0)
                {
                    MediaPlayer.IsRepeating = _queue[0].loop;
                    MediaPlayer.Play(_queue[0].song);
                    currentName = _queue[0].song.Name;
                    if (MediaPlayer.Volume == 0)
                    {
                        _effect = MusicEffect.FadeIn;
                    }
                    _queue.RemoveAt(0);
                }
            }
        }
    }
}
