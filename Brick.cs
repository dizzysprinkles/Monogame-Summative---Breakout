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
        private List<Color> _colours;
        private int _score;

        public Brick(List<Rectangle> locations, List<Texture2D> textures, List<Color>colours)
        {
            _locations = locations;
            _textures = textures;
            _colours = colours;
            _score = 0;
        }

        public List<Rectangle> GetBricks
        {
            get { return _locations; }
        }

        public List<Texture2D> GetTextures
        {
            get { return _textures; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                spriteBatch.Draw(_textures[i], _locations[i], _colours[i]);
            }
        }

        public void RemoveBricks(List<Rectangle> hitBricks)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                if (hitBricks.Contains(_locations[i]))
                {
                    _score += 2;
                    _locations.RemoveAt(i);
                    _textures.RemoveAt(i);
                    _colours.RemoveAt(i);
                }
            }
        }

    }
}
