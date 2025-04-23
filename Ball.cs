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

        //Have to fix offsetting speed - watch game to see why
        public void Update(List<Rectangle> bricks) 
        {
            BounceVertical(bricks);
            _location.Offset(0, _speed.Y); // apply seperate x and y speeds to detect collisions seperate (top/bottom = y) (left/right = x)
            //BounceVertical(bricks);

            BounceHorizontal(bricks);
            _location.Offset(_speed.X, 0);

            //BounceHorizontal(bricks);


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }

        public void BounceVertical(List<Rectangle> bricks)
        {

            if (_location.Y > _window.Height - _location.Height )
            {
                _speed.Y*=-1;
            }
            else if (_location.Y < 0)
            {
                _speed.Y*=-1;
            }

            for (int i = 0; i < bricks.Count; i++) //might hit two bricks - both have to disappear  
            {
                if (Intersects(bricks[i]))
                {
                    _speed.Y *= -1;
                }
            }
        }

        public void BounceHorizontal(List<Rectangle>bricks)
        {
            if (_location.X > _window.Width - _location.Width)
            {
                _speed.X *= -1;
            }
            else if (_location.X < 0)
            {
                _speed.X *= -1;
            }

            for (int i = 0; i < bricks.Count; i++) //might hit two bricks - both have to disappear  
            {
                if (Intersects(bricks[i]))
                {
                    _speed.X *= -1;
                }
            }


        }

        public bool Intersects(Rectangle bricks)
        {
            return _location.Intersects(bricks);
        }

    }
}
