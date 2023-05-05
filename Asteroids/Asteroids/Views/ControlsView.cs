using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Asteroids.Views
{
    internal class ControlsView : GameState
    {
      


        public override void loadContent(ContentManager contentManager)
        {
            
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.Controls;
        }

        public override void render(GameTime gameTime)
        {
            
        }

        public override void update(GameTime gameTime)
        {
        }

        public override void previousScreen(GameState screen)
        {

        }
    }
}

