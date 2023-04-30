using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacMan.Input;

namespace PacMan
{
    internal class CreditsView : GameState
    {
        private SpriteFont m_font;
        
        
        private const string MESSAGE = "Made by \n";
        private const string MESSAGE1 = "Lane Barnes";
        private Texture2D m_square;


        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
            m_square = contentManager.Load<Texture2D>("Images/square");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Credits;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_square, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y - 200), Color.Yellow);

            stringSize = m_font.MeasureString(MESSAGE1);
            m_spriteBatch.DrawString(m_font, MESSAGE1,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 2 - stringSize.Y - 150), Color.Yellow);

            
            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }

        public override void previousScreen(GameStateEnum screen)
        {
            
        }
    }
}

