namespace AdventOfCode.Support;

public class Map2D<T>
{
    private readonly T[,] _map;
    private readonly int _height, _width;

    public Map2D(int width, int height)
    {
        _width = width;
        _height = height;
        _map = new T[width, height];
    }

    public Map2D<T> CreateEmptyLike()
    {
        return new Map2D<T>(_width, _height);
    }

    public int Height => _height;
    public int Width => _width;

    public bool IsInMap(Point p) => IsInMap(p.X, p.Y);

    public bool IsInMap(int x, int y)
    {
        if (x >= _width || x < 0)
        {
            return false;
        }

        if (y >= _height || y < 0)
        {
            return false;
        }

        return true;
    }

    public T this[Point p]
    {
        get => this[p.X, p.Y];
        set => this[p.X, p.Y] = value;
    }

    public T this[int x, int y]
    {
        get
        {
            if (!IsInMap(x, y))
            {
                throw new IndexOutOfRangeException($"{x},{y} doesn't fit in {_width},{_height}");
            }
            return _map[x, y];
        }
        set
        {
            if (!IsInMap(x, y))
            {
                throw new IndexOutOfRangeException($"{x},{y} doesn't fit in {_width},{_height}");
            }
            _map[x, y] = value;
        }
    }

    /// <summary>
    /// A simple Bresenham line routine, I once wrote this for the C64, Amiga and PC in Assembly...
    /// Now I just copied it...
    /// </summary>
    /// <param name="line">Line</param>
    /// <param name="valueFunc">Func to create the new value</param>
    public void DrawLine(Line line, Func<T,T> valueFunc)
    {
        int x = line.P1.X;
        int y = line.P1.Y;
        int w = line.P2.X - line.P1.X;
        int h = line.P2.Y - line.P1.Y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        dx1 = w switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dx1
        };
        dy1 = h switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dy1
        };
        dx2 = w switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => dx2
        };
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            dy2 = h switch
            {
                < 0 => -1,
                > 0 => 1,
                _ => dy2
            };
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            _map[x, y] = valueFunc(_map[x,y]);
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }

}