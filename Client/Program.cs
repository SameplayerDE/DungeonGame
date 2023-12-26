
using QColonFrame;
using System;

using var game = new Client.Game1();
game.Run();

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
