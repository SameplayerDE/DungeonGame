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
    public partial class DungeonEntity : QCEntity
    {
        public DungeonEntityFlags Flags;
        public Vector2 Position;
        public Texture2D? Texture;
        public Color Tint = Color.White;

        public virtual void Update(GameTime gameTime)
        {
            if (!Flags.HasFlag(DungeonEntityFlags.Updateable))
            {
                return;
            }
        }

        public virtual void Draw(QCRenderContext context, GameTime gameTime)
        {
            if (!Flags.HasFlag(DungeonEntityFlags.Drawable))
            {
                return;
            }

            if (Texture == null)
            {
                return;
            }

            context.SpriteBatch.Draw(Texture, Position, Tint);

        }
    }
}
