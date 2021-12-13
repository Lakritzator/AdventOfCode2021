namespace AdventOfCode.Support;

public record Point(int X, int Y)
{
    public IEnumerable<Point> PointsAllAround()
    {
        yield return LeftAbove();
        yield return Above();
        yield return RightAbove();
        yield return Left();
        yield return Right();
        yield return LeftUnder();
        yield return Under();
        yield return RightUnder();
    }

    public IEnumerable<Point> PointsAround()
    {
        yield return Above();
        yield return Left();
        yield return Right();
        yield return Under();
    }

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

    public Point LeftAbove() => Left().Above();
    public Point RightAbove() => Right().Above();

    public Point Under() => this with
    {
        Y = Y + 1
    };

    public Point LeftUnder() => Left().Under();
    public Point RightUnder() => Right().Under();

    /// <summary>
    /// Parse a point from a string with x,y
    /// </summary>
    /// <param name="pointInformation">string to parse</param>
    /// <returns>Point</returns>
    public static Point Parse(string pointInformation)
    {
        var pointSplit = pointInformation.SplitClean(',');
        return new Point(int.Parse(pointSplit[0]), int.Parse(pointSplit[1]));
    }
}