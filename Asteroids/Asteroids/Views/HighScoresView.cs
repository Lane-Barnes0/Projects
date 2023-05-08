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

namespace Asteroids.Views
{
    public class HighScoresView : GameState
    {


        private SpriteFont m_fontMenuSelect;
        private SpriteFont m_fontMenu;

        private SpriteFont m_title;
        private Texture2D m_square;

        public override void loadContent(ContentManager contentManager)
        {
            m_square = contentManager.Load<Texture2D>("Images/square");

            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_title = contentManager.Load<SpriteFont>("Fonts/title");
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

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            
        }

       

        public override void previousScreen(GameState screen)
        {
            
        }
    }


}