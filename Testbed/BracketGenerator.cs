using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testbed
{
    public class Round
    {
        public List<Match> Matches { get; set; }
        public Round() { }
        public Round(List<Match> matches)
        {
            Matches = matches;  
        }
    }

    public class Match
    {
        // where number is player's seed number        
        public int Seed1 { get; set; }
        public int Seed2 { get; set; }

        public Match(int s1, int s2)
        {
            Seed1 = s1;
            Seed2 = s2;
        }
    }

    public class BracketGenerator
    {
        public enum PlayerNumber
        {
            p4 = 4,
            p8 = 8,
            p16 = 16,
            p32 = 32,
            p64 = 64
        }

        public static List<Round> Generate(PlayerNumber playersNumber)
        {
            // only works for power of 2 number of players   
            var roundsNumber = (int)Math.Log((int)playersNumber, 2);
            var rounds = new List<Round>(roundsNumber);

            for (int round = 0; round < roundsNumber; round++)
            {
                List<Match> matches = new List<Match>();    
                branch(1, 1, roundsNumber - round + 1, ref matches);
                rounds.Add(new Round(matches));
            }

            return rounds;
        }

        private static void branch(int seed, int level, int limit, ref List<Match> matches)
        {
            var levelSum = (int)Math.Pow(2, level) + 1;

            if (limit == level + 1)
            {
                matches.Add(new Match(seed, levelSum - seed));
                //Console.WriteLine("Seed {0} vs. Seed {1}", seed, levelSum - seed);
                return;
            }
            else if (seed % 2 == 1)
            {
                branch(seed, level + 1, limit, ref matches);
                branch(levelSum - seed, level + 1, limit, ref matches);
            }
            else
            {
                branch(levelSum - seed, level + 1, limit, ref matches);
                branch(seed, level + 1, limit, ref matches);
            }
        }
    }
}
