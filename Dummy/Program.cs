using QColonUtils.Algorithmes.ModelSynthesis;

QCModelSynthesis<char> function = new QCModelSynthesis<char>(10, 10);
function.AddStates('w', 'b', 'g', 'm'); // Zustände für Wasser, Strand, Gras und Berge

// Regel: Wasser kann nur neben Strand sein
var ruleWater = new QCSynthesisRule<char>('w');
ruleWater.AddAllowedState('w', 50); // Strand neben Wasser
ruleWater.AddAllowedState('b', 50); // Strand neben Wasser

// Regel: Strand kann nur neben Gras sein
var ruleBeach = new QCSynthesisRule<char>('b');
ruleBeach.AddAllowedState('g', 33); // Gras neben Strand
ruleBeach.AddAllowedState('b', 33); // Gras neben Strand
ruleBeach.AddAllowedState('w', 33); // Gras neben Strand

// Regel: Gras kann nur neben Strand oder Bergen sein
var ruleGrass = new QCSynthesisRule<char>('g');
ruleGrass.AddAllowedState('g', 33); // Strand neben Gras
ruleGrass.AddAllowedState('b', 33); // Strand neben Gras
ruleGrass.AddAllowedState('m', 33); // Berge neben Gras

// Regel: Berge können nur neben Gras sein
var ruleMountain = new QCSynthesisRule<char>('m');
ruleMountain.AddAllowedState('g', 50); // Gras neben Bergen
ruleMountain.AddAllowedState('m', 50); // Gras neben Bergen

// Regeln hinzufügen
function.AddRule(ruleWater);
function.AddRule(ruleBeach);
function.AddRule(ruleGrass);
function.AddRule(ruleMountain);

Console.WriteLine("Drücke Enter, um den nächsten Schritt auszuführen...");
var running = true;

while (running)
{
    var line = Console.ReadLine();
    if (string.IsNullOrEmpty(line))
    {
        function.Collapse();
        function.PrintGrid();
    }
    else
    {
        running = false;
    }
}
