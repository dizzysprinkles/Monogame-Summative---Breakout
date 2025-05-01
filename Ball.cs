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
        private List<int> _damagedBricks;
        private Vector2 _speed;
        private Texture2D _texture;
        private bool _fast, _slow;

        public Ball(Rectangle location, Vector2 speed, Texture2D texture, Rectangle window)
        {
            _location = location;
            _speed = speed;
            _texture = texture;
            _window = window;
            _hitBricks = new List<Rectangle>();
            _damagedBricks = new List<int>();
            _fast = false;
            _slow = false;
        }

        public bool Fast
        {
            get { return _fast; }
        }

        public bool Slow
        {
            get { return _slow; }
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

        //public List<int> DamagedBricks
        //{
        //    get { return _damagedBricks; }
        //}



        public void Update(List<Rectangle> bricks, Paddle paddle, List<Texture2D> brickTextures, Rectangle fastRect, bool fast, bool slow, Rectangle slowRect, SoundEffectInstance bounce, SoundEffectInstance death, SoundEffectInstance powerUp, SoundEffectInstance score) 
        {
            _hitBricks.Clear();
            _damagedBricks.Clear();

            _fast = false;
            _slow = false;
            bool bouncedX = false;
            bool bouncedY = false;
            

            //Check vertical movement
            Rectangle futureY = _location;
            futureY.Offset(0, (int)_speed.Y);

            foreach (Rectangle brick in bricks)
            {
                for (int i = 0; i < brickTextures.Count; i++)
                {
                    if (futureY.Intersects(brick))
                    {
                        _damagedBricks.Add(i);
                        _hitBricks.Add(brick);
                        bouncedY = true;
                        score.Play();
                    }
                }

            }

            if (futureY.Intersects(fastRect) && fast == true)
            {
                _fast = true;
                bouncedY = true;
                powerUp.Play();
            }

            if (futureY.Intersects(slowRect) && slow == true)
            {
                _slow = true;
                bouncedY = true;
                powerUp.Play();
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
                for (int i = 0; i < brickTextures.Count; i++)
                {
                    if (futureX.Intersects(brick) && !_hitBricks.Contains(brick))
                    {
                        _damagedBricks.Add(i);
                        _hitBricks.Add(brick);
                        bouncedX = true;
                        score.Play();
                    }
                }
            }

            if (futureX.Intersects(fastRect) && fast == true)
            {
                _fast = true;
                bouncedX = true;
                powerUp.Play();
            }

            if (futureX.Intersects(slowRect) && slow == true)
            {
                _slow = true;
                bouncedX = true;
                powerUp.Play();
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

        //public void Sound(SoundEffectInstance bounce, SoundEffectInstance death, SoundEffectInstance powerUp, SoundEffectInstance score, Paddle paddle, Rectangle slowRect, Rectangle fastRect, List<Rectangle> bricks, bool fast, bool slow)
        //{
        //    if (_location.Right > _window.Right || _location.Left < 0 || _location.Top < 0 || _location.Intersects(paddle.Bounds))
        //    {
        //        bounce.Play();
        //    }

        //    if (_location.Bottom > _window.Bottom)
        //    {
        //        death.Play();
        //    }

        //    if (slow)
        //    {
        //        if (_location.Intersects(slowRect))
        //        {
        //            powerUp.Play();
        //        }
        //    }

        //    if (fast)
        //    {
        //        if (_location.Intersects(fastRect))
        //        {
        //            powerUp.Play();
        //        }
        //    }

        //    foreach (Rectangle brick in bricks)
        //    {
        //        if (_location.Intersects(brick))
        //        {
        //            score.Play();
        //        }
        //    }


        //}

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
