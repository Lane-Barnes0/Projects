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
        Scoring scoringSystem;


        public override void loadContent(ContentManager contentManager)
        {
            scoringSystem = new Scoring();
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            m_square = contentManager.Load<Texture2D>("Images/square");
            scoringSystem.loadScores(loading);
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
            scoringSystem.loadScores(loading);

            if (scoringSystem.getLoadedState() != null)
            {
                if (highscores.Count < scoringSystem.getLoadedState().Score.Count)
                {
                    highscores = scoringSystem.getLoadedState().Score;
                    highscores.Sort();
                    highscores.Reverse();
                }


            }
        }

       

        public override void previousScreen(GameStateEnum screen)
        {
            scoringSystem.loadScores(loading);
        }
    }


}