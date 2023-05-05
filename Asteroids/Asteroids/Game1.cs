﻿using Asteroids.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Asteroids
{
    public class Game1 : Game
    {
        
        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;
        private GameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, GameState> m_states;
        public Game1()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            m_graphics.PreferredBackBufferWidth = 1920;
            m_graphics.PreferredBackBufferHeight = 1080;
            m_graphics.IsFullScreen= true;
            m_graphics.ApplyChanges();
            // Create Dictionary of all GameStates

            m_states = new Dictionary<GameStateEnum, GameState>
            {
                {GameStateEnum.MainMenu, new MainMenuView() },
                {GameStateEnum.Game, new GamePlayView() },
                {GameStateEnum.Continue, new ContinueGameView() },
                {GameStateEnum.Highscores, new HighScoresView() },
                {GameStateEnum.Controls, new ControlsView() },
                {GameStateEnum.Pause, new PauseView() },
                {GameStateEnum.Credits, new CreditsView() },
                {GameStateEnum.GameOver, new GameOverView() }


            };

            //Initialize Gamestates
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics);
            }

            m_currentState = m_states[m_nextStateEnum];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var item in m_states)
            {
                item.Value.loadContent(this.Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            GameStateEnum previous = m_nextStateEnum;


            m_nextStateEnum = m_currentState.processInput(gameTime);



            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }
            else
            {
                m_currentState.update(gameTime);
                m_currentState = m_states[m_nextStateEnum];
                if (m_nextStateEnum != GameStateEnum.Exit && m_states[previous] != m_states[m_nextStateEnum])
                {
                    m_currentState.previousScreen(m_states[previous]);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_currentState.render(gameTime);
            m_currentState = m_states[m_nextStateEnum];


            base.Draw(gameTime);
        }
    }
}