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
        private List<Texture2D> _textures;
        private Rectangle _location;
        private Rectangle _window;
        private Vector2 _speed;
        private int _textureIndex;
        private float _seconds;
        private float _animationSpeed;


        public Paddle(Rectangle location, List<Texture2D> textures, Rectangle window)
        {
            _location = location;
            _speed = Vector2.Zero;
            _textureIndex = 0;
            _textures = textures;
            _window = window;
            _seconds = 0f;
            _animationSpeed = 0.2f;
        }


        public Rectangle Bounds
        {
            get { return _location; }
            set { _location = value; }
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            _speed = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                _speed.X = -4;
            }
            if (_location.X < 0)
            {
                _speed.X += 4;
            }

            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _speed.X = 4;
            }
            if (_location.X > _window.Width - _location.Width)
            {
                _speed.X -= 4;
            }

            //Animation
            _seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_seconds > _animationSpeed)
            {
                _seconds = 0;
                _textureIndex++;
                if (_textureIndex >= _textures.Count)
                {
                    _textureIndex = 1;
                }
            }

            _location.Offset(_speed);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textures[_textureIndex], _location, Color.White);
        }

    }
}
