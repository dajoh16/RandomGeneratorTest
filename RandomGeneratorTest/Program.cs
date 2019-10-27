using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomGeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRand = new List<double>();
            testRand.Add(0.44);
            testRand.Add(0.81);
            testRand.Add(0.14);
            testRand.Add(0.05);
            testRand.Add(0.93);
            var rand1 = Random(123456789,100,101427,321,65536);
            var rand2 = Random(123456789,100,65539,0,2147483648);
            var rand3 = CollectCSharpRandom(123456789,100);
            bool rejected = KolmogorowSmirnow(testRand);
            Console.WriteLine("Test Random returned " + rejected );
            rejected = KolmogorowSmirnow(rand1);
            Console.WriteLine("LCG 1 returned " + rejected );
            rejected = KolmogorowSmirnow(rand2);
            Console.WriteLine("LCG 2 returned " + rejected );
            rejected = KolmogorowSmirnow(rand3);
            Console.WriteLine("C# implementation returned " + rejected );
        }

        /**
         * Returns true if the random sequence is rejected and fails the kolmogorow smirnow test
         * Uses the standard Alpha value of 0.05
         */
        private static bool KolmogorowSmirnow(List<double> rand1)
        {
            rand1.Sort();
            List<double> dPositives = new List<double>();
            List<double> dNegatives = new List<double>();
            foreach (var i in rand1)
            {
                dPositives.Add(((rand1.IndexOf(i)+1.0) / rand1.Count) - i);
                dNegatives.Add(i-(((rand1.IndexOf(i)+1.0)-1)/rand1.Count));
            }
            double dNegative = dNegatives.Max();
            double dPositive = dPositives.Max();
            double d = Math.Max(dNegative, dPositive);
            double dAlpha = 1.36 / Math.Sqrt(rand1.Count);
            return dAlpha < d;
        }


        static List<double> Random(long seed, int size, long a, long c, long m)
        {
            List<double> toReturn = new List<double>();
            long xi = seed;
            foreach (var i in Enumerable.Range(0, size))
            {
                xi = lcg(xi,a,c,m);
                double rand = ((double) xi) /  m;
                toReturn.Add(rand);
            }
            return toReturn;
        }
        static long lcg(long xi, long a, long c, long m)
        {
            return (a * xi + c) % m;
        }
        
        private static List<double> CollectCSharpRandom(int seed, int size)
        {
            List<double> toReturn = new List<double>();
            Random random = new Random(seed);
            foreach (var i in Enumerable.Range(0, size))
            {
                var rand = random.NextDouble();
                toReturn.Add(rand);
            }
            return toReturn;
        }
    }
    
}