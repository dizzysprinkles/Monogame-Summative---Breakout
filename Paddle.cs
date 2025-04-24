using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Summative___Breakout
{
    public class Paddle
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Rectangle _window;
        private Vector2 _speed;

        public Paddle(Rectangle location, Texture2D texture, Rectangle window)
        {
            _location = location;
            _speed = Vector2.Zero;
            _texture = texture;
            _window = window;
        }


        public Rectangle Bounds
        {
            get { return _location; }
            set { _location = value; }
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            _speed = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A))
            {
                _speed.X = -2;
            }
            if (_location.X < 0)
            {
                _speed.X += 2;
            }

            else if (keyboardState.IsKeyDown(Keys.D))
            {
                _speed.X = 2;
            }
            if (_location.X > _window.Width - _location.Width)
            {
                _speed.X -= 2;
            }

            _location.Offset(_speed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }


    }
}
