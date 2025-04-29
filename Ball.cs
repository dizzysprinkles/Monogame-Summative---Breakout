using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Summative___Breakout
{
    public class Ball
    {
        private Rectangle _location, _window;
        private List<Rectangle> _hitBricks;
        private List<int> _damagedBricks;
        private Vector2 _speed;
        private Texture2D _texture;

        public Ball(Rectangle location, Vector2 speed, Texture2D texture, Rectangle window)
        {
            _location = location;
            _speed = speed;
            _texture = texture;
            _window = window;
            _hitBricks = new List<Rectangle>();
            _damagedBricks = new List<int>();
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

        public List<int> DamagedBricks
        {
            get { return _damagedBricks; }
        }



        public void Update(List<Rectangle> bricks, Paddle paddle, List<Texture2D> brickTextures, Rectangle powerUpRect, bool powerUp) 
        {
            _hitBricks.Clear();
            _damagedBricks.Clear();

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
                    }
                }

            }

            if (futureY.Intersects(powerUpRect) && powerUp == true)
            {
                bouncedY = true;
            }



            if ( futureY.Top < 0)
                bouncedY = true;

            if (futureY.Bottom > _window.Bottom)
            {
                _speed = Vector2.Zero;
            }
         
            if (futureY.Intersects(paddle.Bounds))
            {
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
                    }
                }
            }

            if (futureX.Intersects(powerUpRect) && powerUp == true)
            {
                bouncedX = true;
            }

            if (futureX.Right > _window.Right || futureX.Left < 0)
                bouncedX = true;

            if (futureX.Intersects(paddle.Bounds))
            {
                bouncedX = true;
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

        //public bool Contains(Rectangle paddle)
        //{
        //    return _location.Contains(paddle);
        //}


    }
}
