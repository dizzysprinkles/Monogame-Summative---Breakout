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
        
        private Vector2 _speed;
        private Texture2D _texture;

        public Ball(Rectangle location, Vector2 speed, Texture2D texture, Rectangle window)
        {
            _location = location;
            _speed = speed;
            _texture = texture;
            _window = window;
        }

        public Rectangle Bounds
        {
            get { return _location; }
            set { _location = value; }
        }


        public void Update(List<Rectangle> bricks, Paddle paddle) 
        {
            bool bouncedX = false;
            bool bouncedY = false;

            //Check vertical movement
            Rectangle futureY = _location;
            futureY.Offset(0, (int)_speed.Y);

            foreach (Rectangle brick in bricks)
            {
                if (futureY.Intersects(brick))
                {
                    bouncedY = true;
                    break;
                }
            }

            if (futureY.Bottom > _window.Bottom || futureY.Top < 0)
                bouncedY = true;

         
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
                if (futureX.Intersects(brick))
                {
                    bouncedX = true;
                    break;
                }
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




        //public List<Rectangle> GetCollidingBricks(List<Rectangle> bricks)
        //{
        //    return bricks.Where(brick => _location.Intersects(brick)).ToList();
        //}

    }
}
