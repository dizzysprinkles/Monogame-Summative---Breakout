using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Summative___Breakout
{
    public class Brick
    {
        private List<Rectangle> _locations;
        private List<Texture2D> _textures;
        //private Color _color;  // Need to add random colour generation in draw so it's less monotone
        private Random _generator;

        public Brick(List<Rectangle> locations, List<Texture2D> textures)
        {
            _locations = locations;
            _textures = textures;
            _generator = new Random();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                spriteBatch.Draw(_textures[i], _locations[i], Color.White);
            }
        }

        public void RemoveBricks(Rectangle ball)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                if (ball.Intersects(_locations[i]))
                {
                    _locations.RemoveAt(i);
                    _textures.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
