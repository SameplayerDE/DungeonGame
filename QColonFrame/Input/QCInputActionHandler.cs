using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame.Input
{
    public class QCInputActionHandler
    {

        private List<QCInputAction> _actions;
        private HashSet<string> _blockedActions;

        public QCInputActionHandler()
        {
            _actions = new List<QCInputAction>();
            _blockedActions = new HashSet<string>();
        }

        public void AddAction(QCInputAction action)
        {
            _actions.Add(action);
        }

        public void BlockAll()
        {
            foreach (var action in _actions)
            {
                _blockedActions.Add(action.Name);
            }
        }

        public void BlockAction(params string[] actions)
        {
            foreach (var action in actions)
            {
                _blockedActions.Add(action);
            }
        }

        public void UnblockAll()
        {
            _blockedActions.Clear();
        }

        public void UnblockAction(params string[] actions)
        {
            foreach (var action in actions)
            {
                _blockedActions.Remove(action);
            }
        }

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime, QCInputHandler inputHandler)
        {
            foreach (var action in _actions)
            {
                // Skip blocked actions
                if (_blockedActions.Contains(action.Name))
                    continue;

                foreach (var keySet in action.Keys)
                {
                    if (keySet.TrueForAll(inputHandler.Keyboard.IsKeyDown))
                    {
                        action.InvokeHold();
                        if (keySet.TrueForAll(inputHandler.Keyboard.IsKeyPressed))
                        {
                            action.InvokePressed();
                        }
                        break;
                    }
                    if (keySet.TrueForAll(inputHandler.Keyboard.IsKeyReleased))
                    {
                        action.InvokeReleased();
                        break;
                    }
                }
            }
        }

    }
}
