using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame.QuestSystem
{
    public abstract class QuestObjective
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public QuestObjective(string description)
        {
            Description = description;
            IsCompleted = false;
        }

        public abstract void UpdateProgress();
    }
}
