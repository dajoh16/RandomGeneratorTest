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
            
            var rand1Full = Random(123456789,10000,101427,321,65536);
            var rand2Full = Random(123456789,10000,65539,0,2147483648);
            var rand3Full = CollectCSharpRandom(123456789,10000);
            
            var reject = RunsTest(rand1Full, 11.070);
            Console.WriteLine("LCG 1 returned " + reject );
            reject = RunsTest(rand2Full, 15.507);
            Console.WriteLine("LCG 2 returned " + reject );
            reject = RunsTest(rand3Full,14.067);
            Console.WriteLine("C# implementation returned " + reject );

        }
        /**
        * Returns true if the runs test is failed
        */
        private static bool RunsTest(List<double> rand, double chi)
        {
            double last = 0.0;
            bool ascending = true;
            bool descending = false;
            int run = -1;
            List<int> runs = new List<int>(new int[100]);
            foreach (var i in rand)
            {
                if (i >= last && ascending)
                {
                    last = i;
                    run++;
                }
                else if(i < last && ascending)
                {
                    last = i;
                    runs[run] = runs[run] + 1;
                    run = 1;
                    ascending = false;
                    descending = true;
                } else if (i >= last && descending)
                {
                    last = i;
                    runs[run] = runs[run] + 1;
                    run = 1;
                    descending = false;
                    ascending = true;
                } else if (i < last && descending)
                {
                    last = i;
                    run++;
                }
            }
            runs[run] = runs[run] + 1;

            runs.RemoveAll(i => i == 0);
            if (runs[0] == 1)
            {
                runs.RemoveAt(0);
            }
            runs.Insert(0,0);
            List<double> expectedRuns = new List<double>();
            foreach (var observedRuns in runs)
            {
                int runLength = runs.IndexOf(observedRuns);
                expectedRuns.Add(ExpectedRun(runLength, rand.Count));    
            }

            double x0pow2 = 0;
            for (int i = 1; i < runs.Count; i++)
            {
                x0pow2 += (Math.Pow(expectedRuns[i] - runs[i], 2)) / expectedRuns[i];
            }

            
            return x0pow2 > chi;
        }

        private static double ExpectedRun(int runLength, int randCount)
        {
            var factorial = Factorial(runLength + 3);
            var division = (double)2 / factorial;
            var bigFormula = randCount*(Math.Pow(runLength,2)+3*runLength+1)-(Math.Pow(runLength,3)+3*Math.Pow(runLength,2)-runLength-4);
            return division * Math.Abs(bigFormula);
        }
        public static int Factorial(int f)
        {
            if(f == 0)
                return 1;
            else
                return f * Factorial(f-1); 
        }

        /**
         * Returns true if the random sequence is rejected and fails the kolmogorow smirnow test
         * Uses the standard Alpha value of 0.05 (95% Confidence interval)
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