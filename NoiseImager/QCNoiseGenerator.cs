using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QColonFrame
{
    public class QCNoiseGenerator
    {

        private FastNoiseLite _noiseGenerator;

        public QCNoiseGenerator()
        {
            _noiseGenerator = new FastNoiseLite();
        }

        public struct NoiseSettings
        {

            public static NoiseSettings Default = new NoiseSettings(1337, FastNoiseLite.NoiseType.Perlin, 1, 1, 1, 1, 0);

            public int Seed;
            public FastNoiseLite.NoiseType NoiseType;
            public int Octaves;
            public float Lacunarity;
            public float Gain;
            public float Scale;
            public float Bias;

            public NoiseSettings(int octaves, float lacunarity, float gain, float scale, float bias)
            {
                Seed = 1337;
                NoiseType = FastNoiseLite.NoiseType.Perlin;
                Octaves = octaves;
                Lacunarity = lacunarity;
                Gain = gain;
                Scale = scale;
                Bias = bias;
            }

            public NoiseSettings(int seed, int octaves, float lacunarity, float gain, float scale, float bias)
            {
                Seed = seed;
                NoiseType = FastNoiseLite.NoiseType.Perlin;
                Octaves = octaves;
                Lacunarity = lacunarity;
                Gain = gain;
                Scale = scale;
                Bias = bias;
            }

            public NoiseSettings(FastNoiseLite.NoiseType noiseType, int octaves, float lacunarity, float gain, float scale, float bias)
            {
                Seed = 1337;
                NoiseType = noiseType;
                Octaves = octaves;
                Lacunarity = lacunarity;
                Gain = gain;
                Scale = scale;
                Bias = bias;
            }

            public NoiseSettings(int seed, FastNoiseLite.NoiseType noiseType, int octaves, float lacunarity, float gain, float scale, float bias)
            {
                Seed = seed;
                NoiseType = noiseType;
                Octaves = octaves;
                Lacunarity = lacunarity;
                Gain = gain;
                Scale = scale;
                Bias = bias;
            }
        }

        public float GetNoise(float x, float y)
        {
            return GetNoise(x, y, NoiseSettings.Default);
        }

        public float GetNoise(float x, float y, NoiseSettings settings)
        {

            _noiseGenerator.SetSeed(settings.Seed);
            _noiseGenerator.SetNoiseType(settings.NoiseType);

            float amplitude = 1f;
            float frequency = settings.Scale;
            float noise = 0f;

            for (int i = 0; i < settings.Octaves; i++)
            {
                float sampleX = x * frequency;
                float sampleY = y * frequency;

                float noiseValue = _noiseGenerator.GetNoise(sampleX, sampleY);
                noise += noiseValue * amplitude;

                amplitude *= settings.Gain;
                frequency *= settings.Lacunarity;
            }

            return Math.Clamp(noise + settings.Bias, -1f, 1f);
        }

        public float GetNoise(float x, float y, float z, NoiseSettings settings)
        {
            _noiseGenerator.SetSeed(settings.Seed);
            _noiseGenerator.SetNoiseType(settings.NoiseType);

            float amplitude = 1f;
            float frequency = settings.Scale;
            float noise = 0f;

            for (int i = 0; i < settings.Octaves; i++)
            {
                float sampleX = x * frequency;
                float sampleY = y * frequency;
                float sampleZ = z * frequency;

                float noiseValue = _noiseGenerator.GetNoise(sampleX, sampleY, sampleZ);
                noise += noiseValue * amplitude;

                amplitude *= settings.Gain;
                frequency *= settings.Lacunarity;
            }

            return Math.Clamp(noise + settings.Bias, -1f, 1f);
        }

        public float GetCombinedNoise(float x, float y, params NoiseSettings[] settingsArray)
        {
            float combinedNoise = 0f;
            foreach (var settings in settingsArray)
            {
                combinedNoise += GetNoise(x, y, settings);
            }

            //return combinedNoise;
            return Math.Clamp(combinedNoise, -1f, 1f);
        }

        public float GetCombinedNoise(float x, float y, float z, params NoiseSettings[] settingsArray)
        {
            float combinedNoise = 0f;
            foreach (var settings in settingsArray)
            {
                combinedNoise += GetNoise(x, y, z, settings);
            }

            //return combinedNoise;
            return Math.Clamp(combinedNoise, -1f, 1f);
        }

    }
}
