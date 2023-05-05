using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Objects
{
    public abstract class Entity
    {
        public int lives;
        public List<Texture2D> animation;
        public Vector2 direction;
        public Vector2 position;
        public float speed;
        protected Entity() { }

        public abstract void update(GameTime gameTime);

        //https://gamedev.stackexchange.com/questions/7755/moving-a-sprite-in-xna-c-using-vectors
    }
}
