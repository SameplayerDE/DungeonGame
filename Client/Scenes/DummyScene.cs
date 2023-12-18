using DungeonFrame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QColonFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Scenes
{
    internal class DummyScene : QCScene
    {

        private DungeonEntity _entity;

        public DummyScene(Game game) : base("dummy", game)
        {
        }

        public override void Initialize()
        {
            _entity = new DungeonEntity();
            _entity.Flags = DungeonEntityFlags.Drawable;
            base.Initialize();
        }

        public override void LoadContent()
        {
            _entity.Texture = Content.Load<Texture2D>("missing");
            base.LoadContent();
        }

        public override void Draw(QCRenderContext context, GameTime gameTime)
        {
            _entity.Draw(context, gameTime);
            base.Draw(context, gameTime);
        }

    }
}
