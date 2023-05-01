using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacMan.Input;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


namespace PacMan.Views
{
    public class GamePlayView : GameState
    {

        private bool saving;
        private bool loading;
        private bool m_gameOver;
        private int wallWidth;
        private int XOFFSET;
        private int YOFFSET;
        private Player m_player;
        private bool m_quitGame;
        private double spawnGhost;

       

        private int foodEaten;

        private List<SoundEffect> chomp;
        private List<Ghost> ghosts;
        private List<Ghost> ghostsToDelete;
        private Scoring scoringSystem;
        
        private int score;
        
        private int nextGhostColor;

        List<List<Texture2D>> ghostAnimations;


        //1 Wall, 2 Food, 3 Empty space outside playable area
        private int[,] MAP = new int[,] {
    {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
    {1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1},
    {1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1},
    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
    {1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1},
    {1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1},
    {1, 1, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 1, 1},
    {3, 3, 3, 3, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 3, 3, 3, 3},
    {1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1},
    {2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2},
    {1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1},
    {3, 3, 3, 3, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 3, 3, 3, 3},
    {3, 3, 3, 3, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 3, 3, 3, 3},
    {1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1},
    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
    {1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 1, 2, 1},
    {1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1},
    {1, 1, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 2, 1, 1},
    {1, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 1},
    {1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 2, 1},
    {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };




        private const int SPRITE_SIZE = 20;

        
        private SpriteFont m_font;
        private SpriteFont m_fontMenu;


        private List<int> m_scores;

        


        
        private Texture2D m_squareTexture;
        private List<Texture2D> m_playerTex;



        public void newGame()
        {
           
            m_player = new Player(3, (MAP.GetLength(0) / 2, MAP.GetLength(1) / 2 + 3), SPRITE_SIZE, m_playerTex, chomp, MAP);



            m_quitGame = false;

            wallWidth = 40;
            saving = false;
            loading = false;
            XOFFSET = m_graphics.PreferredBackBufferWidth / 2 - MAP.GetLength(0) * wallWidth / 2;
            YOFFSET = m_graphics.PreferredBackBufferHeight / 2 - MAP.GetLength(1) * wallWidth / 2;

           

            
            foodEaten = 0;
            ghosts = new List<Ghost>();
            m_gameOver = false;
            ghostsToDelete = new List<Ghost>();
            
            spawnGhost = 0;
            score = 0;
            //Test
            nextGhostColor = 0;
            scoringSystem = new Scoring();

        }
        




        public override void loadContent(ContentManager contentManager)
        {

            chomp = new List<SoundEffect>
            {
                contentManager.Load<SoundEffect>("Audio/munch_1"),
                contentManager.Load<SoundEffect>("Audio/munch_2")
            };
            m_squareTexture = contentManager.Load<Texture2D>("Images/square");
            m_playerTex = new List<Texture2D> {
                contentManager.Load<Texture2D>("Images/player0"),
                contentManager.Load<Texture2D>("Images/player1"),
                contentManager.Load<Texture2D>("Images/player2")
        };

            m_font = contentManager.Load<SpriteFont>("Fonts/smallFont");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            ghostAnimations = new List<List<Texture2D>>();

            //Red Ghost
            ghostAnimations.Add(new List<Texture2D>());
            ghostAnimations[0].Add(contentManager.Load<Texture2D>("Images/redGhost0"));
            ghostAnimations[0].Add(contentManager.Load<Texture2D>("Images/redGhost1"));
            //Blue Ghost
            ghostAnimations.Add(new List<Texture2D>());
            ghostAnimations[1].Add(contentManager.Load<Texture2D>("Images/blueGhost0"));
            ghostAnimations[1].Add(contentManager.Load<Texture2D>("Images/blueGhost1"));
            //Pink Ghost
            ghostAnimations.Add(new List<Texture2D>());
            ghostAnimations[2].Add(contentManager.Load<Texture2D>("Images/pinkGhost0"));
            ghostAnimations[2].Add(contentManager.Load<Texture2D>("Images/pinkGhost1"));
            //Yellow Ghost
            ghostAnimations.Add(new List<Texture2D>());
            ghostAnimations[3].Add(contentManager.Load<Texture2D>("Images/yellowGhost0"));
            ghostAnimations[3].Add(contentManager.Load<Texture2D>("Images/yellowGhost1"));

            //Edible Ghost
            ghostAnimations.Add(new List<Texture2D>());
            ghostAnimations[4].Add(contentManager.Load<Texture2D>("Images/edibleGhost0"));
            ghostAnimations[4].Add(contentManager.Load<Texture2D>("Images/edibleGhost1"));
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.Pause;
            }
            if(m_gameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    m_quitGame = true;
                }
            }
            if (m_quitGame)
            {
                m_scores.Add(score);
                scoringSystem.saveScore(saving, m_scores);

                
                return GameStateEnum.MainMenu;
            }
            return GameStateEnum.Game;

        }


        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            m_spriteBatch.Draw(m_squareTexture, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            if (m_gameOver)
            {
                Vector2 stringSize = m_fontMenu.MeasureString("Game Over ");
                m_spriteBatch.DrawString(
                   m_fontMenu,
                    "Game Over",
                   new Vector2((m_graphics.PreferredBackBufferWidth - stringSize.X) / 2, 10),
                   Color.Red);

                 stringSize = m_fontMenu.MeasureString("Your Score " + score.ToString());
                m_spriteBatch.DrawString(
                   m_fontMenu,
                    "Your Score " + score.ToString(),
                   new Vector2((m_graphics.PreferredBackBufferWidth - stringSize.X) / 2, m_graphics.PreferredBackBufferHeight / 2 - 100),
                   Color.Red);

                stringSize = m_fontMenu.MeasureString("Press Enter to Quit");
                m_spriteBatch.DrawString(
                   m_fontMenu,
                    "Press Enter to Quit",
                   new Vector2((m_graphics.PreferredBackBufferWidth - stringSize.X) / 2, m_graphics.PreferredBackBufferHeight / 2 - 25),
                   Color.Red);


            }
            else
            {
                drawWalls();
                drawScore();
                drawFood();
                drawLives();
                foreach (Ghost ghost in ghosts)
                {
                    ghost.drawGhost(m_spriteBatch, ghostAnimations[4]);
                }
                m_player.draw(m_spriteBatch);
            }
            m_spriteBatch.End();
        }


       

