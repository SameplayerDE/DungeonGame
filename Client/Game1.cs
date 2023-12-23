using Client.Scenes;
using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;
using System;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        private QCRenderContext _context;
        private QCCamera _camera;
        private DungeonEntity _player;
        private World _world;
        private Texture2D _tileSet;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _context = new QCRenderContext();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            ClientCore.SetGame(this);
        }

        protected override void Initialize()
        {

            _camera = new QCCamera(GraphicsDevice.Viewport);
            _player = new DungeonEntity();
            _player.Flags = DungeonEntityFlags.Drawable;

            QCSceneHandler.Instance.RenderContext = _context;

            QCSceneHandler.Instance.Add(new DummyScene(this));
            QCSceneHandler.Instance.Initialize();

            QCSceneHandler.Instance.Stage("dummy");
            QCSceneHandler.Instance.Grab();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _context.SpriteBatch = _spriteBatch;
            _context.Camera = _camera;

            _player.Texture = Content.Load<Texture2D>("missing");
        }

        protected override void Update(GameTime gameTime)
        {
            ClientCore.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);
            QCSceneHandler.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);
            QCSceneHandler.Instance.Draw(gameTime);
            //GraphicsDevice.SetRenderTarget(null);

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            //    DepthStencilState.Default, RasterizerState.CullCounterClockwise, effect: _blur);
            //_spriteBatch.Draw(_renderTarget, RenderTargetRectangle, Color.White);
            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
