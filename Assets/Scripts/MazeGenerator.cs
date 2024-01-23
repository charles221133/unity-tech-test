using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private int _width;

    [SerializeField]
    private int _height;
    private int[,] _maze;
    private int[] _startingPoint = new int[2];
    private System.Random _rand = new System.Random();

    public void Awake()
    {
        _maze = new int[_width, _height];
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _maze[x, y] = 1; // Fill the maze with walls to start
            }
        }

        // Randomly choose a starting point for the player
        int startX = _rand.Next(0, _width / 2) * 2;
        int startY = _rand.Next(0, _height / 2) * 2;
        _startingPoint[0] = startX;
        _startingPoint[1] = startY;
        _maze[startX, startY] = 0;

        CreatePath(startX, startY);
    }

    private void CreatePath(int x, int y)
    {
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int[] directions = { 0, 1, 2, 3 };
            int random = _rand.Next(i, 4);
            (directions[i], directions[random]) = (directions[random], directions[i]);

            foreach (int dir in directions)
            {
                int nx = x + dx[dir] * 2;
                int ny = y + dy[dir] * 2;
                if (InRange(nx, ny) && _maze[nx, ny] == 1)
                {
                    _maze[nx, ny] = 0;
                    _maze[x + dx[dir], y + dy[dir]] = 0;
                    CreatePath(nx, ny);
                }
            }
        }
    }

    private bool InRange(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }

    /// <summary>
    /// 1 = blocking, 0 = path we can walk on
    /// </summary>
    public int[,] Maze
    {
        get { return _maze; }
    }

    public int[] StartingPoint
    {
        get { return _startingPoint; }
    }
}
