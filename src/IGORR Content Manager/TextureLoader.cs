using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace IGORR.Content
{
    static class TextureLoader
    {
        static Texture2D _default;

        public static void LoadDefault(PackedContentManager content)
        {
            _default = content.Load<Texture2D>("gfx/default");
        }

        public static Texture2D GetTexture(PackedContentManager content, GraphicsDevice device, string name)
        {
            try
            {
                if (File.Exists(content.RootDirectory+"/gfx/"+name))
                {
                    return Texture2D.FromStream(device, File.OpenRead(content.RootDirectory + "/gfx/" + name));
                }
                else
                    return content.Load<Texture2D>("gfx/"+name);
            }
            catch (ContentLoadException cle)
            {
                Console.WriteLine("Error loading texture: " + "gfx/"+name);
                return _default;
            }
        }

        public static Texture2D Default { get { return _default; } }
    }
}
