using AdventOfCode;

Console.WriteLine("Advent of code 2021 answers!");

for(int day = 1; day < 25; day++)
{
    var typeForSolution = Type.GetType($"AdventOfCode.Solutions.Day{day:D2}", false);
    if (typeForSolution == null)
    {
        continue;
    }
    var daySolution = Activator.CreateInstance(typeForSolution) as AdventOfCodeBase;
    if(daySolution == null) { continue; }

    Console.WriteLine($"For day {day}");
    Console.WriteLine($"\tthe part one answer is: {daySolution.AnswerPartOne()}");
    Console.WriteLine($"\tthe part two answer is: {daySolution.AnswerPartTwo()}");
}
Console.WriteLine("Ready.");
Console.ReadLine();
