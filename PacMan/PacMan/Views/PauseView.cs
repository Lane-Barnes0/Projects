using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan.Views
{
    internal class PauseView : GameState
    {



        private Texture2D m_playerTex;
        private SpriteFont m_fontMenuSelect;
        private SpriteFont m_fontMenu;
        private bool m_waitForKeyRelease;
        private Texture2D m_square;
        private enum MenuState
        {

            Resume,
            Exit
        }
        private MenuState m_currentSelection;
        private KeyboardState m_previousKeyboard;



        public override void loadContent(ContentManager contentManager)
        {
            m_square = contentManager.Load<Texture2D>("Images/square");
            m_playerTex = contentManager.Load<Texture2D>("Images/square");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (!m_waitForKeyRelease)
            {
                // Arrow keys to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {

                    if (m_currentSelection != MenuState.Exit)
                    {
                        m_currentSelection = m_currentSelection + 1;
                    }

                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (m_currentSelection != MenuState.Resume)
                    {
                        m_currentSelection = m_currentSelection - 1;
                    }

                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Resume)
                {

                    return GameStateEnum.Game;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Exit)
                {
                    return GameStateEnum.MainMenu;
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.Pause;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_square, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            float bottom = drawMenuItem(m_currentSelection == MenuState.Resume ? m_fontMenuSelect : m_fontMenu,
                "Resume",
                200,
                m_currentSelection == MenuState.Resume ? Color.Yellow : Color.Blue);


            drawMenuItem(m_currentSelection == MenuState.Exit ? m_fontMenuSelect : m_fontMenu, "Main Menu", bottom, m_currentSelection == MenuState.Exit ? Color.Yellow : Color.Blue);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {

        }


        private float drawMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;

        }

        public override void previousScreen(GameStateEnum screen)
        {

            m_currentSelection = MenuState.Resume;
        }
    }
}
