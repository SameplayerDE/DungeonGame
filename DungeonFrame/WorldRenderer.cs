using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QColonFrame;
using System;

namespace DungeonFrame
{
    public class WorldRenderer
    {
        public static WorldRenderer Instance { get; } = new WorldRenderer();

        public static int RenderCalls = 0;

        private WorldRenderer() { }

        public enum RenderingStyle { Isometric, Orthogonal }
        public RenderingStyle CurrentStyle { get; set; } = RenderingStyle.Orthogonal;

        public void Draw(QCRenderContext context, GameTime gameTime, World world, Texture2D tileSet)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                CurrentStyle = RenderingStyle.Orthogonal;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CurrentStyle = RenderingStyle.Isometric;
            }

            RenderCalls = 0;
            int standardTileWidth = 64;
            int standardTileHeight = 32;

            // Ermitteln des sichtbaren Bereichs
            var cameraViewMatrix = context.Camera.GetViewMatrix();
            var inverseViewMatrix = Matrix.Invert(cameraViewMatrix);
            var topLeft = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var bottomRight = Vector2.Transform(new Vector2(context.SpriteBatch.GraphicsDevice.Viewport.Width, context.SpriteBatch.GraphicsDevice.Viewport.Height), inverseViewMatrix);

            // Durchlaufe alle Chunks
            foreach (var chunkEntry in world.Chunks.Where(c => c.Value.IsLoaded))
            {
                var chunk = chunkEntry.Value;
                // Berechne globale Position des Chunks
                int chunkGlobalX = chunk.X * Chunk.Width;
                int chunkGlobalY = chunk.Y * Chunk.Height;

                for (int localY = 0; localY < Chunk.Height; localY++)
                {
                    for (int localX = 0; localX < Chunk.Width; localX++)
                    {
                        // Globale Tile-Position
                        int globalX = chunkGlobalX + localX;
                        int globalY = chunkGlobalY + localY;

                        Tile tile = world.GetTileById(chunk.Tiles[localX, localY]);
                        if (tile != null)
                        {
                            int baseIsoX, baseIsoY;
                            if (CurrentStyle == RenderingStyle.Isometric)
                            {
                                baseIsoX = (globalX - globalY) * standardTileWidth / 2;
                                baseIsoY = (globalX + globalY) * standardTileHeight / 2;
                            }
                            else // Orthogonal
                            {
                                baseIsoX = globalX * standardTileWidth;
                                baseIsoY = globalY * standardTileHeight;
                            }

                            int isoX = baseIsoX - tile.BaseXOffset;
                            int isoY = baseIsoY - tile.BaseYOffset;

                            // Prüfen, ob das Tile im sichtbaren Bereich liegt
                            if (isoX > topLeft.X - tile.SourceL && isoX < bottomRight.X && isoY > topLeft.Y - tile.SourceB && isoY < bottomRight.Y)
                            {
                                Rectangle source = new Rectangle(tile.SourceX, tile.SourceY, tile.SourceL, tile.SourceB);
                                Rectangle destination = new Rectangle(isoX, isoY, tile.SourceL, tile.SourceB);

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
