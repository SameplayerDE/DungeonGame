using Client.Scenes;
using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;
using QColonUtils.Algorithmes.ModelSynthesis;
using System;

namespace Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private RenderTarget2D _gameplayRenderTarget; //world, map, entities
        private RenderTarget2D _overlayRenderTarget; //minimap
        private RenderTarget2D _interfaceRenderTarget; //user interface

        private QCRenderContext _context;
        private Camera _camera;
        private DungeonEntity _player;
        private World _world;
        private Texture2D _tileSet;

        private int _width = 1280;
        private int _height = 720;
        private Rectangle _renderWindow;

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

            // Initialisiere das Gameplay Render Target
            _gameplayRenderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                _width, // Breite
                _height, // Höhe
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            // Initialisiere das Overlay (Minimap) Render Target
            _overlayRenderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                _width, // Breite
                _height, // Höhe
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            // Initialisiere das Interface Render Target
            _interfaceRenderTarget = new RenderTarget2D(
                _graphics.GraphicsDevice,
                _width, // Breite
                _height, // Höhe
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.DiscardContents);

            _camera = new Camera(GraphicsDevice.Viewport);
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
            _tileSet = Content.Load<Texture2D>("missing");
        }

        protected override void Update(GameTime gameTime)
        {
            ClientCore.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.Update(gameTime);
            QCSceneHandler.Instance.Update(gameTime);

            _renderWindow = new Rectangle(0, 0, _width / 2, _height / 2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(_gameplayRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            QCSceneHandler.Instance.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(_overlayRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            //QCSceneHandler.Instance.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(_interfaceRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            //QCSceneHandler.Instance.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_gameplayRenderTarget, _renderWindow, Color.White);
            _spriteBatch.Draw(_overlayRenderTarget, _renderWindow, Color.White);
            _spriteBatch.Draw(_interfaceRenderTarget, _renderWindow, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
