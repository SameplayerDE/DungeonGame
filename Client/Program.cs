
using QColonFrame;
using System;

QCWaveFunction<int> waveFunction = new QCWaveFunction<int>(5, 5);

// Füge mögliche Zustände hinzu
for (int i = 0; i < 10; i++)
{
    waveFunction.AddState(i);
}

// Füge Regeln hinzu
waveFunction.AddRule(new NeighborRule(2)); // Maximaler Unterschied von 2 zu den Nachbarn
waveFunction.AddRule(new SpecificValueRule(0, 0, 5)); // Spezifischer Wert für die Zelle (0,0)

// Gib den Zustand des Rasters in der Konsole aus
waveFunction.PrintGrid();

// Führe den Wave-Function-Collapse aus
waveFunction.Collapse();

// Gib den Zustand des Rasters in der Konsole aus
waveFunction.PrintGrid();

//using var game = new Client.Game1();
//game.Run();

public class SpecificValueRule : QCWaveRule<int>
{
    private int requiredValue;
    private int posX, posY;

    public SpecificValueRule(int x, int y, int value)
    {
        this.posX = x;
        this.posY = y;
        this.requiredValue = value;
    }

    public override bool IsValid(int[,] grid, int x, int y)
    {
        return !(x == posX && y == posY) || grid[x, y] == requiredValue;
    }
}


public class NeighborRule : QCWaveRule<int>
{
    private int maxDifference;

    public NeighborRule(int maxDifference)
    {
        this.maxDifference = maxDifference;
    }

    public override bool IsValid(int[,] grid, int x, int y)
    {
        // Prüfe die Nachbarn der Zelle
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Überspringe die aktuelle Zelle
                int neighborX = x + dx;
                int neighborY = y + dy;
                if (neighborX >= 0 && neighborX < grid.GetLength(0) && neighborY >= 0 && neighborY < grid.GetLength(1))
                {
                    int diff = Math.Abs(grid[x, y] - grid[neighborX, neighborY]);
                    if (diff > maxDifference)
                        return false;
                }
            }
        }
        return true;
    }
}
