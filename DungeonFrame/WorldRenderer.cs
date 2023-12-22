using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QColonFrame;
using System;

namespace DungeonFrame
{
    public class WorldRenderer
    {
        public static WorldRenderer Instance { get; } = new WorldRenderer();

        public static int RenderCalls = 0;

        private WorldRenderer()
        {
        }

        public void Draw(QCRenderContext context, GameTime gameTime, World world, Texture2D tileSet)
        {
            RenderCalls = 0;
            int tileWidth = 64;
            int tileHeight = 32;

            // Ermitteln des sichtbaren Bereichs
            var cameraViewMatrix = context.Camera.GetViewMatrix();
            var inverseViewMatrix = Matrix.Invert(cameraViewMatrix);
            var topLeft = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var bottomRight = Vector2.Transform(new Vector2(context.SpriteBatch.GraphicsDevice.Viewport.Width, context.SpriteBatch.GraphicsDevice.Viewport.Height), inverseViewMatrix);

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    // Isometrische Positionierung auf dem Bildschirm
                    int isoX = (x - y) * (tileWidth / 2);
                    int isoY = (x + y) * (tileHeight / 2);

                    // Prüfen, ob das Tile im sichtbaren Bereich liegt
                    if (isoX > topLeft.X - tileWidth && isoX < bottomRight.X && isoY > topLeft.Y - tileHeight && isoY < bottomRight.Y)
                    {
                        Tile tile = world.GetTile(x, y);
                        if (tile != null)
                        {
                            int tileDepth = tile.Depth;

                            // Berechnung der source Rectangle, basierend auf der Tile-ID
                            int tileRow = tile.Id / (tileSet.Width / tileWidth);
                            int tileColumn = tile.Id % (tileSet.Width / tileWidth);
                            Rectangle source = new Rectangle(tileColumn * tileWidth, tileRow * (tileHeight + tileDepth), tileWidth, tileHeight + tileDepth);

                            // Anpassung für die Tiefe
                            isoY -= tileDepth;

                            Rectangle destination = new Rectangle(isoX, isoY, tileWidth, tileHeight + tileDepth);
                            context.SpriteBatch.Draw(tileSet, destination, source, Color.White);
                            RenderCalls++;
                        }
                    }
                }
            }
            Console.WriteLine("RenderCalls: " + RenderCalls);
        }
    }
}
