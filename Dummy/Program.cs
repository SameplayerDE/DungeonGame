using QColonUtils.Algorithmes.ModelSynthesis;

int[,] example = new int[,]
{
    { 0, 0, 0, 0, 0},
    { 0, 0, 1, 0, 0},
    { 0, 1, 2, 1, 0},
    { 0, 0, 1, 0, 0},
    { 0, 0, 0, 0, 0},
};

QCModelSynthesis modelSynthesis = new QCModelSynthesis(10, 10);
modelSynthesis.Learn(example);
modelSynthesis.Collapse();

for (int x = 0; x < 10; x++)
{
    for (int y = 0; y < 10; y++)
    {
        int id = modelSynthesis.Result[x, y];
        Console.Write(id);
    }
    Console.WriteLine();
}

Console.WriteLine("Drücke Enter, um den nächsten Schritt auszuführen...");
var running = true;

while (running)
{
    var line = Console.ReadLine();
    if (string.IsNullOrEmpty(line))
    {
    }
    else
    {
        running = false;
    }
}
