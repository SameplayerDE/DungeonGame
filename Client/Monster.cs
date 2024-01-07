using DungeonFrame;
using Microsoft.Xna.Framework;
using System;

namespace Client
{
    public class Monster : DungeonEntity
    {
        private Vector2 _direction;
        private float _moveTimer;
        private float _moveInterval = 1.0f; // Zeit in Sekunden, bevor sich die Richtung ändert
        private Random _random = new Random();
        private float _flutterAmplitude = 1.0f; // Wie stark das Monster flattert
        private float _flutterFrequency = 100.0f; // Wie schnell das Monster flattert

        public Monster()
        {
            _direction = GetRandomDirection();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Flags.HasFlag(DungeonEntityFlags.Updateable))
            {
                return;
            }

            _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_moveTimer > _moveInterval)
            {
                _direction = GetRandomDirection();
                _moveTimer = 0;
            }

            // Flatterbewegung
            float flutter = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * _flutterFrequency) * _flutterAmplitude;

            Position += _direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(Position.X, Position.Y + flutter);
        }

        private Vector2 GetRandomDirection()
        {
            float angle = (float)_random.NextDouble() * MathHelper.TwoPi;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 100; // Nur horizontale Bewegung
        }
    }
}
