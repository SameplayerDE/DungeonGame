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


        private QCRenderContext _context;
        private Camera _camera;
        private DungeonEntity _player;
        private World _world;
        private Texture2D _tileSet;

        int[,] example = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
            { 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0},
            { 0, 0, 1, 2, 2, 3, 2, 2, 1, 0, 0},
            { 0, 0, 1, 2, 3, 3, 3, 2, 1, 0, 0},
            { 0, 0, 1, 2, 2, 3, 2, 2, 1, 0, 0},
            { 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0},
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        };

        QCModelSynthesis modelSynthesis = new QCModelSynthesis(250, 250);


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

            modelSynthesis.Learn(example);
            modelSynthesis.Collapse();

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);
            //QCSceneHandler.Instance.Draw(gameTime);
            //GraphicsDevice.SetRenderTarget(null);

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
            //    DepthStencilState.Default, RasterizerState.CullCounterClockwise, effect: _blur);
            //_spriteBatch.Draw(_renderTarget, RenderTargetRectangle, Color.White);
            //_spriteBatch.End();

            _spriteBatch.Begin();
            
            for (int x = 0; x < modelSynthesis.Width; x++)
            {
                for (int y = 0; y < modelSynthesis.Height; y++)
                {
                    int id = modelSynthesis.Result[x, y];
            
                    // Wähle die Farbe basierend auf der ID
                    Color tileColor;
                    switch (id)
                    {
                        case 0:
                            tileColor = Color.Blue;
                            break;
                        case 1:
                            tileColor = Color.Yellow;
                            break;
                        case 2:
                            tileColor = Color.Green;
                            break;
                        case 3:
                            tileColor = Color.Gray;
                            break;
                        default:
                            tileColor = Color.White; // Standardfarbe für unbekannte IDs
                            break;
                    }
            
                    _spriteBatch.Draw(_tileSet, new Rectangle(x * 4, y * 4, 4, 4), tileColor);
                }
            }
            
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
