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
            int tileDepth = 32; // Die zusätzliche "Tiefe" des Tiles

            for (int y = 0; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    int tileId = world.Get(x, y);

                    // Berechnung der source Rectangle, basierend auf der Tile-ID
                    int tileRow = tileId / (tileSet.Width / tileWidth);
                    int tileColumn = tileId % (tileSet.Width / tileWidth);
                    Rectangle source = new Rectangle(tileColumn * tileWidth, tileRow * (tileHeight + tileDepth), tileWidth, tileHeight + tileDepth);

                    // Isometrische Positionierung auf dem Bildschirm
                    int isoX = (x - y) * (tileWidth / 2);
                    int isoY = (x + y) * (tileHeight / 2) - tileDepth; // Verschiebung um die Tiefe nach oben

                    Rectangle destination = new Rectangle(isoX, isoY, tileWidth, tileHeight + tileDepth);

                    context.SpriteBatch.Draw(tileSet, destination, source, Color.White);
                }
            }
        }
    }
}
