using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.IO;

namespace IGORR.Content
{
    public static class ContentInterface
    {
        static PackedContentManager _content;
        static GraphicsDevice _device;
        static Mutex _mutex = new Mutex();
        static bool _contentSet = false;

        public static void SetContent(IServiceProvider serviceProvider, string ContentPath, string PackFile)
        {
            if (serviceProvider == null)
                return;
            _content = new PackedContentManager(serviceProvider, PackFile, ContentPath, Nomad.Archive.SevenZip.KnownSevenZipFormat.SevenZip);
            TextureLoader.LoadDefault(_content);
            _contentSet = true;
        }

        public static void SetGraphicsDevice(GraphicsDevice device)
        {
            _device = device;
        }

        public static Texture2D LoadTexture(string file)
        {
            if (!_contentSet)
                return null;
            _mutex.WaitOne();
            Texture2D tex = TextureLoader.GetTexture(_content, _device, file);
            _mutex.ReleaseMutex();
            return tex;
        }

        public static string LoadFile(string file)
        {
            if (!_contentSet)
                return "";
            _mutex.WaitOne();
            try
            {
                if (File.Exists(file))
                {
                    string data = File.OpenText(file).ReadToEnd();
                    return data;
                }
                string data2 = _content.Load<string>(file);
                return data2;
            }
            catch (Microsoft.Xna.Framework.Content.ContentLoadException cle)
            {
                return "";
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public static CharTemplate LoadCharacter(string file)
        {
            return null;
        }

        public static EffectTemplate LoadEffect(string file)
        {
            return null;
        }

        public static Effect LoadShader(string file)
        {
            if (!_contentSet)
                return null;
            _mutex.WaitOne();
            try
            {
                return _content.Load<Effect>("shaders/" + file);
            }
            finally { _mutex.ReleaseMutex(); }
        }

        public static Song LoadSong(string file)
        {
            if (!_contentSet)
                return null;
            _mutex.WaitOne();
            try
            {
                return _content.Load<Song>("sound/" + file);
            }
            finally { _mutex.ReleaseMutex(); }
        }

        public static SpriteFont LoadFont(string file)
        {
            if (!_contentSet)
                return null;
            _mutex.WaitOne();
            try
            {
                return _content.Load<SpriteFont>("fonts/" + file);
            }
            finally { _mutex.ReleaseMutex(); }
        }

        public static bool ContentSet
        {
            get { return _contentSet; }
        }

        public static Texture2D DefaultTexture { get { return TextureLoader.Default; } }
    }
}
