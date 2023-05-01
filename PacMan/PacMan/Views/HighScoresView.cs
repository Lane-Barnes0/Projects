using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace PacMan.Views
{
    public class HighScoresView : GameState
    {

        private SpriteFont m_font;
        private Texture2D m_square;
        private bool loading = false;
        private List<int> highscores;



        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            m_square = contentManager.Load<Texture2D>("Images/square");
            loadScores();
            highscores = new List<int>();
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Highscores;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_square, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);

            m_spriteBatch.DrawString(m_font, "HIGH SCORES", new Vector2(700, 110), Color.Yellow);

            if (highscores.Count > 0)
            {
                for (int i = 0; i < (highscores.Count > 4 ? 5 : highscores.Count); i++)
                {
                    m_spriteBatch.DrawString(m_font, highscores[i].ToString(), new Vector2(700, 210 + i * 75), Color.Yellow);
                }


            }

            m_spriteBatch.End();

        }

        public override void update(GameTime gameTime)
        {
            loadScores();

            if (m_loadedState != null)
            {
                if (highscores.Count < m_loadedState.Score.Count)
                {
                    highscores = m_loadedState.Score;
                    highscores.Sort();
                    highscores.Reverse();
                }


            }
        }

        private void loadScores()
        {
            lock (this)
            {
                if (!loading)
                {
                    loading = true;
                    finalizeLoadAsync();
                }
            }
        }
        private Scoring m_loadedState = null;

        private async void finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("PacManHighscores.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("PacManHighscores.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(Scoring));
                                    m_loadedState = (Scoring)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                loading = false;
            });
        }

        public override void previousScreen(GameStateEnum screen)
        {
            loadScores();
        }
    }


}