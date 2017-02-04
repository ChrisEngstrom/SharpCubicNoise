namespace CubicNoise_Sample {
    using System;
    using System.Drawing;
    using CubicNoise;

    class Program {
        static void Main(string[] args) {
            const int SIZE = 512;

            Bitmap heightMap = new Bitmap(SIZE, SIZE);
            CubicNoiseConfig config = CubicNoise.CubicNoiseConfig2D(0, 32, SIZE, SIZE);

            for ( int y = 0; y < SIZE; y++ ) {
                for ( int x = 0; x < SIZE; x++ ) {
                    float sample = CubicNoise.CubicNoiseSample2D(config, x, y);
                    byte byteSample = (byte)(sample * 100);
                    byte adjustedSample = (byte)(byteSample + 155);

                    heightMap.SetPixel(x,
                                       y,
                                       Color.FromArgb(
                                           adjustedSample,
                                           adjustedSample,
                                           adjustedSample));
                }
            }

            heightMap.Save("heightmap.png");
        }
    }
}
