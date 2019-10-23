using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomGeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand1 = Random(123456789,100,101427,321,65536);
            var rand2 = Random(123456789,100,65539,0,2147483648);
            var rand3 = CollectCSharpRandom(123456789,100);
            bool rejected = KolmogorowSmirnow(rand1, 0.05);
        }

        private static bool KolmogorowSmirnow(List<double> rand1, double alpha)
        {
            return false;
        }


        static List<double> Random(long seed, int size, long a, long c, long m)
        {
            List<double> toReturn = new List<double>();
            long xi = seed;
            foreach (var i in Enumerable.Range(0, size))
            {
                xi = lcg(xi,a,c,m);
                double rand = ((double) xi) / ((double) (m));
                toReturn.Add(rand);
            }
            return toReturn;
        }
        static long lcg(long xi, long a, long c, long m)
        {
            return ((a * xi + c) % m);
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