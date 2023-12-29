using System.Collections.Generic;
using System.Data;

namespace QColonUtils.Algorithmes.ModelSynthesis
{



    public class QCModelSynthesis
    {

        public enum Direction
        {
            Up, Down, Left, Right
        }

        public List<int> States; //possible states
        public int[,] Result; //result
        private int[,] _model; //copy of model
        public readonly int Height;
        public readonly int Width;
        private Dictionary<(int x, int y), List<int>>? _progess = null; //working grid
        private (int x, int y) _current = (0, 0); //current position

        public QCModelSynthesis(int width, int height)
        {
            Width = width;
            Height = height;
            States = new List<int>();
            Result = new int[width, height];
        }

        public void Learn(int[,] model)
        {
            _model = new int[model.GetLength(0), model.GetLength(1)];
            Array.Copy(model, _model, model.Length);

            for (int x = 0; x < model.GetLength(0); x++)
            {
                for (int y = 0; y < model.GetLength(1); y++)
                {
                    AddState(model[x, y]);
                }
            }
            InitializeProgress();
        }

        private void InitializeProgress()
        {
            if (_progess == null)
            {
                _progess = new();
            }
            else
            {
                _progess.Clear();
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _progess.Add((x, y), new List<int>(States));
                }
            }
        }

        public void AddState(int state)
        {
            if (!States.Contains(state))
            {
                States.Add(state);
            }
        }

        //public void Collapse()
        //{
        //    if (_current == (0, 0))
        //    {
        //        var random = new Random();
        //        var states = _progess[_current];    
        //        int randomStateIndex = random.Next(states.Count);
        //        int selectedState = states[randomStateIndex];
        //        _progess[_current] = new List<int> { selectedState };
        //    }
        //
        //    for (int x = 0; x < _width; x++)
        //    {
        //        for (int y = 0; y < _height; y++)
        //        {
        //            var cellStates = _progess[(x, y)];
        //            if (cellStates.Count == 1)
        //            {
        //                SetState(x, y, cellStates[0]);
        //            }
        //        }
        //    }
        //
        //    MoveToNext();
        //}

        private void SetState(int x, int y, int state)
        {
            Result[x, y] = state;
        }

        private void MoveBy(int x, int y)
        {
            int newX = (_current.x + x) % Width;
            int newY = (_current.y + y) % Height;

            if (newX < 0) newX += Width;
            if (newY < 0) newY += Height;

            _current = (newX, newY);
        }

        public Dictionary<Direction, HashSet<int>> PossibleNeighbourStates(int centerState)
        {
            var result = new Dictionary<Direction, HashSet<int>>
            {
                { Direction.Up, new HashSet<int>() },
                { Direction.Down, new HashSet<int>() },
                { Direction.Left, new HashSet<int>() },
                { Direction.Right, new HashSet<int>() }
            };

            for (int x = 0; x < _model.GetLength(0); x++)
            {
                for (int y = 0; y < _model.GetLength(1); y++)
                {
                    var state = _model[x, y];
                    if (centerState == state)
                    {
                        if (y > 0)
                        {
                            var up = _model[x, y - 1];
                            result[Direction.Up].Add(up);
                        }
                        if (x > 0)
                        {
                            var left = _model[x - 1, y];
                            result[Direction.Left].Add(left);
                        }
                        if (x < _model.GetLength(0) - 1)
                        {
                            var right = _model[x + 1, y];
                            result[Direction.Right].Add(right);
                        }
                        if (y < _model.GetLength(1) - 1)
                        {
                            var down = _model[x, y + 1];
                            result[Direction.Down].Add(down);
                        }
                    }
                }
            }

            return result;
        }

        private void MoveToNext()
        {
            _current.x++;
            if (_current.x >= Width)
            {
                _current.x = 0;
                _current.y++;
                if (_current.y >= Height)
                {
                    //done
                }
            }
        }

        public void Collapse()
        {
            bool progressMade;
            do
            {
                progressMade = false;
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (_progess[(x, y)].Count > 1)
                        {
                            CollapseCellAndUpdateNeighbors(x, y);
                            progressMade = true;
                        }
                        else
                        {
                            int state = _progess[(x, y)][0];
                            SetState(x, y, state);
                            UpdateNeighbors(x, y, state);
                        }
                    }
                }
            } while (progressMade); // Wiederhole, bis keine Änderungen mehr vorgenommen werden
        }

        private void CollapseCellAndUpdateNeighbors(int x, int y)
        {
            var random = new Random();
            var states = _progess[(x, y)];
            int randomStateIndex = random.Next(states.Count);
            int selectedState = states[randomStateIndex];

            SetState(x, y, selectedState);
            _progess[(x, y)] = new List<int> { selectedState };

            UpdateNeighbors(x, y, selectedState);
        }

        private void UpdateNeighbors(int x, int y, int state)
        {
            // Aktualisiere die Zustände der Nachbarn
            UpdateNeighbor(x - 1, y, Direction.Right, state);
            UpdateNeighbor(x + 1, y, Direction.Left, state);
            UpdateNeighbor(x, y - 1, Direction.Down, state);
            UpdateNeighbor(x, y + 1, Direction.Up, state);
        }

        private void UpdateNeighbor(int x, int y, Direction fromDirection, int state)
        {
            if (IsWithinBounds(x, y))
            {
                var possibleStates = PossibleNeighbourStates(state);
                var allowedStates = possibleStates[fromDirection];

                _progess[(x, y)].RemoveAll(item => !allowedStates.Contains(item));
            }
        }

        private bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
