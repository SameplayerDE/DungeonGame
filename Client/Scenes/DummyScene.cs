using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;
using System;
using System.Collections.Generic;

namespace Client.Scenes
{
    internal class DummyScene : QCScene
    {
        private List<DungeonEntity> _entities;
        private FastNoiseLite _noise;
        private World _world;
        private Texture2D _tileSet;
        private float _noiseOffsetX = 0f;
        private float _noiseOffsetY = 0f;
        private float currentScrollValue = 0f;

        public DummyScene(Game game) : base("dummy", game)
        {
        }

        public override void Initialize()
        {
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            _noise.SetSeed(10111999);

            float scale = 4;

            _entities = new List<DungeonEntity>();

            for (float y = 0; y <= Game.Window.ClientBounds.Height / scale; y += 1)
            {
                for (float x = 0; x <= Game.Window.ClientBounds.Width / scale; x += 1)
                {
                    var entity = new DungeonEntity();
                    entity.Flags = DungeonEntityFlags.Drawable;
                    entity.Position = new Vector2(x * scale, y * scale);
                    _entities.Add(entity);
                }
            }

            _world = new World(100, 100, TileAtlas.LoadTilesFromJson("Assets/dummy.json"));
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    // Setze alle Tiles auf die ID 0, welche später in Update() aktualisiert werden.
                    _world.Set(x, y, 0);
                }
            }

            base.Initialize();
        }

        public override void LoadContent()
        {
            var texture = Content.Load<Texture2D>("missing");
            _entities.ForEach(e => e.Texture = texture);
            _tileSet = Content.Load<Texture2D>("tileset");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float noiseScale = 4;

            _noiseOffsetX += 0.1f; // Geschwindigkeit der Rauschbewegung in X
            _noiseOffsetY += 0.1f; // Geschwindigkeit der Rauschbewegung in Y

            for (int y = 0; y < _world.Height; y++)
            {
                for (int x = 0; x < _world.Width; x++)
                {
                    float noiseValue = _noise.GetNoise(
                        (x + _noiseOffsetX) * noiseScale,
                        (y + _noiseOffsetY) * noiseScale,
                        (float)gameTime.TotalGameTime.TotalSeconds * 10);

                    int tileId;
                    if (noiseValue < -0.6) tileId = 7; // ID für Tiefer Ozean
                    else if (noiseValue < -0.2) tileId = 0; // ID für Flacher Ozean
                    else if (noiseValue < 0) tileId = 1; // ID für Küstenbereich
                    else if (noiseValue < 0.2) tileId = 2; // ID für Strand
                    else if (noiseValue < 0.4) tileId = 3; // ID für Grasland
                    else if (noiseValue < 0.6) tileId = 4; // ID für Wald
                    else if (noiseValue < 0.8) tileId = 5; // ID für Hügelland
                    else tileId = 6; // ID für Berggipfel

                    _world.Set(x, y, tileId);
                }
            }

            
            float previousScrollValue = currentScrollValue; // Du musst den vorherigen Scroll-Wert speichern
            currentScrollValue = Mouse.GetState().ScrollWheelValue;
            float zoomSpeed = 0.001f; // Geschwindigkeit des Zooms, kannst du anpassen
            QCSceneHandler.Instance.RenderContext.Camera.Zoom += (currentScrollValue - previousScrollValue) * zoomSpeed;


            Vector2 cameraMove = Vector2.Zero;
            float moveSpeed = 5f; // Bewegungsgeschwindigkeit, kannst du anpassen

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                cameraMove.Y -= moveSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                cameraMove.Y += moveSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                cameraMove.X -= moveSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                cameraMove.X += moveSpeed;

            QCSceneHandler.Instance.RenderContext.Camera.Position += cameraMove;


        }

        public override void Draw(QCRenderContext context, GameTime gameTime)
        {
            _entities.ForEach(e => e.Draw(context, gameTime));
            WorldRenderer.Instance.Draw(context, gameTime, _world, _tileSet);

            base.Draw(context, gameTime);
        }
    }
}
