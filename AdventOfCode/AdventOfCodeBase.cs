global using JM.LinqFaster;
global using System.IO;
global using System.Linq;
global using Xunit;
global using System;
global using System.Collections.Generic;

namespace AdventOfCode;

/// <summary>
/// Basic advent of code functionality for the day answers
/// </summary>
public abstract class AdventOfCodeBase
{
    protected string InputFilename => $"Inputs\\{this.GetType().Name}.txt";
    protected string InputExampleFilename => $"Inputs\\{this.GetType().Name}_example.txt";

    /// <summary>
    /// Returns the answer part one
    /// </summary>
    /// <returns>string</returns>
    public abstract string AnswerPartOne();

    /// <summary>
    /// Returns the answer part one
    /// </summary>
    /// <returns>string</returns>
    public abstract string AnswerPartTwo();

}