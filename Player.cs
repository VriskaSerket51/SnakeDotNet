using System.Collections.Concurrent;

namespace SnakeDotNet;

public class Player
{
    private readonly List<Body> _bodies = new();

    // Left : 0, Up : 1, Right : 2, Down : 3
    private readonly ConcurrentQueue<int> _directionBuffer = new();
    private int _lastDirection;

    public int Score => _bodies.Count;

    public event Action<List<Body>>? OnPlayerMove;

    public void Init(int x, int y)
    {
        _directionBuffer.Enqueue(2);
        _lastDirection = 2;

        AddBody(x, y);

        Thread readKey = new Thread(ReadKey);
        readKey.Start();
    }

    public void AddBody(int x, int y)
    {
        Body body = new Body();
        body.SetPosition(x, y);
        _bodies.Add(body);
    }

    public void Tick()
    {
        lock (_directionBuffer)
        {
            if (_directionBuffer.Count > 1)
            {
                _directionBuffer.TryDequeue(out int direction);
                Move(direction);
            }
            else if (_directionBuffer.TryPeek(out int lastDirection))
            {
                Move(lastDirection);
            }

            OnPlayerMove?.Invoke(_bodies);
        }
    }

    private void Move(int direction)
    {
        for (int i = _bodies.Count - 1; i >= 0; i--)
        {
            Body body = _bodies[i];
            if (i == 0)
            {
                int[] position = body.GetPosition();
                int dx = 0;
                int dy = 0;
                switch (direction)
                {
                    case 0:
                        dx -= 1;
                        break;
                    case 1:
                        dy -= 1;
                        break;
                    case 2:
                        dx += 1;
                        break;
                    case 3:
                        dy += 1;
                        break;
                }

                body.SetPosition(position[0] + dx, position[1] + dy);
            }
            else
            {
                Body beforeBody = _bodies[i - 1];
                int[] beforePosition = beforeBody.GetPosition();
                body.SetPosition(beforePosition[0], beforePosition[1]);
            }
        }
    }

    private void ReadKey()
    {
        while (true)
        {
            if (Game.Instance.IsGameOver)
            {
                break;
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            int key = (int)keyInfo.Key - 37;
            if (key is >= 0 and <= 3)
            {
                if (_lastDirection != key && Math.Abs(_lastDirection - key) != 2)
                {
                    _lastDirection = key;
                    _directionBuffer.Enqueue(key);
                }
            }
        }
    }
}