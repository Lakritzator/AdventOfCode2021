# Advent of Code 2021

This is the solution to the [Advent of Code 2021](https://adventofcode.com/2021/about) puzzles, written in C#
Every time I finish a solution, I will push it to the repository.

I use some of the newest C# language features (version 10), so it will most likely only work with Visual Studio 2022!

The repository is structured as follows:

* In the "root" you can find this README.md and the AdventOfCode.sln to open with VS 2022
* In the [AdventOfCode](https://github.com/Lakritzator/AdventOfCode2021/tree/main/AdventOfCode) directory you will find the main Program.cs and the AdventOfCodeBase class
  * In the [Inputs](https://github.com/Lakritzator/AdventOfCode2021/tree/main/AdventOfCode/Inputs) you will find my personal inputs for every day, including (mostly) the examples. Filenames are DayXX.txt (where XX is the day number in 2 digits)
  * In the [Solutions](https://github.com/Lakritzator/AdventOfCode2021/tree/main/AdventOfCode/Solutions) directory you will find the implementation of each day (Day01.cs, Day02.cs etc.), which implements the AdventOfCodeBase and has two answers (if I finished them)

Run the programm, it will automatically start all implemented days and show the answers in the console.
(You could just copy your inputs to the corresponding input file to get your answer.)
