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
    internal class Paddle
    {
        private Texture2D _texture;
        private Rectangle _location;
        private Vector2 _speed;

        public Paddle(Rectangle location, Texture2D texture)
        {
            _location = location;
            _speed = Vector2.Zero;
            _texture = texture;
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            _speed = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A) && prevKeyboardState.IsKeyUp(Keys.A))
            {
                _speed.X = -1;
            }

            else if (keyboardState.IsKeyDown(Keys.D) && prevKeyboardState.IsKeyUp(Keys.D))
            {
                _speed.X = 1;
            }

            _location.Offset(_speed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, Color.White);
        }


    }
}
