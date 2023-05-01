using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace PacMan
{
    public class Ghost
    {

        public (int, int) pos;
        public bool isEdible = false;
        private double edibleTimer = 10;
        public bool facingRight = true;
        public double moveTimer;
        public double speed;
        public int animationFrame = 0;
        public double nextAnimationTimer = 0;
        public double animationSpeed;
        public Rectangle rectangle;
        public List<Texture2D> animation;
        public int direction = 4;
        public Random random = new Random();
        bool chasePlayer = false;
        public int size;

        public Ghost(int x, int y, double speed, List<Texture2D> animation, double animationSpeed, int size)
        {
            pos = (x, y);
            this.speed = speed;
            this.animation = animation;
            this.animationSpeed = animationSpeed;
            this.size = size;
            moveTimer = speed;
        }

        public void drawGhost(SpriteBatch m_spriteBatch, List<Texture2D> deathAnimation)
        {
            if (this.facingRight)
            {
                if (this.isEdible)
                {
                    m_spriteBatch.Draw(deathAnimation[this.animationFrame], this.rectangle, Color.White);
                }
                else
                {
                    m_spriteBatch.Draw(this.animation[this.animationFrame], this.rectangle, Color.White);
                }


            }
            else
            {
                if (this.isEdible)
                {
                    m_spriteBatch.Draw(deathAnimation[this.animationFrame], this.rectangle, null, Color.White, 0, new Vector2(deathAnimation[this.animationFrame].Width / 2, deathAnimation[this.animationFrame].Height / 2 - size),
                SpriteEffects.FlipHorizontally, 0);

                }
                else
                {
                    m_spriteBatch.Draw(this.animation[this.animationFrame], this.rectangle, null, Color.White, 0, new Vector2(this.animation[this.animationFrame].Width / 2 - size , this.animation[this.animationFrame].Height / 2 - size),
                SpriteEffects.FlipHorizontally, 0);
                }


            }
        }

        public void updateGhost(GameTime gameTime, Player player, int[,] MAP, int wallWidth, int XOFFSET, int YOFFSET)
        {
            if(this.isEdible)
            {
                edibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            } 
            if(edibleTimer <=0 )
            {
                this.isEdible = false;
                this.edibleTimer = 10;
            }

           this.rectangle = new Rectangle(this.pos.Item1 * wallWidth + XOFFSET + size / 2, this.pos.Item2 * wallWidth + YOFFSET + size / 2, size, size);

            doChase(player);
            if (this.moveTimer <= 0)
            {
                this.moveTimer = this.speed;


                //If we are chasing the player
                if (chasePlayer)
                {
                    handleChaseMovement(gameTime, MAP, player);
                }
                else
                {
                    handleRandomMovement(gameTime, MAP);
                }
            }
            else
            {
                this.moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            handleAnimation(gameTime);
            

        }

        private void handleChaseMovement(GameTime gametime, int[,] MAP, Player player)
        {
            //Computing distance between ghost and player needs fixed. Should find the shortest path
            //For now just do random movement again. 
            handleRandomMovement(gametime, MAP);
        }
        private void handleRandomMovement(GameTime gameTime, int[,] MAP)
        {
            //If we can move in the same direction we are going keep moving else switch direction

            switch (this.direction)
            {
                case 1: // Right
                    if (MAP[this.pos.Item1 + 1, this.pos.Item2] != 1)
                    {
                        this.pos.Item1 += 1;
                        this.facingRight = true;
                    }
                    else
                    {
                        this.moveTimer = 0;
                        //Randomly Choose Next Direction
                        if (this.random.NextDouble() <= 0.5)
                        {
                            this.direction = 2;
                        }
                        else
                        {
                            this.direction = 3;
                        }

                    }

                    break;
                case 2: // Up

                    if (MAP[this.pos.Item1, this.pos.Item2 - 1] != 1)
                    {
                        this.pos.Item2 -= 1;
                    }
                    else
                    {
                        this.moveTimer = 0;
                        //Randomly Choose Next Direction
                        if (this.random.NextDouble() <= 0.5)
                        {
                            this.direction = 1;
                        }
                        else
                        {
                            this.direction = 4;
                        }

                    }
                    break;
                case 3: // Down

                    if (MAP[this.pos.Item1, this.pos.Item2 + 1] != 1)
                    {
                        this.pos.Item2 += 1;
                    }
                    else
                    {
                        this.moveTimer = 0;
                        //Randomly Choose Next Direction
                        if (this.random.NextDouble() <= 0.5)
                        {
                            this.direction = 1;
                        }
                        else
                        {
                            this.direction = 4;
                        }
                    }
                    break;
                case 4: // Left

                    if (MAP[this.pos.Item1 - 1, this.pos.Item2] != 1)
                    {
                        this.pos.Item1 -= 1;
                        this.facingRight = false;
                    }
                    else
                    {
                        this.moveTimer = 0;
                        //Randomly Choose Next Direction
                        if (this.random.NextDouble() <= 0.5)
                        {
                            this.direction = 2;
                        }
                        else
                        {
                            this.direction = 3;
                        }
                    }
                    break;
            }

        }

        private void handleAnimation(GameTime gameTime)
        {
            if (this.nextAnimationTimer <= 0)
            {
                this.nextAnimationTimer = this.animationSpeed;
                this.animationFrame += 1;
                if (this.animationFrame >= this.animation.Count)
                {
                    this.animationFrame = 0;
                }
            }
            else
            {
                this.nextAnimationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void doChase(Player player)
        {
            if (spacesAwayFromPlayer(player) <= 4)
            {
                chasePlayer = true;
            } else if (spacesAwayFromPlayer(player) >= 10)
            {
                chasePlayer = false;
            }

        }
        private int spacesAwayFromPlayer(Player player)
        {
            int distance = 0;

            distance += Math.Abs(player.pos.Item1 - this.pos.Item1);
            distance += Math.Abs(player.pos.Item2 - this.pos.Item2);
            return distance;

        }
    }
}
