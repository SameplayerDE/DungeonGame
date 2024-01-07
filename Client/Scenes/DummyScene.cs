using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;
using QColonFrame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Scenes
{
    internal class DummyScene : QCScene
    {
        private List<DungeonEntity> _entities;
        private DungeonEntity _player;
        private FastNoiseLite _noise;
        private World _world;
        private Texture2D _tileSet;
        private Texture2D _monsterTexture;
        private float _noiseOffsetX = 0f;
        private float _noiseOffsetY = 0f;
        private float currentScrollValue = 0f;
        private QCInputActionHandler _inputActionHandler;

        public DummyScene(Game game) : base("dummy", game)
        {
        }

        public override void Initialize()
        {

            _inputActionHandler = new QCInputActionHandler();
            _inputActionHandler.AddAction(new QCInputAction("exit_game").AddKey(Keys.Left, Keys.A).SetHoldAction(ClientCore.Exit));

            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            _noise.SetSeed(10111999);

            float scale = 4;

            _entities = new List<DungeonEntity>();
            _player = new DungeonEntity();
            _player.Tint = Color.Red;
            _player.Flags = DungeonEntityFlags.Drawable;
            _player.Position = new Vector2(0, 0);

            for (int i = 0; i < 10; i++)
            {
                var monster = new Monster();
                monster.Flags = DungeonEntityFlags.Drawable | DungeonEntityFlags.Updateable;
                monster.Position = new Vector2(new Random().Next(0, 1000), new Random().Next(0, 1000));
                _entities.Add(monster);
            }

            TileAtlas tileAtlas = TileAtlas.LoadTilesFromJson("Assets/dummy.json");
            _world = new World(tileAtlas);

            base.Initialize();
        }

        public override void LoadContent()
        {
            var texture = Content.Load<Texture2D>("missing");
            _monsterTexture = Content.Load<Texture2D>("monster");
            _entities.ForEach(e => e.Texture = _monsterTexture);
            _player.Texture = texture;
            _tileSet = Content.Load<Texture2D>("tileset");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _inputActionHandler.Update(gameTime, ClientCore.Input);

            _entities.ForEach(e => e.Update(gameTime));

            float previousScrollValue = currentScrollValue; // Du musst den vorherigen Scroll-Wert speichern
            currentScrollValue = Mouse.GetState().ScrollWheelValue;
            float zoomSpeed = 0.001f; // Geschwindigkeit des Zooms, kannst du anpassen
            ((Camera)QCSceneHandler.Instance.RenderContext.Camera).Zoom += (currentScrollValue - previousScrollValue) * zoomSpeed;
            //QCSceneHandler.Instance.RenderContext.Camera.Rotation += (currentScrollValue - previousScrollValue) * zoomSpeed;

            Vector2 cameraMove = Vector2.Zero;
            float moveSpeed = 20f; // Bewegungsgeschwindigkeit, kannst du anpassen

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                cameraMove.Y -= moveSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                cameraMove.Y += moveSpeed;      
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                cameraMove.X -= moveSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                cameraMove.X += moveSpeed;

            _player.Position += cameraMove;

            ((Camera)QCSceneHandler.Instance.RenderContext.Camera).Position = _player.Position - ((Camera)QCSceneHandler.Instance.RenderContext.Camera).Viewport.Bounds.Center.ToVector2();

            int chunkX = (int)Math.Floor(_player.Position.X / (Chunk.Width * 64));
            int chunkY = (int)Math.Floor(_player.Position.Y / (Chunk.Height * 32));

            Console.WriteLine(_player.Position.X);
            Console.WriteLine(_player.Position.Y);
            Console.WriteLine(chunkX);
            Console.WriteLine(chunkY);
            Console.WriteLine("------");

            //Unload all other

            int distance = 10;

            foreach (var chunkKey in _world.Chunks.Keys.ToList())
            {
                if (Math.Abs(chunkKey.Item1 - chunkX) > distance || Math.Abs(chunkKey.Item2 - chunkY) > distance)
                {
                    _world.UnloadChunkAsync(chunkKey.Item1, chunkKey.Item2);
                }
            }

            for (int i = chunkX - distance; i <= chunkX + distance; i++)
            {
                for (int j = chunkY - distance; j <= chunkY + distance; j++)
                {
                    if (!_world.Chunks.ContainsKey((i, j)))
                    {
                        _world.LoadChunkAsync(i, j);
                    }
                }
            }
        }

        public override void Draw(QCRenderContext context, GameTime gameTime)
        {
             WorldRenderer.Instance.Draw(context, gameTime, _world, _tileSet);
            _player.Draw(context, gameTime);
            _entities.ForEach(e => e.Draw(context, gameTime));

            base.Draw(context, gameTime);
        }

        private Vector2 ToIsometric(Vector2 position)
        {
            float isoX = (Math.Abs(position.X - position.Y));
            float isoY = (Math.Abs(position.X) + Math.Abs(position.Y)) / 2;

            // Richtung der Originalkoordinaten berücksichtigen
            isoX *= Math.Sign(position.X);
            isoY *= Math.Sign(position.Y);

            return new Vector2(isoX, isoY);
        }
    }
}
