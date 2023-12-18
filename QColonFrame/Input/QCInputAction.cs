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

        public enum TriggerState
        {
            Pressed,
            Released,
            Down,
            Up
        }

        public Keys? Key;
        public TriggerState State;
        public bool IsTriggered;

    }
}
