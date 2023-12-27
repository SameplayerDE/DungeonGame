namespace QColonUtils.Algorithmes.ModelSynthesis
{
    public class QCModelSynthesis<T> where T : IComparable
    {
        public List<T> States;
        public T[,] Grid;
        private (int x, int y) _current = (0, 0);

        public QCModelSynthesis(int width, int height, List<T> states)
        {
            States = states;
            Grid = new T[width, height];
        }

        public void AddState(T state)
        {
            States.Add(state);
        }

        public void Collapse()
        {
            if (_current == (0, 0) && States.Any())
            {
                Grid[_current.x, _current.y] = States.Min();
            }

            UpdateNeighbours();
            MoveToNextCell();
        }

        private void UpdateNeighbours()
        {
            var neighbourOffsets = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach (var offset in neighbourOffsets)
            {
                int neighbourX = _current.x + offset.dx;
                int neighbourY = _current.y + offset.dy;

                if (neighbourX >= 0 && neighbourX < Grid.GetLength(0) && neighbourY >= 0 && neighbourY < Grid.GetLength(1))
                {
                    List<T> possibleStatesForNeighbour = GetPossibleStatesForNeighbour(Grid[_current.x, _current.y]);

                    if (possibleStatesForNeighbour.Any())
                    {
                        Grid[neighbourX, neighbourY] = possibleStatesForNeighbour.Min();
                    }
                }
            }
        }

        private List<T> GetPossibleStatesForNeighbour(T currentState)
        {
            int currentStateValue = Convert.ToInt32(currentState);
            List<T> possibleStates = new List<T>();

            foreach (var state in States)
            {
                int stateValue = Convert.ToInt32(state);
                if (stateValue > currentStateValue + 1)
                {
                    possibleStates.Add(state);
                }
            }

            return possibleStates;
        }

        private void MoveToNextCell()
        {
            if (_current.x < Grid.GetLength(0) - 1)
            {
                _current.x++;
            }
            else if (_current.y < Grid.GetLength(1) - 1)
            {
                _current.x = 0;
                _current.y++;
            }
            else
            {
                // Ende des Rasters erreicht
            }
        }
    }
}
