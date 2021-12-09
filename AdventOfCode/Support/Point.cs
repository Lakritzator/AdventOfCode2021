using System.Runtime.InteropServices;

namespace AdventOfCode.Support;

public record Point(int X, int Y)
{
    public Point Left() => this with
    {
        X = X - 1
    };
    public Point Right() => this with
    {
        X = X + 1
    };
    public Point Above() => this with
    {
        Y = Y - 1
    };
    public Point Under() => this with
    {
        Y = Y + 1
    };
}