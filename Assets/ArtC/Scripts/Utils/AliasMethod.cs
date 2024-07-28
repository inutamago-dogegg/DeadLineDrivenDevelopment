using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtC.Utils {
    public class AliasMethod {
        //https://en.wikipedia.org/wiki/Alias_method
        //https://stackoverflow.com/a/39199014
        //https://www.keithschwarz.com/darts-dice-coins/

        public AliasMethod() { }

        public AliasMethod(float zeroThreshold) {
            SetZeroThreshold(zeroThreshold);
        }

        /// <summary>
        /// if the probability is less than this value, it is treated as 0
        /// </summary>
        public float ZeroThreshold { get; private set; } = 0f;

        public bool ApplyZeroThreshold => ZeroThreshold > 0f;

        /// <summary>
        /// set the threshold for treating the probability as 0
        /// </summary>
        /// <param name="zeroThreshold"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetZeroThreshold(float zeroThreshold) {
            if (zeroThreshold is < 0f or > 1f) {
                throw new ArgumentOutOfRangeException(nameof(zeroThreshold));
            }

            ZeroThreshold = zeroThreshold;
        }

        private int _n;
        private float[] _probabilities;
        private int[] _alias;

        /// <summary>
        /// construct probability distribution
        /// </summary>
        /// <param name="weights"></param>
        public void Constructor(float[] weights) {
            int n;
            float sum;
            float[] p;

            if (!ApplyZeroThreshold) {
                sum = weights.Sum();
                n = weights.Length;
                p = weights.Select(x => x / sum * n).ToArray();
            }
            else {
                sum = weights.Where(x => x > ZeroThreshold).Sum();
                n = weights.Count(x => x / sum > ZeroThreshold);
                p = weights.Where(x => x / sum > ZeroThreshold).Select(x => x / sum * n).ToArray();
            }

            var prob = new float[n];
            var alias = new int[n];

            Array.Fill(prob, 0f);
            Array.Fill(alias, 0);

            var small = new Queue<int>();
            var large = new Queue<int>();

            foreach (var (pp, i) in p.Select((pp, i) => (pp, i))) {
                if (pp < 1) {
                    small.Enqueue(i);
                }
                else {
                    large.Enqueue(i);
                }
            }

            while (small.Count > 0 && large.Count > 0) {
                var l = small.Dequeue();
                var g = large.Dequeue();

                prob[l] = p[l];
                alias[l] = g;

                p[g] = p[g] + p[l] - 1;

                if (p[g] < 1) {
                    small.Enqueue(g);
                }
                else {
                    large.Enqueue(g);
                }
            }

            while (large.Count > 0) {
                var g = large.Dequeue();
                prob[g] = 1;
            }

            while (small.Count > 0) {
                var l = small.Dequeue();
                prob[l] = 1;
            }

            _n = n;
            _probabilities = prob;
            _alias = alias;
        }

        /// <summary>
        /// roll the dice
        /// </summary>
        /// <returns></returns>
        public int Roll() {
            var i = new Random().Next(_n);
            return new Random().NextDouble() < _probabilities[i] ? i : _alias[i];
        }
    }
}
