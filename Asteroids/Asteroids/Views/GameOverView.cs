using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Views
{
    internal class GameOverView : GameState
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

        public override void previousScreen(GameState screen)
        {
            
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            return GameStateEnum.GameOver;
        }

        public override void render(GameTime gameTime)
        {
            
        }

        public override void update(GameTime gameTime)
        {
            
        }
    }
}
