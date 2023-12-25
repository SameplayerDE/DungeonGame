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

        public QCNoiseGenerator(int seed)
        {
            _noiseGenerator = new FastNoiseLite(seed);
        }

        public void SetSeed(int seed)
        {
            _noiseGenerator.SetSeed(seed);
        }

        public void SetNoiseType(FastNoiseLite.NoiseType type)
        {
            _noiseGenerator.SetNoiseType(type);
        }

        public struct NoiseSettings
        {
            public int Octaves;
            public float Lacunarity;
            public float Gain;
            public float Scale;
            public float Bias;

            public NoiseSettings(int octaves, float lacunarity, float gain, float scale, float bias)
            {
                Octaves = octaves;
                Lacunarity = lacunarity;
                Gain = gain;
                Scale = scale;
                Bias = bias;
            }
        }

        public float GetNoise(float x, float y, NoiseSettings settings)
        {
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
