using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public abstract class GameState
    {

        protected GraphicsDeviceManager m_graphics;
        protected SpriteBatch m_spriteBatch;

        public void initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics)
        {
            m_graphics = graphics;
            m_spriteBatch = new SpriteBatch(graphicsDevice);
        }
        public abstract void loadContent(ContentManager contentManager);

        public abstract GameStateEnum processInput(GameTime gameTime);

        public abstract void update(GameTime gameTime);
        public abstract void render(GameTime gameTime);

        public abstract void previousScreen(GameStateEnum screen);
    }
}
