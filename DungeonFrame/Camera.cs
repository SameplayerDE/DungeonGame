using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QColonFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    public class Camera : QCCamera
    {
        private readonly Viewport _viewport;
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Rectangle? Limits { get; set; }

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            Zoom = 1f;
            Rotation = 0f;
            Position = Vector2.Zero + new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        }

        public void Update(GameTime gameTime)
        {
            // Hier können Animations-Updates oder glatte Übergänge implementiert werden.

            Zoom = Math.Clamp(Zoom, 0.05f, 100f);



            if (Limits.HasValue)
            {
                AdjustZoomWithinLimits();
                var cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(GetViewMatrix()));
                var cameraSize = new Vector2(_viewport.Width / Zoom, _viewport.Height / Zoom);
                var limitWorldMin = new Vector2(Limits.Value.Left, Limits.Value.Top);
                var limitWorldMax = new Vector2(Limits.Value.Right, Limits.Value.Bottom);
                var positionOffset = Position - cameraWorldMin;
                Position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
            }
        }

        private void AdjustZoomWithinLimits()
        {
            var cameraWorldSize = new Vector2(_viewport.Width / Zoom, _viewport.Height / Zoom);
            var worldSize = new Vector2(Limits.Value.Width, Limits.Value.Height);

            // Stelle sicher, dass der gezoomte Bereich nicht größer als die Welt ist
            if (cameraWorldSize.X > worldSize.X)
            {
                Zoom = _viewport.Width / worldSize.X;
            }

            if (cameraWorldSize.Y > worldSize.Y)
            {
                Zoom = _viewport.Height / worldSize.Y;
            }
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-_viewport.Width * 0.5f, -_viewport.Height * 0.5f, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0.0f));
        }

        public override Matrix GetViewMatrix()
        {
            return GetViewMatrix(Vector2.One);
        }
    }
}
