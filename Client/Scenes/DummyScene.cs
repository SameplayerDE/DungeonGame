using Microsoft.Xna.Framework;
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
        public DummyScene(Game game) : base("dummy", game)
        {
        }

        public override void Initialize()
        {
            Console.Write("HI");
            base.Initialize();
        }

        public override void Draw(QCRenderContext context, GameTime gameTime)
        {
            Console.Write("DRAW");
            base.Draw(context, gameTime);
        }

    }
}
