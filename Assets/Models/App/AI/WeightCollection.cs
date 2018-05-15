using System;
using Rand = UnityEngine.Random;

namespace App
{
    [Serializable]
    public class WeightCollection
    {
        public int countIterations;
        public float[][] weights1, weights2;
        [NonSerialized] public float[] sumLayer1, sumLayer2;

        public WeightCollection()
        {
            weights1 = new float[2][];
            sumLayer1 = new float[2];
            weights2 = new float[1][];
            sumLayer2 = new float[1];
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < weights1.Length; i++)
            {
                weights1[i] = new float[4];
                for (int j = 0; j < weights1[i].Length; j++)
                {
                    weights1[i][j] = Rand.value;
                }
            }

            for (int i = 0; i < weights2.Length; i++)
            {
                weights2[i] = new float[3];
                for (int j = 0; j < weights2[i].Length; j++)
                {
                    weights2[i][j] = Rand.value;
                }
            }
        }
    }
}
