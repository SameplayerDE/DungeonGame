using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame
{
    [Serializable]
    public class EventTracker
    {

        private Dictionary<string, bool> events;

        public EventTracker()
        {
            events = new Dictionary<string, bool>();
        }

        public void RecordEvent(string eventName, bool occurred)
        {
            events[eventName] = occurred;
        }

        public bool CheckEvent(string eventName)
        {
            return events.ContainsKey(eventName) && events[eventName];
        }

    }
}
