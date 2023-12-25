using System;

namespace QColonFrame
{
    public class QCTerrainGenerator
    {
        private QCNoiseGenerator _noiseGenerator;
        private int _seed;

        [Flags]
        public enum TerrainType
        {
            MOUNTAIN = 1,
            CAVES = 2,
            SHORES = 4,
            PLAINS = 8,
            FORESTS = 16,
            PONDS = 32
        }

        public QCTerrainGenerator(int seed)
        {
            _noiseGenerator = new QCNoiseGenerator();
            _seed = seed;
        }

        public void SetSeed(int seed)
        {
            _seed = seed;
        }

        public float GetNoise(float x, float y)
        {
            return GetNoise(x, y, TerrainType.PLAINS);
        }

        public float GetNoise(float x, float y, TerrainType type)
        {
            return CalculateCombinedTerrainHeight(x, y, type);
        }

        public float GetNoise(float x, float y, float z, TerrainType type)
        {
            return CalculateCombinedTerrainHeight(x, y, z, type);
        }

        private float CalculateCombinedTerrainHeight(float x, float y, TerrainType type)
        {
            float noiseValue = 0;

            if (type.HasFlag(TerrainType.PLAINS))
            {
                var plainsSettings = new QCNoiseGenerator.NoiseSettings(_seed, 1, 2.0f, 0.3f, 0.05f, 0.1f); // Flachere Ebenen
                noiseValue += _noiseGenerator.GetNoise(x, y, plainsSettings);
            }

            if (type.HasFlag(TerrainType.MOUNTAIN))
            {
                var mountainSettings = new QCNoiseGenerator.NoiseSettings(_seed, 6, 2.0f, 1.0f, 0.03f, 0.5f); // Detailreiche und hohe Berge
                noiseValue += _noiseGenerator.GetNoise(x, y, mountainSettings);
            }

            if (type.HasFlag(TerrainType.SHORES))
            {
                var shoreSettings = new QCNoiseGenerator.NoiseSettings(_seed,  2, 1.5f, 0.4f, 0.1f, -0.1f); // Sanfte Strände
                noiseValue += _noiseGenerator.GetNoise(x, y, shoreSettings);
            }

            if (type.HasFlag(TerrainType.PONDS))
            {
                var pondSettings = new QCNoiseGenerator.NoiseSettings(_seed,  2, 1.0f, 0.3f, 0.2f, -0.5f); // Kleine Teiche
                noiseValue += _noiseGenerator.GetNoise(x, y, pondSettings);
            }

            // Weitere Terrain-Typen hinzufügen...

            return noiseValue;
        }


        private float CalculateCombinedTerrainHeight(float x, float y, float z, TerrainType type)
        {
            float noiseValue = 0;

            if (type.HasFlag(TerrainType.PLAINS))
            {
                var plainsSettings = new QCNoiseGenerator.NoiseSettings(1, 2.0f, 0.3f, 0.05f, 0.1f); // Flachere Ebenen
                noiseValue += _noiseGenerator.GetNoise(x, y, z, plainsSettings);
            }

            if (type.HasFlag(TerrainType.MOUNTAIN))
            {
                var mountainSettings = new QCNoiseGenerator.NoiseSettings(6, 2.0f, 1.0f, 0.03f, 0.5f); // Detailreiche und hohe Berge
                noiseValue += _noiseGenerator.GetNoise(x, y, z, mountainSettings);
            }

            if (type.HasFlag(TerrainType.SHORES))
            {
                var shoreSettings = new QCNoiseGenerator.NoiseSettings(2, 1.5f, 0.4f, 0.1f, -0.1f); // Sanfte Strände
                noiseValue += _noiseGenerator.GetNoise(x, y, z, shoreSettings);
            }

            if (type.HasFlag(TerrainType.PONDS))
            {
                var pondSettings = new QCNoiseGenerator.NoiseSettings(2, 1.0f, 0.3f, 0.2f, -0.5f); // Kleine Teiche
                noiseValue += _noiseGenerator.GetNoise(x, y, z, pondSettings);
            }

            // Weitere Terrain-Typen hinzufügen...

            return noiseValue;
        }

    }
}
