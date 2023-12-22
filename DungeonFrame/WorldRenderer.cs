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

        private WorldRenderer() { }

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

            // Durchlaufe alle Chunks
            foreach (var chunkEntry in world.Chunks)
            {
                var chunk = chunkEntry.Value;
                // Berechne globale Position des Chunks
                int chunkGlobalX = chunk.X * world.ChunkSize;
                int chunkGlobalY = chunk.Y * world.ChunkSize;

                for (int localY = 0; localY < chunk.Height; localY++)
                {
                    for (int localX = 0; localX < chunk.Width; localX++)
                    {
                        // Globale Tile-Position
                        int globalX = chunkGlobalX + localX;
                        int globalY = chunkGlobalY + localY;

                        // Isometrische Positionierung
                        int isoX = (globalX - globalY) * (tileWidth / 2);
                        int isoY = (globalX + globalY) * (tileHeight / 2);

                        // Prüfen, ob das Tile im sichtbaren Bereich liegt
                        if (isoX > topLeft.X - tileWidth && isoX < bottomRight.X && isoY > topLeft.Y - tileHeight && isoY < bottomRight.Y)
                        {
                            int tileId = chunk.Tiles[localX, localY];
                            Tile tile = world.GetTileById(tileId);
                            if (tile != null)
                            {
                                int tileDepth = tile.Depth;

                                // Berechnung der source Rectangle
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
            }
            Console.WriteLine("RenderCalls: " + RenderCalls);
        }
    }

}
