using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private QCRenderContext _context;
        private DungeonEntity _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _context = new QCRenderContext();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _player = new DungeonEntity();
            _player.Flags = DungeonEntityFlags.Drawable;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _context.SpriteBatch = _spriteBatch;

            _player.Texture = Content.Load<Texture2D>("missing");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _player.Draw(_context, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
