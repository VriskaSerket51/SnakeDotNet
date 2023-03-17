namespace SnakeDotNet;

public class Game
{
    private readonly Player _player = new();
    private readonly World _world = new();

    public static Game Instance { get; } = new();

    public bool IsGameOver { get; private set; }

    public void Init()
    {
        var playerPosition = _world.SetGridRandom(Constant.GridType.Head);
        _player.Init(playerPosition.Item1, playerPosition.Item2);
        _player.OnPlayerMove += OnPlayerMove;

        _world.SetGridRandom(Constant.GridType.Item);

        Thread.Sleep(200);
        while (true)
        {
            if (IsGameOver)
            {
                break;
            }

            Thread.Sleep(Constant.UpdateDelay);
            _player.Tick();
            _world.Draw();
        }
    }

    private void OnPlayerMove(List<Body> bodies)
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            Body body = bodies[i];
            if (i == 0)
            {
                int[] headPosition = body.GetPosition();
                Constant.GridType grid;
                try
                {
                    grid = _world.GetGrid(headPosition[0], headPosition[1]);
                }
                catch (Exception)
                {
                    OnGameOver();
                    return;
                }

                if (grid == Constant.GridType.Item)
                {
                    Body lastBody = bodies[^1];
                    int[] lastBodyPosition = lastBody.GetOldPosition();
                    _player.AddBody(lastBodyPosition[0], lastBodyPosition[1]);

                    _world.SetGridRandom(Constant.GridType.Item);
                }
                else if (grid == Constant.GridType.Body)
                {
                    OnGameOver();
                    return;
                }

                _world.SetGrid(headPosition[0], headPosition[1], Constant.GridType.Head);
            }
            else
            {
                int[] position = body.GetPosition();
                _world.SetGrid(position[0], position[1], Constant.GridType.Body);
            }
            int[] bodyOldPosition = body.GetOldPosition();
            _world.SetGrid(bodyOldPosition[0], bodyOldPosition[1], Constant.GridType.None);
        }
    }

    private void OnGameOver()
    {
        IsGameOver = true;
        Console.WriteLine("GAME OVER!!!");
        Console.WriteLine($"Your score: {_player.Score}");
        Console.WriteLine("Press ANY key to leave...");
        Console.ReadKey();
    }
}