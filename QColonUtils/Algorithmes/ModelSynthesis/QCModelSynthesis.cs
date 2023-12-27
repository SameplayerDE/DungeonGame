using System.Data;

namespace QColonUtils.Algorithmes.ModelSynthesis
{
    public class QCModelSynthesis<T> where T : IComparable
    {
        public List<T> States;
        public T[,] Grid;
        private (int x, int y) _current = (0, 0);
        private List<QCSynthesisRule<T>> _rules = new List<QCSynthesisRule<T>>();

        public QCModelSynthesis(int width, int height, List<T> states)
        {
            States = states;
            Grid = new T[width, height];
        }

        public QCModelSynthesis(int width, int height)
        {
            States = new List<T>();
            Grid = new T[width, height];
        }

        public void AddState(T state)
        {
            States.Add(state);
        }

        public void AddRule(QCSynthesisRule<T> rule)
        {
            _rules.Add(rule);
        }

        public void AddStates(params T[] states)
        {
            States.AddRange(states);
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

        public void PrintGrid()
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                for (int x = 0; x < Grid.GetLength(0); x++)
                {
                    Console.Write($"{Grid[x, y]}");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Aktuelle Zelle: {_current}");
            Console.WriteLine($"Verfügbare Zustände: {string.Join(", ", States)}");
        }


        private void SetConsoleColor(int state)
        {
            switch (state)
            {
                case 0: // Wasser
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 1: // Strand
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 2: // Gras
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3: // Berge
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
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

        private T SelectStateBasedOnProbability(List<QCSynthesisRule<T>.StateProbabilityPair> allowedStates)
        {
            Random random = new Random();
            double randomValue = random.NextDouble() * 100; // Wert zwischen 0 und 100
            double cumulativeProbability = 0;

            foreach (var pair in allowedStates)
            {
                cumulativeProbability += pair.Probability;
                if (randomValue <= cumulativeProbability)
                {
                    return pair.State;
                }
            }

            return default(T); // oder einen Standardwert zurückgeben, falls kein Zustand ausgewählt wurde
        }


        private List<T> GetPossibleStatesForNeighbour(T currentState)
        {
            var possibleStates = new List<T>();
            foreach (var rule in _rules)
            {
                if (rule.RequiredNeighbour.Equals(currentState))
                {
                    T selectedState = SelectStateBasedOnProbability(rule.AllowedStates);
                    if (!selectedState.Equals(default(T)))
                    {
                        possibleStates.Add(selectedState);
                    }
                }
            }
            return possibleStates;
        }

        private bool RandomValueMatchesProbability(double probability)
        {
            Random random = new Random();
            return random.NextDouble() < probability;
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
