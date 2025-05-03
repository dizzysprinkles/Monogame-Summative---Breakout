using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Monogame_Summative___Breakout
{
    public class Ball
    {
        private Rectangle _location, _window;
        private List<Rectangle> _hitBricks;
        private Vector2 _speed;
        private Texture2D _texture;
        private List<bool> _powerUp;

        public Ball(Rectangle location, Vector2 speed, Texture2D texture, Rectangle window)
        {
            _location = location;
            _speed = speed;
            _texture = texture;
            _window = window;
            _hitBricks = new List<Rectangle>();
            _powerUp = new List<bool>();
            for (int i = 0; i < powerUpRects.Count; i++)
            {
                _powerUp.Add(false);
            }
        }

        public List<bool> PowerUp
        {
            get { return _powerUp; }
        }

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public List<Rectangle> HitBricks
        {
            get { return _hitBricks; }
        }




        public void Update(List<Rectangle> bricks, Paddle paddle, List<Texture2D> brickTextures, SoundEffectInstance bounce, SoundEffectInstance death, SoundEffectInstance powerUp, SoundEffectInstance score, List<Rectangle> powerUpRects, List<bool> powerUps) 
        {
            _hitBricks.Clear();
            _powerUp.Clear();
            for (int i = 0; i < powerUpRects.Count; i++)
            {
                _powerUp.Add(false);
            }

            bool bouncedX = false;
            bool bouncedY = false;
            

            //Check vertical movement
            Rectangle futureY = _location;
            futureY.Offset(0, (int)_speed.Y);

            foreach (Rectangle brick in bricks)
            {
                if (futureY.Intersects(brick))
                {
                    _hitBricks.Add(brick);
                    bouncedY = true;
                    score.Play();
                }

            }

            for (int i = 0; i < powerUpRects.Count; i++)
            {
                if (futureY.Intersects(powerUpRects[i]) && powerUps[i] == true)
                {
                    _powerUp[i] = true;
                    bouncedY = true;
                    powerUp.Play();
                }
            }
           


            if (futureY.Top < 0)
            {
                bounce.Play();
                bouncedY = true;
            }

            if (futureY.Bottom > _window.Bottom)
            {
                death.Play();
                _speed = Vector2.Zero;
                
            }
         
            if (futureY.Intersects(paddle.Bounds))
            {
                bounce.Play();
                bouncedY = true;
            }


            if (bouncedY)
                _speed.Y *= -1;

            _location.Offset(0, (int)_speed.Y); // move after checking

            //Check horizontal movement
            Rectangle futureX = _location;
            futureX.Offset((int)_speed.X, 0);

            foreach (Rectangle brick in bricks)
            {
                if (futureX.Intersects(brick) && !_hitBricks.Contains(brick))
                {
                    _hitBricks.Add(brick);
                    bouncedX = true;
                    score.Play();
                }
            }

            for (int i = 0; i < powerUpRects.Count; i++)
            {
                if (futureX.Intersects(powerUpRects[i]) && powerUps[i] == true)
                {
                    _powerUp[i] = true;
                    bouncedX = true;
                    powerUp.Play();
                }
            }


            if (futureX.Right > _window.Right || futureX.Left < 0)
            {
                bouncedX = true;
                bounce.Play();
            }

            if (futureX.Intersects(paddle.Bounds))
            {
                bouncedX = true;
                bounce.Play();
            }


            if (bouncedX)
            {
                _speed.X *= -1;
            }

            _location.Offset((int)_speed.X, 0); // move after checking

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }

        public bool Intersects(Rectangle bricks)
        {
            return _location.Intersects(bricks);
        }

    }
}
