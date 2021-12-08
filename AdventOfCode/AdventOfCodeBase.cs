global using JM.LinqFaster;
global using System.IO;
global using System.Linq;
global using Xunit;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Text;

namespace AdventOfCode;

/// <summary>
/// Basic advent of code functionality for the day answers
/// </summary>
public abstract class AdventOfCodeBase
{
    protected string InputFilename => $"Inputs\\{this.GetType().Name}.txt";
    protected string InputExampleFilename => $"Inputs\\{this.GetType().Name}_example.txt";
    protected string InputExampleMiniFilename => $"Inputs\\{this.GetType().Name}_example_mini.txt";

    /// <summary>
    /// Returns the answer part one
    /// </summary>
    /// <returns>string</returns>
    public abstract string AnswerPartOne();

    /// <summary>
    /// Returns the answer part one
    /// </summary>
    /// <returns>string</returns>
    public virtual string AnswerPartTwo() => "Answer 2: Not implemented yet.";

}