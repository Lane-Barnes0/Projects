using Asteroids.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Objects
{
    internal class Player : Entity
    {
        double fireRate;
        double rotation = 0;
        KeyboardInput keyBoard;

        public Player(int lives, List<Texture2D> animation, Vector2 direction, Vector2 startingPosition, float startingSpeed, double fireRate) { 
            
            this.position = startingPosition;
            this.speed = startingSpeed;
            this.direction = direction;
            this.animation = animation;
            this.keyBoard = new KeyboardInput();
            this.lives= lives;
            this.animation = animation;
            this.fireRate= fireRate;
        }
        public override void update(GameTime gameTime)
        {
            
        }
    }
}
