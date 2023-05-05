using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids.Views
{
    internal class GamePlayView : GameState
    {
        public override void loadContent(ContentManager contentManager)
        {
            
        }

        public override void previousScreen(GameState screen)
        {
           
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }
            return GameStateEnum.Game;
        }

        public override void render(GameTime gameTime)
        {
            
        }

        public override void update(GameTime gameTime)
        {
            
        }
    }
}
