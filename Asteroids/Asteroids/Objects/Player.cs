using Asteroids.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Objects
{
    internal class Player : Entity
    {
        double fireRate;
        float rotation = 0;
        KeyboardInput keyBoard;
        int currentAnimationFrame = 0;
        double animationTimer;
        double animationLength;
        double fireRateTimer;
        double MAX_SPEED = 500;
        Bullet bullet;
        List<Bullet> bullets;
        Vector2 origin;
        public float rotationVelocity = 3f;
        SoundEffect m_shot;

        public Player(int lives, List<Texture2D> animation,Vector2 direction, Vector2 startingPosition, float startingSpeed, double fireRate, SoundEffect shotSound) { 
            
            this.position = startingPosition;
            this.speed = startingSpeed;
            this.direction = direction;
            this.animation = animation;
            this.keyBoard = new KeyboardInput();
            this.lives= lives;
            this.animation = animation;
            this.fireRate= fireRate;
            this.fireRateTimer = fireRate;
            this.animationLength = 0.2;
            m_shot = shotSound;
            origin = new Vector2(animation[0].Width / 2, animation[0].Height / 2);
            this.bullets = new List<Bullet>();
            setInput();
        }

        private void setInput()
        {
            keyBoard = new KeyboardInput();
            keyBoard.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(onTurnLeft));
            keyBoard.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(onTurnRight));
            keyBoard.registerCommand(Keys.Up, false, new InputDeviceHelper.CommandDelegate(onIncreaseSpeed));
            keyBoard.registerCommand(Keys.Down, false, new InputDeviceHelper.CommandDelegate(onDecreaseSpeed));
            keyBoard.registerCommand(Keys.Space, false, new InputDeviceHelper.CommandDelegate(onFire));
        }

        private void onFire(GameTime gameTime, float scale)
        {
            
          
         //Fire Bullets 
        if (fireRateTimer <= 0)
            {

            m_shot.Play(0.5f, 0, 0);
            fireRateTimer = fireRate;
            bullet = new Bullet();
            bullets.Add(bullet);

            }

            
        }
      
        private void onTurnLeft(GameTime gameTime, float scale)
        {
            rotation -= MathHelper.ToRadians(rotationVelocity);
            
        }
        private void onTurnRight(GameTime gameTime, float scale)
        {
            rotation += MathHelper.ToRadians(rotationVelocity);
            
        }
        private void onIncreaseSpeed(GameTime gameTime, float scale)
        {
            if (speed < MAX_SPEED)
            {
                speed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }
        private void onDecreaseSpeed(GameTime gameTime, float scale)
        {
            if(speed > 0)
            {
                speed -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }


        public override void update(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            animationTimer -= elapsedTime;
            if (animationTimer < 0)
            {
                animationTimer = animationLength;
                currentAnimationFrame++;
                currentAnimationFrame = currentAnimationFrame % animation.Count;
            }
            keyBoard.Update(gameTime);
            updateBullets(gameTime);

            if(speed >= 0 )
            {
                speed -= (float)elapsedTime * 1000 / 5;
            } else
            {
                speed = 0;
            }
            
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * (float)speed * (float)elapsedTime;
            fireRateTimer -= elapsedTime;
           
            
        }

        public void render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(animation[currentAnimationFrame], rectangle, Color.White);

            spriteBatch.Draw(animation[currentAnimationFrame], position, null, Color.White, rotation, origin, 0.25f, SpriteEffects.None, 0f);
            renderBullets(gameTime, spriteBatch);
            
        }


        public void renderBullets(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Bullet bullet in bullets)
            {
                
            }
        }

        public void updateBullets(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
            {

            }
        }

    }
}
