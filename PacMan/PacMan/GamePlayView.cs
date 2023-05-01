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
        
        private bool saving;
        private bool loading;
        private bool m_gameOver;
        private int wallWidth;
        private int XOFFSET;
        private int YOFFSET;
        private int foodEaten;
       
        private double spawnGhost;
        
        private const double SPEED = 0.15;
        private int currentPlayerFrame;
        private double nextPlayerFrame;
        private int currentChompSound;
        private double nextChompSound;
        private List<SoundEffect> chomp;
        private List<Ghost> ghosts;
        private List<Ghost> ghostsToDelete;
        private double moveLimit;
        private int score;
        private int lives ;
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
    {3, 3, 3, 3, 1, 2, 1, 2, 2, 2, 2, 2, 2, 2, 1, 2, 1, 3, 3, 3, 0},
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

            wallWidth = 40;
            saving = false;
            loading = false;
            XOFFSET = m_graphics.PreferredBackBufferWidth / 2 - MAP.GetLength(0) * wallWidth / 2;
            YOFFSET = m_graphics.PreferredBackBufferHeight / 2 - MAP.GetLength(1) * wallWidth / 2;
           
            m_playerPos = (MAP.GetLength(0) / 2, MAP.GetLength(1) / 2 + 3);
            m_player = new Rectangle(m_playerPos.Item1, m_playerPos.Item2, SPRITE_SIZE, SPRITE_SIZE);

            currentPlayerFrame = 0;
            nextPlayerFrame = 0.2;

            moveLimit = SPEED;
            score = 0;
            currentChompSound = 0;
            nextChompSound = 0.2;
            foodEaten = 0;
            ghosts = new List<Ghost>();
            m_gameOver = false;
            ghostsToDelete = new List<Ghost>();
            lives = 3;
            spawnGhost = 0;
            //Test
            nextGhostColor= 0;

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
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.Pause;
            }

            if(m_gameOver)
            {
                
                return GameStateEnum.MainMenu;
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

            foreach(Ghost ghost in ghosts)
            {
                Color ghostColor = Color.White;
                if(ghost.isEdible) { ghostColor = Color.Orange; }
                if(ghost.facingRight)
                {
                    m_spriteBatch.Draw(ghost.animation[ghost.animationFrame], ghost.rectangle, ghostColor);

                } else
                {
                    m_spriteBatch.Draw(ghost.animation[ghost.animationFrame], ghost.rectangle, null, ghostColor, 0, new Vector2(ghost.animation[ghost.animationFrame].Width / 2, ghost.animation[ghost.animationFrame].Height / 2 - SPRITE_SIZE),
                    SpriteEffects.FlipHorizontally, 0);

                }
                
               
            }
            
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


        private void onMoveLeft(GameTime gameTime, float scale)
        {
            playChompSound();
            m_playerFacing = 2;

            if (moveLimit < 0)
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
                if (m_playerPos.Item2 + 1 > MAP.GetLength(1) - 1)
                {
                    m_playerPos = (m_playerPos.Item1, 0);
                }
                else
                if (MAP[m_playerPos.Item1, m_playerPos.Item2 + 1] != 1)
                {
                    m_playerPos = (m_playerPos.Item1, m_playerPos.Item2 + 1);

                }
            }

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
                       
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET, wallWidth, wallWidth / 10), Color.Blue);
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET, wallWidth / 10, wallWidth), Color.Blue);

                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET + wallWidth, j * wallWidth + YOFFSET, wallWidth / 10, wallWidth), Color.Blue);
                        m_spriteBatch.Draw(m_squareTexture, new Rectangle(i * wallWidth + XOFFSET, j * wallWidth + YOFFSET + wallWidth, wallWidth, wallWidth / 10 ), Color.Blue);

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

            

            if (m_inputKeyboard == null)
            {
                newGame();
            } else
            {
                if(lives <= 0)
                {
                    m_scores.Add(score);
                    saveScore();
                    m_gameOver = true;
                }


                m_inputKeyboard.Update(gameTime);
                foodCollision();
                playerAnimation(gameTime);
                resetFood();

                

                

                nextChompSound -= gameTime.ElapsedGameTime.TotalSeconds;
                moveLimit -= gameTime.ElapsedGameTime.TotalSeconds;

                m_player.X = m_playerPos.Item1 * wallWidth + XOFFSET + SPRITE_SIZE ;
                m_player.Y = m_playerPos.Item2 * wallWidth + YOFFSET + SPRITE_SIZE ;

                //Spawn Ghosts

                if(spawnGhost <= 0 && ghosts.Count <= 8)
                {
                    ghosts.Add(new Ghost(MAP.GetLength(0) / 2, MAP.GetLength(1) / 2, 0.3, ghostAnimations[nextGhostColor], 0.2));
                    nextGhostColor += 1;
                    nextGhostColor = nextGhostColor % ghostAnimations.Count;
                    spawnGhost = 20;
                }
                spawnGhost -= gameTime.ElapsedGameTime.TotalSeconds;

                foreach(Ghost ghost in ghosts)
                {
                    //Update Ghosts

                    updateGhost(gameTime, ghost);

                }

                //Remove any Eaten Ghosts

                foreach(Ghost ghost in ghostsToDelete)
                {

                    ghosts.Remove(ghost);
                }
                ghostsToDelete.Clear();

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
                //Test to test Ghost Edible
                foreach(Ghost ghost in ghosts)
                {
                    ghost.isEdible = true;
                }
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


      

        public void updateGhost(GameTime gameTime, Ghost ghost)
        {
            ghost.rectangle = new Rectangle(ghost.pos.Item1 * wallWidth + XOFFSET + SPRITE_SIZE / 2, ghost.pos.Item2 * wallWidth + YOFFSET + SPRITE_SIZE / 2, SPRITE_SIZE, SPRITE_SIZE);


            if (ghost.moveTimer <= 0)
            {
                ghost.moveTimer = ghost.speed;

                //If can move in direction we are going keep moving else switch direction

                switch (ghost.direction)
                {
                    case 1: // Right
                        if (MAP[ghost.pos.Item1 + 1, ghost.pos.Item2 ] != 1)
                        {
                            ghost.pos.Item1 += 1;
                            ghost.facingRight = true;
                        } else
                        {
                            ghost.moveTimer = 0;
                            //Randomly Choose Next Direction
                            if (ghost.random.NextDouble() <= 0.5)
                            {
                                ghost.direction = 2;
                            }
                            else
                            {
                                ghost.direction = 3;
                            }

                        }
                        
                        break;
                    case 2: // Up
                        
                        if (MAP[ghost.pos.Item1, ghost.pos.Item2 - 1] != 1)
                        {
                            ghost.pos.Item2 -= 1;
                        }
                        else
                        {
                            ghost.moveTimer = 0;
                            //Randomly Choose Next Direction
                            if (ghost.random.NextDouble() <= 0.5)
                            {
                                ghost.direction = 1;
                            } else
                            {
                                ghost.direction = 4;
                            }
                            
                        }
                        break;
                    case 3: // Down
                        
                        if (MAP[ghost.pos.Item1, ghost.pos.Item2 + 1] != 1)
                        {
                            ghost.pos.Item2 += 1;
                        }
                        else
                        {
                            ghost.moveTimer = 0;
                            //Randomly Choose Next Direction
                            if (ghost.random.NextDouble() <= 0.5)
                            {
                                ghost.direction = 1;
                            }
                            else
                            {
                                ghost.direction = 4;
                            }
                        }
                        break;
                    case 4: // Left
                       
                        if (MAP[ghost.pos.Item1 - 1, ghost.pos.Item2] != 1)
                        {
                            ghost.pos.Item1 -= 1;
                            ghost.facingRight = false;
                        }
                        else
                        {
                            ghost.moveTimer = 0;
                            //Randomly Choose Next Direction
                            if (ghost.random.NextDouble() <= 0.5)
                            {
                                ghost.direction = 2;
                            }
                            else
                            {
                                ghost.direction = 3;
                            }
                        }
                        break;
                }


            }
            else
            {
                ghost.moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ghost.nextAnimationTimer <= 0)
            {
                ghost.nextAnimationTimer = ghost.animationSpeed;
                ghost.animationFrame += 1;
                if (ghost.animationFrame >= ghost.animation.Count)
                {
                    ghost.animationFrame = 0;
                }
            }
            else
            {
                ghost.nextAnimationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            checkGhostCollision(ghost);

        }

        private void checkGhostCollision(Ghost ghost)
        {
            if(m_playerPos == ghost.pos)
            {
                if(!ghost.isEdible)
                {
                    lives -= 1;
                    m_playerPos = (MAP.GetLength(0) / 2, MAP.GetLength(1) / 2 + 3);
                } else
                {
                    ghostsToDelete.Add(ghost);
                    score += 200;
                }
               
            }
        }

        public class Ghost
        {

            public (int, int) pos;
            public bool isEdible = false;
            public bool facingRight = true;
            public double moveTimer = 0;
            public double speed;
            public int animationFrame = 0;
            public double nextAnimationTimer = 0;
            public double animationSpeed;
            public Rectangle rectangle;
            public List<Texture2D> animation;
            public int direction = 4;
            public Random random = new Random();


            public Ghost(int x, int y, double speed, List<Texture2D> animation, double animationSpeed) 
            {
                pos = (x, y);
                this.speed = speed;
                this.animation = animation;
                this.animationSpeed = animationSpeed;
            }
           
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
