using QColonFrame;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class NoiseImageGenerator
{

    public static void Main(string[] args)
    {
        string folderPath = "Images";
        Directory.CreateDirectory(folderPath);  // Erstellt den Ordner, falls er nicht existiert

        Random rand = new Random();
        List<QCNoiseGenerator.NoiseSettings> settingsList = GenerateRandomSettingsList(rand, 1, 5); // Erstellt eine Liste von 1-5 zufälligen Settings

        //string settingsText = $"{settings.Octaves}_{settings.Lacunarity}_{settings.Gain}_{settings.Scale}_{settings.Bias}";
        string settingsText = "";
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssM");
        string filePath = Path.Combine(folderPath, $"{settingsText}_{timestamp}.png");
        CreateNoiseImage(filePath, settingsList);
    }

    private static List<QCNoiseGenerator.NoiseSettings> GenerateRandomSettingsList(Random rand, int minSettings, int maxSettings)
    {
        int numberOfSettings = rand.Next(minSettings, maxSettings + 1);
        List<QCNoiseGenerator.NoiseSettings> settingsList = new List<QCNoiseGenerator.NoiseSettings>();

        for (int i = 0; i < numberOfSettings; i++)
        {
            var settings = new QCNoiseGenerator.NoiseSettings(
                rand.Next(1, 999999),
                rand.Next(1, 3),       // Octaves
                rand.NextFloat(0.5f, 1f),  // Lacunarity
                rand.NextFloat(0.5f, 1f),  // Gain
                rand.NextFloat(0.5f, 1f),  // Scale
                rand.NextFloat(0, 0.2f)); // Bias
            settingsList.Add(settings);
        }

        return settingsList;
    }

    public static void CreateNoiseImage(string filePath, params QCNoiseGenerator.NoiseSettings settings)
    {
        QCNoiseGenerator noise = new QCNoiseGenerator();

        int width = 1024;
        int height = 1024;

        using (Image<Rgba32> image = new Image<Rgba32>(width, height))
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float nx = x;
                    float ny = y;

                    float noiseValue = noise.GetCombinedNoise(nx, ny, settings);

                    int gray = (int)((noiseValue + 1) * 0.5 * 255); // Skaliert von [-1, 1] auf [0, 255]
                    gray = Math.Clamp(gray, 0, 255);

                    image[x, y] = new Rgba32((byte)gray, (byte)gray, (byte)gray);
                }
            }

            image.Save(filePath);
        }



    }


}
