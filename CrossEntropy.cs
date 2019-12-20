using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntropyBuddy
{
    class CrossEntropy
    {
        Random r = new Random();
        double objective_function(double[] vector)
        {
            double sum = 0f;
            foreach (var f in vector)
            {
                sum += (double)Math.Pow(f, 2);
            }
            return -sum;
        }

        double QuadraticEquation(double[] vector)
        {
            // 5X^2 + 10X - 2 = 0 -> X=-2.183216 || X=0.183216
            double sum = 5 * Math.Pow(vector[0], 2) + 10 * vector[0] - 2;
            return -Math.Abs(sum);
        }
        double QuadraticEquation2(double[] vector)
        {
            // 5X^2 + 10X - 2 = 0 -> X=-2.183216 || X=0.183216
            double sum1 = vector[0] * Math.Pow(0.183216, 2) + vector[1] * 0.183216 + vector[2];
            double sum2 = vector[0] * Math.Pow(-2.183216, 2) + vector[1] * -2.183216 + vector[2];
            return -(Math.Abs(sum1) + Math.Abs(sum2));
        }

        double random_variable(double min, double max)
        {
            return min + ((max - min) * r.NextDouble());
        }

        double random_gaussian(double mean = 0.0, double stdev = 1.0)
        {
            double u1, u2, w;
            u1 = u2 = w = 0;
            do
            {
                u1 = 2 * r.NextDouble() - 1;
                u2 = 2 * r.NextDouble() - 1;
                w = u1 * u1 + u2 * u2;
            } while (w >= 1);

            w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
            return mean + (u2 * w) * stdev;
        }

        double[] generate_sample(double[][] search_space, double[] means, double[] stdevs)
        {
            double[] vector = new double[search_space.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = random_gaussian(means[i], stdevs[i]);
                vector[i] = Math.Max(vector[i], search_space[i][0]);
                vector[i] = Math.Min(vector[i], search_space[i][1]);
            }

            return vector;
        }

        void update_distribution(double[][] samples, double alpha, ref double[] means, ref double[] stdevs)
        {
            for (int i = 0; i < means.Length; i++)
            {
                double[] tArray = new double[samples.Length];
                for (int z = 0; z < samples.Length; z++)
                {
                    tArray[z] = samples[z][i];
                }
                means[i] = alpha * means[i] + ((1.0 - alpha) * tArray.Average());

                stdevs[i] = alpha * stdevs[i] + ((1.0 - alpha) * standardDeviation(tArray));
            }
        }

        double standardDeviation(double[] array)
        {
            double average = array.Average();
            double sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / array.Length);

            return sd;
        }

        double[] search(double[][] bounds, int max_iter, int num_samples, int num_update, double learning_rate)
        {
            double[] means = new double[bounds.Length];
            double[] stdevs = new double[bounds.Length];
            for (int i = 0; i < means.Count(); i++)
            {
                means[i] = random_variable(bounds[i][0], bounds[i][1]);
                stdevs[i] = bounds[i][1] - bounds[i][0];
            }
            double[] best = null;
            double bestScore = double.MinValue;
            for (int t = 0; t < max_iter; t++)
            {
                double[][] samples = new double[num_samples][];
                double[] scores = new double[num_samples];
                for (int s = 0; s < num_samples; s++)
                {
                    samples[s] = generate_sample(bounds, means, stdevs);
                    scores[s] = QuadraticEquation(samples[s]);
                }
                Array.Sort(scores, samples);
                Array.Reverse(scores);
                Array.Reverse(samples);
                if (best == null || scores.First() > bestScore)
                {
                    bestScore = scores.First();
                    best = samples.First();
                }
                double[][] selected = new double[num_update][];
                Array.Copy(samples, selected, num_update);
                update_distribution(selected, learning_rate, ref means, ref stdevs);
                Console.WriteLine("iteration={0}, fitness={1}", t, bestScore);
            }
            return best;
        }
        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static String ToBinary(Byte[] data)
        {
            return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }

        public void RunCrossEntropy(string content)
        {
            double[][] parameters = new double[][] { new double[] { -222, 500 } }; //QuadraticEquation parameters
                                                                                   //double[][] parameters = new double[][] { new double[] { 4, 6 }, new double[] { 9, 11 }, new double[] { -3, -1} }; //QuadraticEquation2 parameters
                                                                                   //double[][] parameters = new double[][] { new double[] { -5, 5 }, new double[] { -5, 5 }, new double[] { -5, 5 } }; //object_function parameters
            
            // take the first 1000 characters 
            // convert to binary

            foreach (char c in content)
            {
                string s = c.ToString();
                string binaryString = ToBinary(ConvertToByteArray(s, Encoding.ASCII));
                long v = 0;
                for (int i = binaryString.Length - 1; i >= 0; i--) v = (v << 1) + (binaryString[i] - '0');
                double d = BitConverter.ToDouble(BitConverter.GetBytes(v), 0);



                // parameters.Add(stupid1, stupid2);

            }

           

          
            int maxIter = 100;
            int nSamples = 50;
            int nUpdate = 5;
            double alpha = 1;
            double[] best = search(parameters, maxIter, nSamples, nUpdate, alpha);
            string str = string.Join(" | ", best.Select(a => a.ToString("N10")).ToArray());
            Console.WriteLine("Best: " + str);
        }
    }
}
