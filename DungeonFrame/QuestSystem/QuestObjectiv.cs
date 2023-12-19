using QColonFrame;
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

    public class KillObjective : QuestObjective
    {
        public int RequiredAmount { get; private set; }
        public int CurrentAmount { get; private set; }

        public KillObjective(string description, int requiredAmount) : base(description)
        {
            RequiredAmount = requiredAmount;
            CurrentAmount = 0;
        }

        public override void UpdateProgress()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                IsCompleted = true;
            }
        }
    }

    public class CollectObjective<T> : QuestObjective where T : QCEntity 
    {
        public int RequiredAmount { get; private set; }
        public int CurrentAmount { get; private set; }

        public CollectObjective(string description, int requiredAmount) : base(description)
        {
            RequiredAmount = requiredAmount;
            CurrentAmount = 0;
        }

        public override void UpdateProgress()
        {
            if (CurrentAmount >= RequiredAmount)
            {
                IsCompleted = true;
            }
        }
    }
}
