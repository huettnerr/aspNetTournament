using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testbed
{
    internal class MatchFactory
    {
        public static void OrderMatches(List<Match> matches)
        {

        }

        public static List<Match> CreateMatches(Round r)
        {
            List<Match> matches = new List<Match>();

            foreach (Group g in r.Groups)
            {
                List<Match> tmpList = scheduleRoundRobin(g.Seeds);
                matches.AddRange(tmpList);
            }

            return matches;
        }

        private static List<Match> scheduleRoundRobin(List<int> seeds)
        {
            List<Match> matches = new List<Match>();

            foreach (Tuple<int, int> pair in CreatePairs(seeds.Count))
            {
                matches.Add(new Match()
                {
                    Seed1 = seeds.ElementAt(pair.Item1 - 1),
                    Seed2 = seeds.ElementAt(pair.Item2 - 1),
                });
            }

            return matches;
        }

        private static List<Tuple<int, int>> CreatePairs(int n)
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            int[] orig = new int[n];
            for (int i = 0; i < n; i++)
            {
                orig[i] = i + 1;
            }
            IEnumerable<int> rev = orig.Reverse();

            int len = orig.Length;
            for (int j = 0; j < len - 1; j++)
            {
                List<int> tmp = new List<int>();
                tmp.Add(orig[0]);
                tmp.AddRange(rev.Take(j).Reverse());
                if (j < len && len > 1 + j) tmp.AddRange(orig.Skip(1).Take(len - 1 - j));
                pairs.AddRange(makeMatches(tmp, j + 1));
            }

            return pairs;
        }

        private static List<Tuple<int, int>> makeMatches(IEnumerable<int> arr, int round)
        {
            int halfSize = arr.Count() / 2;

            IEnumerable<int> A = arr.Take(halfSize);
            IEnumerable<int> B = arr.Skip(halfSize).Take(halfSize).Reverse();

            return A.Zip(B, (x, y) => new Tuple<int, int>(x, y)).ToList();
        }
    }
}
