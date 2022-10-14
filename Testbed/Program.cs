// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

Console.WriteLine("Hello, World!");

Console.WriteLine(RoundType.RoundRobin);
Console.WriteLine(RoundType.SingleKo);
Console.WriteLine(RoundType.DoubleKo);
RoundType val = (RoundType)Enum.Parse(typeof(RoundType), "RoundRobin");
Console.WriteLine(val); 

public enum RoundType
{
    RoundRobin,
    SingleKo,
    DoubleKo
}