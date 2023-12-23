using Microsoft.Xna.Framework;
using QColonFrame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class ClientCore
    {

        public static QCInputHandler Input;

        public static Game Game;

        static ClientCore()
        {
            Input = new QCInputHandler();
        }

        public static void SetGame(Game game)
        {
            Game = game;
        }

        public static void Exit()
        {
            Game?.Exit();
        }

        public static void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
        }

    }
}
