using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    [Flags]
    public enum DungeonEntityFlags
    {
        None = 0x00,
        Drawable = 0x01,
        Updateable = 0x02,
        Contactable = 0x04,
    }
}
