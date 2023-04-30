using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal class MainMenuView : GameState
    {
        
        private Texture2D m_playerTex;
        private SpriteFont m_fontMenuSelect;
        private SpriteFont m_fontMenu;
        private bool m_waitForKeyRelease;
        private Texture2D m_square;
        private enum MenuState
        {

            Game,
            Highscores,
            Credits,
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
            if(!m_waitForKeyRelease)
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
                    if (m_currentSelection != MenuState.Game)
                    {
                        m_currentSelection = m_currentSelection - 1;
                    }

                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Game)
                {
                    m_previousKeyboard = Keyboard.GetState();
                    return GameStateEnum.Game;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Highscores)
                {
                    m_previousKeyboard = Keyboard.GetState();
                    return GameStateEnum.Highscores;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Credits)
                {

                    m_previousKeyboard = Keyboard.GetState();
                    return GameStateEnum.Credits;
                }

        

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !m_previousKeyboard.IsKeyDown(Keys.Enter) && m_currentSelection == MenuState.Exit)
                {
                    return GameStateEnum.Exit;
                }
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.MainMenu;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_square, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            float bottom = drawMenuItem(m_currentSelection == MenuState.Game ? m_fontMenuSelect : m_fontMenu,
                "New Game",
                200,
                m_currentSelection == MenuState.Game ? Color.Yellow : Color.Blue);
            

            bottom = drawMenuItem(m_currentSelection == MenuState.Highscores ? m_fontMenuSelect : m_fontMenu, "High Scores", bottom, m_currentSelection == MenuState.Highscores ? Color.Yellow : Color.Blue);
 
            bottom = drawMenuItem(m_currentSelection == MenuState.Credits ? m_fontMenuSelect : m_fontMenu, "Credits", bottom, m_currentSelection == MenuState.Credits ? Color.Yellow : Color.Blue);

            drawMenuItem(m_currentSelection == MenuState.Exit ? m_fontMenuSelect : m_fontMenu, "Quit", bottom, m_currentSelection == MenuState.Exit ? Color.Yellow : Color.Blue);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            m_previousKeyboard = Keyboard.GetState();
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
            m_currentSelection = MenuState.Game;
        }
    }
}
