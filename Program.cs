using System;
using System.Collections.Generic;
using System.IO;

namespace EntropyBuddy
{
    class Program
    {
        static void Main(string[] filepath)
        {

            if (filepath.Length == 0 || null == filepath)
            {
                System.Console.WriteLine("Please enter params!");
            }
            else
            {
                string contents = File.ReadAllText(filepath[0]);
                double entropyNess = ShannonEntropy(contents);
                System.Console.WriteLine(entropyNess.ToString());
            }

        }



        //https://stackoverflow.com/questions/990477/how-to-calculate-the-entropy-of-a-file
        public static double ShannonEntropy(string s)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in s)
            {
                if (!map.ContainsKey(c))
                    map.Add(c, 1);
                else
                    map[c] += 1;
            }

            double result = 0.0;
            int len = s.Length;
            foreach (var item in map)
            {
                var frequency = (double)item.Value / len;
                result -= frequency * (Math.Log(frequency) / Math.Log(2));
            }

            return result;
        }

    }
}
