﻿namespace AdventOfCode
{
    /// <summary>
    /// Basic advent of code fuctionality for the day answers
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
}
