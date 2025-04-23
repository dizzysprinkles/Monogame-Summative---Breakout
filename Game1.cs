using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace Monogame_Summative___Breakout
{
    enum Screen
    {
        Title,
        Tutorial,
        Main,
        End
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle window, ballRect;
        Screen screenState;
        Texture2D titleBackgroundTexture, tutorialBackgroundTexture, mainBackgroundTexture, endBackgroundTexture, ballTexture;
        KeyboardState currentKeyboardState, prevKeyboardState;
        Ball ball;
        Brick bricks;
        Vector2 ballSpeed;

        List<Rectangle> brickRects;
        List<Texture2D> brickTextures;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenState = Screen.Main;  // NEED TO CHANGE

            brickRects = new List<Rectangle>();
            brickTextures = new List<Texture2D>();

            window = new Rectangle(0,0,800,600);

            ballRect = new Rectangle(400, 500, 25, 25);
            ballSpeed = new Vector2(2, 2);

            for (int x = 0; x < window.Width; x += 100)
            {
                for (int y = 0; y < 363; y += 33)
                {
                    brickRects.Add(new Rectangle(x, y, 100, 33));
                }
            }
           
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            base.Initialize();

            ball = new Ball(ballRect, ballSpeed, ballTexture, window);
            bricks = new Brick(brickRects, brickTextures);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < brickRects.Count; i++)
            {
                brickTextures.Add(Content.Load<Texture2D>("Images/brick_1"));
            }

            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            tutorialBackgroundTexture = Content.Load<Texture2D>("Images/tutorialBackground");
            mainBackgroundTexture = Content.Load<Texture2D>("Images/mainBackground");
            endBackgroundTexture = Content.Load<Texture2D>("Images/endBackground");
            ballTexture = Content.Load<Texture2D>("Images/ball");
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (screenState == Screen.Title)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                {
                    screenState = Screen.Tutorial;
                }
            }
            else if (screenState == Screen.Tutorial)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {
                    screenState = Screen.Main;
                }
            }
            else if (screenState == Screen.Main)
            {
                ball.Update(brickRects);
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                {
                    screenState = Screen.End;
                }
            }
            else
            {
                if (currentKeyboardState.IsKeyDown(Keys.Q) && prevKeyboardState.IsKeyUp(Keys.Q))
                {
                    Exit();
                }
            }

            
            prevKeyboardState = currentKeyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if (screenState == Screen.Title)
            {
                _spriteBatch.Draw(titleBackgroundTexture, window, Color.White);
            }
            else if (screenState == Screen.Tutorial)
            {
                _spriteBatch.Draw(tutorialBackgroundTexture, window, Color.White);
            }
            else if (screenState == Screen.Main)
            {
                _spriteBatch.Draw(mainBackgroundTexture, window, Color.White);

                for (int i = 0; i < brickRects.Count; i++)
                {
                    bricks.Draw(_spriteBatch);
                }

                ball.Draw(_spriteBatch);
            }
            else
            {
                _spriteBatch.Draw(endBackgroundTexture, window, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
