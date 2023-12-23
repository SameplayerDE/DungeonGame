using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame.Input
{
    public class QCInputAction
    {

        public string Name { get; private set; }
        public List<List<Keys>> Keys
        {
            get
            {
                return _keys;
            }
        }

        private List<List<Keys>> _keys;
        private Action _pressedCallback;
        private Action _releasedCallback;
        private Action _holdCallback;

        public QCInputAction(string name)
        {
            _keys = new List<List<Keys>>();
            Name = name;
        }

        public QCInputAction SetAction(Action action)
        {
            _pressedCallback = action;
            return this;
        }

        public QCInputAction SetPressedAction(Action action)
        {
            _pressedCallback = action;
            return this;
        }

        public QCInputAction SetReleasedAction(Action action)
        {
            _releasedCallback = action;
            return this;
        }

        public QCInputAction SetHoldAction(Action action)
        {
            _holdCallback = action;
            return this;
        }

        public QCInputAction AddKey(params Keys[] keys)
        {
            _keys.Add(new List<Keys>(keys));
            return this;
        }

        public void Invoke()
        {
            _pressedCallback?.Invoke();
        }

        public void InvokeHold()
        {
            _holdCallback?.Invoke();
        }

        public void InvokePressed()
        {
            _pressedCallback?.Invoke();
        }

        public void InvokeReleased()
        {
            _releasedCallback?.Invoke();
        }

    }
}
