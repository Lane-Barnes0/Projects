using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asteroids.Objects;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids.Views
{
    internal class GamePlayView : GameState
    {
        private SpriteFont m_fontMenuSelect;
        private SpriteFont m_fontMenu;
        private SpriteFont m_title;
        private Texture2D m_square;
        private Player m_player;
        private List<Texture2D> m_playerAnimation;
        private SoundEffect m_shot;
        private Texture2D m_bulletTex;
        public override void loadContent(ContentManager contentManager)
        {
            m_square = contentManager.Load<Texture2D>("Images/square");
            m_bulletTex = contentManager.Load < Texture2D>("Images/square");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_title = contentManager.Load<SpriteFont>("Fonts/title");
            m_shot = contentManager.Load<SoundEffect>("Audio/shot");
            m_playerAnimation = new List<Texture2D> {
                contentManager.Load<Texture2D>("Images/Player")
            
            };

            newGame();
        }

        public override void previousScreen(GameState screen)
        {
           
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }
            return GameStateEnum.Game;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_square, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            if(m_player != null)
            {

                m_player.render(gameTime, m_spriteBatch);

            }
            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            if(m_player != null)
            {

                m_player.update(gameTime);
                loopScreen();


            }
        }

        private void loopScreen()
        {
            if (m_player.position.X < 0)
            {
                m_player.position.X = m_graphics.PreferredBackBufferWidth;

            }
            else if (m_player.position.X > m_graphics.PreferredBackBufferWidth)
            {
                m_player.position.X = 0;
            }

            if (m_player.position.Y < 0)
            {
                m_player.position.Y = m_graphics.PreferredBackBufferHeight;

            }
            else if (m_player.position.Y > m_graphics.PreferredBackBufferHeight)
            {
                m_player.position.Y = 0;
            }
        }
        private void newGame()
        {
            m_player = new Player(3, m_playerAnimation, new Vector2(0, 0), new Vector2(m_graphics.PreferredBackBufferWidth / 2, m_graphics.PreferredBackBufferHeight / 2), 0, 0.2, m_shot, m_bulletTex);

        }
    }
}
