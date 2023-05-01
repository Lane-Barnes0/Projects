using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacMan.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Player
    {
        private int currentPlayerFrame;
        private double nextPlayerFrame;
        private List<SoundEffect> noise;
        private int currentNoiseSound;
        private double nextNoiseTimer;
        private double moveLimit;
        public int lives;
        private int[,] MAP;
        private List<Texture2D> m_playerTex;
        public KeyboardInput m_inputKeyboard = new KeyboardInput();
        int size;

        //0 is Right, 1 is Down, 2 is Left, 3 is Up
        private int m_playerFacing = 0;

        private const double SPEED = 0.15;
        Rectangle rectangle;
        public (int, int) pos;

        public Player(int lives, (int, int) pos, int size, List<Texture2D> texture, List<SoundEffect> noise, int[,] map)
        {
            currentPlayerFrame = 0;
            nextPlayerFrame = 0.2;
            currentNoiseSound = 0;
            nextNoiseTimer = 0.2;
            moveLimit = SPEED;
            this.lives = lives;
            this.pos = pos;
            this.size = size;
            rectangle = new Rectangle(pos.Item1, pos.Item2, size, size);
            this.noise = noise;
            MAP = map;
            m_playerTex = texture;

            //Set Input
            m_inputKeyboard.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(onMoveLeft));
            m_inputKeyboard.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(onMoveRight));
            m_inputKeyboard.registerCommand(Keys.Up, false, new InputDeviceHelper.CommandDelegate(onMoveUp));
            m_inputKeyboard.registerCommand(Keys.Down, false, new InputDeviceHelper.CommandDelegate(onMoveDown));
        }

        

        public void updatePlayer(GameTime gameTime, int wallWidth, int XOFFSET, int YOFFSET)
        {

            m_inputKeyboard.Update(gameTime);
            nextNoiseTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            moveLimit -= gameTime.ElapsedGameTime.TotalSeconds;

            rectangle.X = pos.Item1 * wallWidth + XOFFSET + size;
            rectangle.Y = pos.Item2 * wallWidth + YOFFSET + size;
        }

        public void draw(SpriteBatch m_spriteBatch)
        {
            if (m_playerFacing == 2)
            {
                m_spriteBatch.Draw(m_playerTex[currentPlayerFrame], rectangle, null, Color.White, (float)(Math.PI / 2 * m_playerFacing), new Vector2(m_playerTex[currentPlayerFrame].Width / 2, m_playerTex[currentPlayerFrame].Height / 2),
                    SpriteEffects.FlipVertically, 0);
            }
            else
            {
                m_spriteBatch.Draw(m_playerTex[currentPlayerFrame], rectangle, null, Color.White, (float)(Math.PI / 2 * m_playerFacing), new Vector2(m_playerTex[currentPlayerFrame].Width / 2, m_playerTex[currentPlayerFrame].Height / 2),
                    SpriteEffects.None, 0);
            }
        }

        private void playNoise()
        {
            if (nextNoiseTimer <= 0)
            {
                noise[currentNoiseSound].Play();
                currentNoiseSound += 1;
                if (currentNoiseSound > 1)
                {
                    currentNoiseSound = 0;
                }

                nextNoiseTimer = 0.2;
            }

        }
        private void onMoveLeft(GameTime gameTime, float scale)
        {
            playNoise();
            m_playerFacing = 2;

            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (MAP[pos.Item1 - 1, pos.Item2] != 1)
                {
                    pos = (pos.Item1 - 1, pos.Item2);

                }
            }
        }

        private void onMoveRight(GameTime gameTime, float scale)
        {
            m_playerFacing = 0;
            playNoise();

            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (MAP[pos.Item1 + 1, pos.Item2] != 1)
                {
                    pos = (pos.Item1 + 1, pos.Item2);

                }
            }
        }

        private void onMoveUp(GameTime gameTime, float scale)
        {

            m_playerFacing = 3;
            playNoise();
            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (pos.Item2 - 1 < 0)
                {
                    pos = (pos.Item1, MAP.GetLength(1) - 1);
                }
                else
                if (MAP[pos.Item1, pos.Item2 - 1] != 1)
                {
                    pos = (pos.Item1, pos.Item2 - 1);

                }
            }
        }

        private void onMoveDown(GameTime gameTime, float scale)
        {
            playNoise();
            m_playerFacing = 1;

            if (moveLimit < 0)
            {
                moveLimit = SPEED;
                if (pos.Item2 + 1 > MAP.GetLength(1) - 1)
                {
                    pos = (pos.Item1, 0);
                }
                else
                if (MAP[pos.Item1, pos.Item2 + 1] != 1)
                {
                    pos = (pos.Item1, pos.Item2 + 1);

                }
            }

        }

        public void playerAnimation(GameTime gameTime)
        {
            nextPlayerFrame -= gameTime.ElapsedGameTime.TotalSeconds;
            if (nextPlayerFrame <= 0)
            {
                currentPlayerFrame += 1;
                if (currentPlayerFrame >= 3)
                {
                    currentPlayerFrame = 0;
                }
                nextPlayerFrame = 0.2;
            }
        }



    }
}
