using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QColonFrame;
using System;
using System.Collections.Generic;

namespace Client.Scenes
{
    internal class DummyScene : QCScene
    {
        private List<DungeonEntity> _entities;
        private FastNoiseLite _noise;
        private float _noiseOffsetX = 0f;
        private float _noiseOffsetY = 0f;

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

            base.Initialize();
        }

        public override void LoadContent()
        {
            var texture = Content.Load<Texture2D>("missing");
            _entities.ForEach(e => e.Texture = texture);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float noiseScale = 2;

            _noiseOffsetX += 0.2f; // Geschwindigkeit der Rauschbewegung in X
            _noiseOffsetY += 0.2f; // Geschwindigkeit der Rauschbewegung in Y

            foreach (var e in _entities)
            {
                float x = e.Position.X / 4 + _noiseOffsetX;
                float y = e.Position.Y / 4 + _noiseOffsetY;

                var noiseValue = _noise.GetNoise(x * noiseScale, y * noiseScale, (float)gameTime.TotalGameTime.TotalSeconds * 10);

                //e.Tint = noiseValue > 0.1f ? Color.White : Color.Black;

                if (noiseValue < -0.6) // Tiefer Ozean
                {
                    e.Tint = Color.MidnightBlue;
                }
                else if (noiseValue < -0.2) // Flacher Ozean
                {
                    e.Tint = Color.DodgerBlue;
                }
                else if (noiseValue < 0) // Küstenbereich
                {
                    e.Tint = Color.LightSkyBlue;
                }
                else if (noiseValue < 0.2) // Strand
                {
                    e.Tint = Color.LightGoldenrodYellow;
                }
                else if (noiseValue < 0.4) // Grasland
                {
                    e.Tint = Color.LawnGreen;
                }
                else if (noiseValue < 0.6) // Wald
                {
                    e.Tint = Color.ForestGreen;
                }
                else if (noiseValue < 0.8) // Hügelland
                {
                    e.Tint = Color.SaddleBrown;
                }
                else // Berggipfel
                {
                    e.Tint = Color.LightGray;
                }

                //e.Tint = Color.White * noiseValue;
            }

            base.Update(gameTime);
        }

        public override void Draw(QCRenderContext context, GameTime gameTime)
        {
            _entities.ForEach(e => e.Draw(context, gameTime));
            base.Draw(context, gameTime);
        }
    }
}
