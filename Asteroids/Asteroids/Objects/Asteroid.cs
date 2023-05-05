using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Objects
{
    public class Asteroid : Entity
    {
       

        public Asteroid(int lives, List<Texture2D> animation, Vector2 direction, Vector2 startingPosition, float startingSpeed) { 
        
            this.lives = lives;
            this.animation = animation;
            this.direction = direction;
            this.position= startingPosition;
            this.speed = startingSpeed;

        }
        public override void update(GameTime gameTime)
        {
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
