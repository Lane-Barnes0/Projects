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

namespace PacMan
{
    public class GamePlayView : GameState
    {
        
        private bool saving = false;
        private bool loading = false;
        private int wallWidth = 40;
        private int XOFFSET;
        private int YOFFSET;
        private int foodEaten;

        
        private const double SPEED = 0.15;
        private int currentPlayerFrame;
        private double nextPlayerFrame;
        private int currentChompSound;
        private double nextChompSound;
        private List<SoundEffect> chomp;
        private double moveLimit;
        private int score;
        private int lives = 3;
       

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
    {3, 3, 3, 3, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 3, 3, 3, 0},
    {1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1, 1, 1, 1, 1},
    {2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2},
    {1, 1, 1, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1},
    {3, 3, 3, 3, 1, 2, 1, 2, 1, 1, 1, 1, 1, 2, 1, 2, 1, 3, 3, 3, 0},
    {3, 3, 3, 3, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 3, 3, 3, 0},
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

    

        private const float SPRITE_MOVE_PIXELS_PER_MS = 600.0f / 1000.0f;
        private const int SPRITE_SIZE = 20;
   
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;
        private SpriteFont m_font;

        private int m_playerFacing;
        private (int,int) m_playerPos;
        private List<int> m_scores;
       
        Rectangle m_player;


        private KeyboardInput m_inputKeyboard;
        private Texture2D m_squareTexture;
        private List<Texture2D> m_playerTex;

       

        public void newGame()
        {
            m_inputKeyboard = new KeyboardInput();

            m_inputKeyboard.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(onMoveRight));
            m_inputKeyboard.registerCommand(Keys.Up, false, new InputDeviceHelper.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.Down, false, new InputDeviceHelper.CommandDelegate(onMoveDown));
            
            //0 is Right, 1 is Down, 2 is Left, 3 is Up
            m_playerFacing = 0;
            
            
            XOFFSET = m_graphics.PreferredBackBufferWidth / 2 - MAP.GetLength(0) * wallWidth / 2;
            YOFFSET = m_graphics.PreferredBackBufferHeight / 2 - MAP.GetLength(1) * wallWidth / 2;
           
            m_playerPos = (MAP.GetLength(0) / 2, MAP.GetLength(1) / 2);
            m_player = new Rectangle(m_playerPos.Item1, m_playerPos.Item2, SPRITE_SIZE, SPRITE_SIZE);

            currentPlayerFrame = 0;
            nextPlayerFrame = 0.2;

            moveLimit = SPEED;
            score = 0;
            currentChompSound = 0;
            nextChompSound = 0.2;
            foodEaten = 0;

            

        }
        private void playChompSound()
        {
            if (nextChompSound <= 0)
            {
                chomp[currentChompSound].Play();
                currentChompSound += 1;
                if (currentChompSound > 1)
                {
                    currentChompSound = 0;
                }

                nextChompSound = 0.2;
            }

        }
        private void onMoveLeft(GameTime gameTime, float scale)
        {
            playChompSound();
            m_playerFacing = 2;

            if(moveLimit < 0)
            {
                moveLimit = SPEED;
                if (MAP[m_playerPos.Item1 - 1, m_playerPos.Item2] != 1)
                {
                    m_playerPos = (m_playerPos.Item1 - 1, m_playerPos.Item2);

                }
            }

            



        }

        

