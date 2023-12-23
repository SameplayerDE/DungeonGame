using Microsoft.VisualBasic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame.Input
{
    public class QCInputHandler
    {
        public QCKeyInputHandler Keyboard { get; private set; }

        public QCInputHandler()
        {
            Keyboard = new QCKeyInputHandler();
        }

        public void Update(GameTime gameTime)
        {
            Keyboard.Update(gameTime);
        }
    }

    public class QCKeyInputHandler
    {
        private KeyboardState _currentState;
        private KeyboardState _previousState;

        public QCKeyInputHandler()
        {
            _currentState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }
    }

}
