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
    public class WorldRenderer
    {

        public static WorldRenderer Instance { get; } = new WorldRenderer();

        static WorldRenderer()
        {
        }

        private WorldRenderer()
        {

        }

        public void Draw(QCRenderContext context, GameTime gameTime, World world, Texture2D tileSet)
        {
            int tileWidth = 64; // Die volle Breite des Tiles
            int tileHeight = 32; // Die sichtbare Höhe der Oberfläche des Tiles im isometrischen Raster

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    Tile tile = world.GetTile(x, y);
                    if (tile != null)
                    {
                        int tileDepth = tile.Depth; // Verwende die Tiefe aus dem Tile-Objekt

                        // Berechnung der source Rectangle, basierend auf der Tile-ID
                        int tileRow = tile.Id / (tileSet.Width / tileWidth);
                        int tileColumn = tile.Id % (tileSet.Width / tileWidth);
                        Rectangle source = new Rectangle(tileColumn * tileWidth, tileRow * (tileHeight + tileDepth), tileWidth, tileHeight + tileDepth);

                        // Isometrische Positionierung auf dem Bildschirm
                        int isoX = (x - y) * (tileWidth / 2);
                        int isoY = (x + y) * (tileHeight / 2) - tileDepth; // Berücksichtige die variable Tiefe

                        Rectangle destination = new Rectangle(isoX, isoY, tileWidth, tileHeight + tileDepth);

                        context.SpriteBatch.Draw(tileSet, destination, source, Color.White);
                    }
                }
            }
        }

    }
}
