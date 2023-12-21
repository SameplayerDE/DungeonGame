using Microsoft.Xna.Framework;
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

        public void Draw(QCRenderContext context, GameTime gameTime, World world)
        {

        }

    }
}
