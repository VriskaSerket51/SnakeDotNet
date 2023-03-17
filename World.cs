namespace SnakeDotNet;

public class World
{
    private readonly Constant.GridType[,] _grids = new Constant.GridType[Constant.GridWidth, Constant.GridHeight];
    private readonly Random _random = new();

    public void Draw()
    {
        Console.Clear();
        for (int y = 0; y < Constant.GridHeight; y++)
        {
            for (int x = 0; x < Constant.GridWidth; x++)
            {
                switch (_grids[x, y])
                {
                    case Constant.GridType.None:
                        Console.Write(Constant.None);
                        break;
                    case Constant.GridType.Head:
                        Console.Write(Constant.Head);
                        break;
                    case Constant.GridType.Item:
                        Console.Write(Constant.Item);
                        break;
                    case Constant.GridType.Body:
                        Console.Write(Constant.Body);
                        break;
                }

                if (x == Constant.GridWidth - 1)
                {
                    Console.WriteLine();
                }
            }
        }
    }

    public Constant.GridType GetGrid(int x, int y)
    {
        return _grids[x, y];
    }

    public void SetGrid(int x, int y, Constant.GridType gridType)
    {
        _grids[x, y] = gridType;
    }

    public Tuple<int, int> SetGridRandom(Constant.GridType gridType)
    {
        List<Tuple<int, int>> availableGrid = new(Constant.GridWidth * Constant.GridHeight);

        for (int y = 0; y < Constant.GridHeight; y++)
        {
            for (int x = 0; x < Constant.GridWidth; x++)
            {
                if (_grids[x, y] == Constant.GridType.None)
                {
                    availableGrid.Add(new Tuple<int, int>(x, y));
                }
            }
        }

        int i = _random.Next(availableGrid.Count);
        SetGrid(availableGrid[i].Item1, availableGrid[i].Item2, gridType);
        return availableGrid[i];
    }
}