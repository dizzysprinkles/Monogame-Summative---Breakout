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
        private List<Color> _colours;  // Need to add random colour generation in draw so it's less monotone
        private Random _generator;

        public Brick(List<Rectangle> locations, List<Texture2D> textures, List<Color>colours)
        {
            _locations = locations;
            _textures = textures;
            _generator = new Random();
            _colours = colours;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

            for (int i = 0; i < _locations.Count; i++)
            {
                spriteBatch.Draw(_textures[i], _locations[i], _colours[i]);
            }
        }

    }
}
