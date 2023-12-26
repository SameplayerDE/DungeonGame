using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame
{
    public class QCWaveFunction<T>
    {
        private List<T> possibleStates;
        private T[,] grid;
        private List<QCWaveRule<T>> rules;
        private Random random;
        private bool[,] collapsed;

        public QCWaveFunction(int width, int height)
        {
            grid = new T[width, height];
            collapsed = new bool[width, height];
            possibleStates = new List<T>();
            rules = new List<QCWaveRule<T>>();
            random = new Random();
        }

        public void PrintGrid()
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x, y] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void AddState(T state)
        {
            possibleStates.Add(state);
        }

        public void AddRule(QCWaveRule<T> rule)
        {
            rules.Add(rule);
        }

        public void Collapse()
        {
            while (HasUncollapsedCells())
            {
                var (x, y) = SelectRandomUncollapsedCell();
                CollapseCell(x, y);
                Propagate(x, y);
            }
        }

        private bool HasUncollapsedCells()
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (!collapsed[x, y]) return true;

            return false;
        }

        private (int, int) SelectRandomUncollapsedCell()
        {
            List<(int, int)> uncollapsedCells = new List<(int, int)>();
            for (int x = 0; x < grid.GetLength(0); x++)
                for (int y = 0; y < grid.GetLength(1); y++)
                    if (!collapsed[x, y]) uncollapsedCells.Add((x, y));

            int index = random.Next(uncollapsedCells.Count);
            return uncollapsedCells[index];
        }

        private void CollapseCell(int x, int y)
        {
            // Hier könntest du die Auswahl des Zustands verfeinern, z.B. basierend auf Gewichtungen.
            T selectedState = possibleStates[random.Next(possibleStates.Count)];
            grid[x, y] = selectedState;
            collapsed[x, y] = true;
        }

        private void Propagate(int x, int y)
        {
            // Implementiere hier die Logik zur Propagation der Auswahl,
            // um die Regeln auf benachbarte Zellen anzuwenden.
        }
    }

}
