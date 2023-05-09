using Asteroids.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Objects
{
    public class Bullet : Entity
    {
        
        float rotation;
        Vector2 origin;
        Texture2D texture;
        public Bullet(float rotation, Texture2D texture, Vector2 direction, Vector2 startingPosition, float startingSpeed)
        {
            this.rotation = rotation;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.texture = texture;
            this.position = startingPosition;
            
            this.speed = startingSpeed;
            this.direction = direction;
        }

        public override void update(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            position += direction * (float)speed * (float)elapsedTime;
        }

        public void render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, 0.005f, SpriteEffects.None, 0f);
        }
    }
}
