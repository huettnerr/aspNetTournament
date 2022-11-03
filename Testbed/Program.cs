using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Testbed;

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