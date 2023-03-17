namespace SnakeDotNet;

public class Body
{
    private readonly int[] _position = new int[2];
    private readonly int[] _oldPosition = new int[2];

    public int[] GetPosition()
    {
        return _position;
    }

    public void SetPosition(int x, int y)
    {
        _oldPosition[0] = _position[0];
        _oldPosition[1] = _position[1];

        _position[0] = x;
        _position[1] = y;
    }

    public int[] GetOldPosition()
    {
        return _oldPosition;
    }
}