        private void drawLives()
        {
            for (int i = 0; i < m_player.lives; i++)
            {
                m_spriteBatch.Draw(m_playerTex[0], new Rectangle(50 * i + 50, m_graphics.PreferredBackBufferHeight - 100, 30, 30), Color.White);
            }

        }

        public void drawWalls()
        {
            for (int i = 0; i < MAP.GetLength(0); i++)
            {

                for (int j = 0; j < MAP.GetLength(1); j++)
                {

                    //If theres a wall
                    if (MAP[i, j] == 1)
                    {

                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET, wallWidth, wallWidth / 10), Color.Blue);
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET, wallWidth / 10, wallWidth), Color.Blue);

                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET + wallWidth, j * wallWidth + YOFFSET, wallWidth / 10, wallWidth), Color.Blue);
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET + wallWidth, wallWidth + (wallWidth /10), wallWidth / 10), Color.Blue);

                    }
                }
            }

        }

        public void drawFood()
        {
            for (int i = 0; i < MAP.GetLength(0); i++)
            {

                for (int j = 0; j < MAP.GetLength(1); j++)
                {

                    //If theres a wall
                    if (MAP[i, j] == 2)
                    {
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET + 10, j * wallWidth + YOFFSET + 10, 10, 10), Color.LightYellow);

                    }
                }
            }
        }


        public override void update(GameTime gameTime)
        {



            if (m_player == null)
            {
                newGame();
            }
            else if (!m_gameOver)
            {

                if (m_player.lives <= 0)
                {

                    m_gameOver = true;
                }

                
                foodCollision();
                m_player.playerAnimation(gameTime);
                resetFood();
                m_player.updatePlayer(gameTime, wallWidth, XOFFSET, YOFFSET);


                //Spawn Ghosts

                if (spawnGhost <= 0 && ghosts.Count <= 8)
                {
                    ghosts.Add(new Ghost(MAP.GetLength(0) / 2, MAP.GetLength(1) / 2, 0.3, ghostAnimations[nextGhostColor], 0.2, SPRITE_SIZE));
                    nextGhostColor += 1;

                    //The last animation is of the edible ghost so skip that one
                    nextGhostColor = nextGhostColor % (ghostAnimations.Count - 1);
                    spawnGhost = 10;
                }
                spawnGhost -= gameTime.ElapsedGameTime.TotalSeconds;

                foreach (Ghost ghost in ghosts)
                {
                    //Update Ghosts

                    ghost.updateGhost(gameTime, m_player, MAP, wallWidth, XOFFSET, YOFFSET);
                    checkGhostCollision(ghost);

                }

                //Remove any Eaten Ghosts

                foreach (Ghost ghost in ghostsToDelete)
                {

                    ghosts.Remove(ghost);
                }
                ghostsToDelete.Clear();

                scoringSystem.loadScores(loading);
                if (scoringSystem.getLoadedState() != null)
                {
                    m_scores = scoringSystem.getLoadedState().Score;
                }
                else
                {
                    m_scores = new List<int>();
                }
            }





        }

        private void resetFood()
        {
            if (foodEaten % 221 == 0)
            {
                for (int i = 0; i < MAP.GetLength(0); i++)
                {

                    for (int j = 0; j < MAP.GetLength(1); j++)
                    {

                        //If theres a wall
                        if (MAP[i, j] == 0)
                        {
                            MAP[i, j] = 2;

                        }
                    }
                }
                //Test to test Ghost Edible
                foreach (Ghost ghost in ghosts)
                {
                    ghost.isEdible = true;
                }
            }


        }
        
        private void foodCollision()
        {
            if (MAP[m_player.pos.Item1, m_player.pos.Item2] == 2)
            {
                MAP[m_player.pos.Item1, m_player.pos.Item2] = 0;
                score += 10;
                foodEaten++;
            }



        }
        private void drawScore()
        {
            Vector2 stringSize = m_font.MeasureString("Score " + score.ToString());
            m_spriteBatch.DrawString(
               m_font,
                "Score " + score.ToString(),
               new Vector2((m_graphics.PreferredBackBufferWidth - stringSize.X) / 2, 10),
               Color.Red);

        }




        public void checkGhostCollision(Ghost ghost)
        {
            if (m_player.pos == ghost.pos)
            {
                if (!ghost.isEdible)
                {
                    m_player.lives -= 1;
                    m_player.pos = (MAP.GetLength(0) / 2, MAP.GetLength(1) / 2 + 3);
                }
                else
                {
                    ghostsToDelete.Add(ghost);
                    score += 200;
                }

            }
        }



        public override void previousScreen(GameStateEnum screen)
        {
            if (screen == GameStateEnum.MainMenu)
            {
                newGame();
            }


        }



    }


}
