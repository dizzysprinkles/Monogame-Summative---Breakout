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

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public void Update(Rectangle bricks) //Need to change to list
        {
            Bounce(bricks);

            _location.Offset(_speed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }

        public void Bounce(Rectangle bricks)
        {
            if (_location.X > _window.Width - _location.Width)
            {
                _speed.X*=-1;
            }
            else if (_location.X < 0)
            {
                _speed.X*=-1;
            }

            if (_location.Y > _window.Height - _location.Height )
            {
                _speed.Y*=-1;
            }
            else if (_location.Y < 0)
            {
                _speed.Y*=-1;
            }

            if (Intersects(bricks))
            {
                _speed.Y *= -1;
            }
        }

        public bool Intersects(Rectangle bricks)
        {
            return _location.Intersects(bricks);
        }

    }
}
