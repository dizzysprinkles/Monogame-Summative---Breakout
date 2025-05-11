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
            for (int i = 0; i <= 5; i++)
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

        public Rectangle Location
        {
            get { return _location; }
            set { _location = value; }
        }


        public void Update(List<Rectangle> bricks, Paddle paddle, SoundEffectInstance bounce, SoundEffectInstance death, SoundEffectInstance powerUp, SoundEffectInstance score, List<Rectangle> powerUpRects, List<bool> powerUps)
        {
            _hitBricks.Clear();
            

            // Move the ball
            _location.Offset((int)_speed.X, (int)_speed.Y);

            // Check for window collisions
            if (_location.Left <= _window.Left || _location.Right >= _window.Right)
            {
                _speed.X *= -1;
                bounce.Play();
            }

            if (_location.Top <= _window.Top)
            {
                bounce.Play();
                _speed.Y *= -1;
                _location.Y = _window.Top;
            }

            // Check for bottom collision 
            if (_location.Bottom >= _window.Bottom)
            {
                death.Play();
                _speed = Vector2.Zero;
            }

            // Check for paddle collision
            if (_location.Intersects(paddle.Bounds))
            {
                Rectangle overlap = Rectangle.Intersect(_location, paddle.Bounds);

                if (overlap.Width < overlap.Height)
                {
                    if (_location.Center.X < paddle.Bounds.Center.X)
                    {
                        _location.X = paddle.Bounds.Left - _location.Width;
                        _speed.X = -Math.Abs(_speed.X);
                    }
                    else
                    {
                        _location.X = paddle.Bounds.Right;
                        _speed.X = Math.Abs(_speed.X);
                    }
                }
                else
                {
                    _location.Y = paddle.Bounds.Top - _location.Height;
                    _speed.Y *= -1;
                }
                bounce.Play();
            }

            // Check for brick collisions
            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                if (_location.Intersects(bricks[i]))
                {
                    Rectangle overlap = Rectangle.Intersect(_location, bricks[i]);

                    if (overlap.Width < overlap.Height)
                    {
                        _speed.X *= -1;
                    }
                    else
                    {
                        _speed.Y *= -1;
                    }

                    _hitBricks.Add(bricks[i]);
                    score.Play();
                    break; // one brick collision per update
                }
            }

            for (int i = 0; i < powerUpRects.Count; i++)
            {
                if (_location.Intersects(powerUpRects[i]) && powerUps[i] == true)
                {
                    Rectangle overlap = Rectangle.Intersect(_location, powerUpRects[i]);

                    if (overlap.Width >= overlap.Height)
                    {
                        _speed.Y *= -1;

                    }
                    else
                    {
                        _speed.X *= -1;
                    }
                    _powerUp[i] = true;
                    powerUp.Play();
                }
            }
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
