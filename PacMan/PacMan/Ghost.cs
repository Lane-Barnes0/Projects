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
        public int size;

        public Ghost(int x, int y, double speed, List<Texture2D> animation, double animationSpeed, int size)
        {
            pos = (x, y);
            this.speed = speed;
            this.animation = animation;
            this.animationSpeed = animationSpeed;
            this.size = size;
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

        public void updateGhost(GameTime gameTime, int[,] MAP, int wallWidth, int XOFFSET, int YOFFSET)
        {
           this.rectangle = new Rectangle(this.pos.Item1 * wallWidth + XOFFSET + size / 2, this.pos.Item2 * wallWidth + YOFFSET + size / 2, size, size);


            if (this.moveTimer <= 0)
            {
                this.moveTimer = this.speed;

                //If can move in direction we are going keep moving else switch direction

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
            else
            {
                this.moveTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

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

        

    }
}
