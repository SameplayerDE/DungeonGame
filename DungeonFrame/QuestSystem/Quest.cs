using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonFrame.QuestSystem
{
    [Serializable]
    public class Quest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestObjective> Objectives { get; private set; }
        public QuestStatus Status { get; set; }
        public Action OnCompletion { get; set; }

        public Quest(string title, string description)
        {
            Title = title;
            Description = description;
            Objectives = new List<QuestObjective>();
            Status = QuestStatus.Available;
        }

        public void AddObjective(QuestObjective objective)
        {
            Objectives.Add(objective);
        }

        public bool CheckIfComplete()
        {
            return Objectives.All(o => o.IsCompleted);
        }

        public void Complete()
        {
            foreach (var objective in Objectives)
            {
                objective.UpdateProgress();
            }

            if (CheckIfComplete())
            {
                Status = QuestStatus.Completed;
                OnCompletion?.Invoke();
            }
        }
    }

}
