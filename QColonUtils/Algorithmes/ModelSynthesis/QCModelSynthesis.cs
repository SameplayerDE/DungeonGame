using System.Data;

namespace QColonUtils.Algorithmes.ModelSynthesis
{
    public class QCModelSynthesis
    {
        public List<int> States; //possible states
        private int[,] _result; //result
        private Dictionary<(int x, int y), List<int>> _progess; //working grid
        private (int x, int y) _current = (0, 0); //current position

        public QCModelSynthesis(int width, int height)
        {
            States = new List<int>();
            _result = new int[width, height];
            _progess = new();
        }

        public void AddState(int state)
        {
            States.Add(state);
        }

        public void AddStates(params int[] states)
        {
            States.AddRange(states);
        }

        public void Collapse()
        {

        }
    }
}
