using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using IGORR.Content;

namespace IGORR.Client
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager manager;
        IGORR.Client.Logic.Clock _clock;
        const int updateInterval=16;
        float excessTime = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            Settings.LoadSettings();
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Durchführung einer Initialisierung, die es benötigt, bevor es ausgeführt werden kann.
        /// Dort kann es erforderliche Dienste abfragen und nicht mit der Grafik
        /// verbundenen Content laden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu

            base.Initialize();
        }

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            ContentInterface.SetContent(Services, "Content", "Content.7z");
            ContentInterface.SetGraphicsDevice(GraphicsDevice);
            manager = new ScreenManager(Content, GraphicsDevice, this);

            manager.AddScreen(new UI.MainMenuScreen());

            //manager.AddScreen(new MainMenuScreen(this));
            Window.Title = "IGORR";

            _clock = new Logic.Clock();
            _clock.Stamp();
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Content hier zu laden
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Content hier
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            _clock.Tick();
            if (_clock.ElapsedStampTime.TotalMilliseconds+excessTime >= updateInterval)
            {
                _clock.Stamp();
                float time=(float)_clock.ElapsedStampTime.TotalMilliseconds+excessTime;
                while (time > updateInterval)
                {
                    gameTime = new GameTime(TimeSpan.Zero, new TimeSpan(0, 0, 0, 0, updateInterval));
                    manager.Update(gameTime);
                    if (this.IsActive)
                        MusicPlayer.Update((float)updateInterval);
                    time -= updateInterval;
                }
                excessTime = time;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            manager.Draw(gameTime);
            base.Draw(gameTime);
        }
    

        protected override void OnExiting(object sender, EventArgs args)
        {
            manager.Clear();
            WorldController.Leave();
            WorldController.Exit();
            base.OnExiting(sender, args);
        }
    }
}
