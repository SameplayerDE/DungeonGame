using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame
{
    public class QCCamera
    {
        private readonly Viewport _viewport;
        public Vector2 Position { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Rectangle? Limits { get; set; }

        public QCCamera(Viewport viewport)
        {
            _viewport = viewport;
            Zoom = 1f;
            Rotation = 0f;
            Position = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            // Hier können Animations-Updates oder glatte Übergänge implementiert werden.

            // Beispiel für eine Kamera-Begrenzung:
            if (Limits.HasValue)
            {
                var cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(GetViewMatrix()));
                var cameraSize = new Vector2(_viewport.Width / Zoom, _viewport.Height / Zoom);
                var limitWorldMin = new Vector2(Limits.Value.Left, Limits.Value.Top);
                var limitWorldMax = new Vector2(Limits.Value.Right, Limits.Value.Bottom);
                var positionOffset = Position - cameraWorldMin;
                Position = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
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

        public Matrix GetViewMatrix()
        {
            return GetViewMatrix(Vector2.One);
        }
    }

}