        private void onMoveRight(GameTime gameTime, float scale)
        {

            m_playerFacing = 0;
            
                
           playChompSound();
            
            
            
            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (MAP[m_playerPos.Item1 + 1, m_playerPos.Item2] != 1)
                {
                    m_playerPos = (m_playerPos.Item1 + 1, m_playerPos.Item2);

                }
            }

           


        }
        private void onMoveUp(GameTime gameTime, float scale)
        {

            m_playerFacing = 3;
            playChompSound();
            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (m_playerPos.Item2 - 1 < 0)
                {
                    m_playerPos = (m_playerPos.Item1, MAP.GetLength(1) - 1);
                }
                else
                if (MAP[m_playerPos.Item1, m_playerPos.Item2 - 1] != 1)
                {
                    m_playerPos = (m_playerPos.Item1, m_playerPos.Item2 - 1);

                }
            }

            


        }
        private void onMoveDown(GameTime gameTime, float scale)
        {
            playChompSound();
            m_playerFacing = 1;

            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if(m_playerPos.Item2 + 1 > MAP.GetLength(1) - 1)
                {
                    m_playerPos = (m_playerPos.Item1, 0);
                } else
                if (MAP[m_playerPos.Item1, m_playerPos.Item2 + 1] != 1)
                {
                    m_playerPos = (m_playerPos.Item1, m_playerPos.Item2 + 1);

                }
            }

            



        }



        public override void loadContent(ContentManager contentManager)
        {
            
            chomp = new List<SoundEffect>
            {
                contentManager.Load<SoundEffect>("Audio/munch_1"),
                contentManager.Load<SoundEffect>("Audio/munch_2")
            };
            m_squareTexture = contentManager.Load<Texture2D>("Images/square");
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
            m_playerTex = new List<Texture2D> {
                contentManager.Load<Texture2D>("Images/player0"),
                contentManager.Load<Texture2D>("Images/player1"),
                contentManager.Load<Texture2D>("Images/player2")
        };
            m_font = contentManager.Load<SpriteFont>("Fonts/smallFont");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.Pause;
            }
             return GameStateEnum.Game;
            
        }


        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            
            
            m_spriteBatch.Draw(m_squareTexture, new Rectangle(0, 0, m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight), Color.Black);
            drawWalls();
            drawScore();
            drawFood();
            drawLives();
            
            if(m_playerFacing == 2)
            {
                m_spriteBatch.Draw(m_playerTex[currentPlayerFrame], m_player, null, Color.White, (float)(Math.PI / 2 * m_playerFacing), new Vector2(m_playerTex[currentPlayerFrame].Width / 2, m_playerTex[currentPlayerFrame].Height / 2),
                    SpriteEffects.FlipVertically, 0);
            } else
            {
                m_spriteBatch.Draw(m_playerTex[currentPlayerFrame], m_player, null, Color.White, (float)(Math.PI / 2 * m_playerFacing), new Vector2(m_playerTex[currentPlayerFrame].Width / 2, m_playerTex[currentPlayerFrame].Height / 2),
                    SpriteEffects.None, 0);
            }
            
            m_spriteBatch.End();
        }


        private void drawLives()
        {
            for (int i = 0; i < lives; i++)
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
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET, wallWidth, wallWidth), Color.Blue);

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
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET + 10, j * wallWidth + YOFFSET + 10, 10, 10), Color.Yellow);

                    }
                }
            }
        }

      
        public override void update(GameTime gameTime)
        {

            

            if (m_inputKeyboard == null)
            {
                newGame();
            } else
            {
               
                m_inputKeyboard.Update(gameTime);
                foodCollision();
                playerAnimation(gameTime);
                resetFood();
                nextChompSound -= gameTime.ElapsedGameTime.TotalSeconds;
                moveLimit -= gameTime.ElapsedGameTime.TotalSeconds;

                m_player.X = m_playerPos.Item1 * wallWidth + XOFFSET + SPRITE_SIZE ;
                m_player.Y = m_playerPos.Item2 * wallWidth + YOFFSET + SPRITE_SIZE ;

                loadScores();
                if (m_loadedState != null)
                {
                    m_scores = m_loadedState.Score;
                }
                else
                {
                    m_scores = new List<int>();
                }
            }


           


        }

        private void resetFood()
        {
            if(foodEaten % 221 == 0)
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
                //Test to save score
                m_scores.Add(score);
                saveScore();
            }

            
        }
        private void playerAnimation(GameTime gameTime)
        {
            nextPlayerFrame -= gameTime.ElapsedGameTime.TotalSeconds;
            if(nextPlayerFrame <= 0)
            {
                currentPlayerFrame += 1;
                if(currentPlayerFrame >= 3)
                {
                    currentPlayerFrame = 0;
                }
                nextPlayerFrame = 0.2;
            }
        }
        private void foodCollision()
        {
            if (MAP[m_playerPos.Item1, m_playerPos.Item2] == 2)
            {
                MAP[m_playerPos.Item1, m_playerPos.Item2] = 0;
                score += 10;
                foodEaten++;
            }
                    
                
            
        }
        private void drawScore()
        {
            Vector2 stringSize = m_font.MeasureString("Score " + score.ToString());
            m_spriteBatch.DrawString(
               m_font,
                "Score " + (score).ToString(),
               new Vector2((m_graphics.PreferredBackBufferWidth - stringSize.X) / 2, 10),
               Color.Red);

        }


        




        private bool intersect(Rectangle r1, Rectangle r2)
        {
            

            bool theyDo = !(
            r2.Left > r1.Right ||
            r2.Right < r1.Left ||
            r2.Top > r1.Bottom ||
            r2.Bottom < r1.Top);
            return theyDo;
        }

        


        private void loadScores()
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
                    finalizeLoadAsync();
                }
            }
        }

        private Scoring m_loadedState = null;

        private async void finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("PacManHighscores.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("PacManHighscores.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(Scoring));
                                    m_loadedState = (Scoring)mySerializer.Deserialize(fs);
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                //this.loading = false;
            });

        }

        private void saveScore()
        {
            lock (this)
            {
                if (!this.saving)
                {
                    this.saving = true;
                    //
                    // Create something to save
                    Scoring myState = new Scoring(m_scores);
                    finalizeSaveAsync(myState);
                }
            }
        }


        private async void finalizeSaveAsync(Scoring state)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("PacManHighscores.xml", FileMode.Create))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(Scoring));
                                mySerializer.Serialize(fs, state);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                this.saving = false;
            });
        }

        public override void previousScreen(GameStateEnum screen)
        {
            if(screen == GameStateEnum.MainMenu)
            {
                newGame();
            }
                
            
        }
    }


}
