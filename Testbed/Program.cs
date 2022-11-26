using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Testbed;

//Store all Players
List<int> players = new List<int>() { 1, 2, 3, 4, 5 };
List<Tuple<int, int>> mapping = new List<Tuple<int, int>>();
int i = 0;
players.ForEach(p => mapping.Add(new Tuple<int, int> (i, p)));

int iSeed = 0;
Random random = new Random();
while (players.Count > 0)
{
    int randomPlayer = players.ElementAt(random.Next(players.Count));
    mapping.ElementAt(iSeed++).Item2 = randomPlayer;
    players.Remove(randomPlayer);
}

await _context.SaveChangesAsync();

var rounds = BracketGenerator.Generate(BracketGenerator.PlayerNumber.p64);
foreach (var round in rounds)
{
    foreach (var match in round.Matches)
    {
        Console.WriteLine("{0} vs {1}", match.Seed1, match.Seed2);
    }
    Console.WriteLine();
}
Console.ReadKey();