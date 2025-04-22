using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        Rectangle window;
        Screen screenState;
        Texture2D titleBackgroundTexture, tutorialBackgroundTexture, mainBackgroundTexture, endBackgroundTexture;
        KeyboardState currentKeyboardState, prevKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenState = Screen.Title;
            window = new Rectangle(0,0,800,600);
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            tutorialBackgroundTexture = Content.Load<Texture2D>("Images/tutorialBackground");
            mainBackgroundTexture = Content.Load<Texture2D>("Images/mainBackground");
            endBackgroundTexture = Content.Load<Texture2D>("Images/endBackground");
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
