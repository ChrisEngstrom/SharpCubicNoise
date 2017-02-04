namespace CubicNoise {
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class CubicNoise {
        private const int CUBIC_NOISE_RAND_A = 134775813;
        private const int CUBIC_NOISE_RAND_B = 1103515245;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static float CubicNoiseRandom(uint seed,
                                              int x,
                                              int y) {
            return (float)
                (
                    (((x ^ y) * CUBIC_NOISE_RAND_A) ^ (seed + x)) *
                    (((CUBIC_NOISE_RAND_B * x) << 16) ^ (CUBIC_NOISE_RAND_B * y) - CUBIC_NOISE_RAND_A)
                ) / uint.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private static int CubicNoiseTile(int coordinate,
                                          int period) {
            return coordinate % period;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static float CubicNoiseInterpolate(float a,
                                                   float b,
                                                   float c,
                                                   float d,
                                                   float x) {
            float p = (d - c) - (a - b);

            return x * x * x * p + x * x * ((a - b) - p) + x * (c - a) + b;
        }

        #region 1-Dimensional
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="octave"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static CubicNoiseConfig CubicNoiseConfig1D(uint seed,
                                                          int octave,
                                                          int period) {
            CubicNoiseConfig config = new CubicNoiseConfig();

            config.Seed = seed;
            config.Octave = octave;
            config.PeriodX = period / octave;

            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float CubicNoiseSample1D(CubicNoiseConfig config,
                                               float x) {
            int xi = (int)(x / config.Octave);
            float lerp = x / config.Octave - xi;

            return CubicNoiseInterpolate(CubicNoiseRandom(config.Seed, CubicNoiseTile(xi - 1, config.PeriodX), 0),
                                         CubicNoiseRandom(config.Seed, CubicNoiseTile(xi, config.PeriodX), 0),
                                         CubicNoiseRandom(config.Seed, CubicNoiseTile(xi + 1, config.PeriodX), 0),
                                         CubicNoiseRandom(config.Seed, CubicNoiseTile(xi + 2, config.PeriodX), 0),
                                         lerp) * 0.5f + 0.25f;
        }
        #endregion

        #region 2-Dimensional
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="octave"></param>
        /// <param name="periodX"></param>
        /// <param name="periodY"></param>
        /// <returns></returns>
        public static CubicNoiseConfig CubicNoiseConfig2D(uint seed,
                                                          int octave,
                                                          int periodX,
                                                          int periodY) {
            CubicNoiseConfig config = new CubicNoiseConfig();

            config.Seed = seed;
            config.Octave = octave;
            config.PeriodX = periodX / octave;
            config.PeriodY = periodY / octave;

            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float CubicNoiseSample2D(CubicNoiseConfig config,
                                               float x,
                                               float y) {
            int xi = (int)Math.Floor(x / config.Octave);
            float lerpX = x / config.Octave - xi;

            int yi = (int)Math.Floor(y / config.Octave);
            float lerpY = y / config.Octave - yi;

            float[] xSamples = new float[4];

            for ( int i = 0; i < 4; i++ ) {
                xSamples[i] = CubicNoiseInterpolate(CubicNoiseRandom(config.Seed,
                                                                     CubicNoiseTile(xi - 1, config.PeriodX),
                                                                     CubicNoiseTile(yi - 1 + i, config.PeriodY)),
                                                    CubicNoiseRandom(config.Seed,
                                                                     CubicNoiseTile(xi, config.PeriodX),
                                                                     CubicNoiseTile(yi - 1 + i, config.PeriodY)),
                                                    CubicNoiseRandom(config.Seed,
                                                                     CubicNoiseTile(xi + 1, config.PeriodX),
                                                                     CubicNoiseTile(yi - 1 + i, config.PeriodY)),
                                                    CubicNoiseRandom(config.Seed,
                                                                     CubicNoiseTile(xi + 2, config.PeriodX),
                                                                     CubicNoiseTile(yi - 1 + i, config.PeriodY)),
                                                    lerpX);
            }

            return CubicNoiseInterpolate(xSamples[0],
                                         xSamples[1],
                                         xSamples[2],
                                         xSamples[3],
                                         lerpY) * 0.5f + 0.25f;
        }
        #endregion
    }
}
