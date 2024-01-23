using UnityEngine;

// runs most of the logic for the level
public class LevelController : MonoBehaviour
{
    // used to build walls
    [SerializeField]
    private GameObject _blockPrefab;

    // used to build paths we can walk on
    [SerializeField]
    private GameObject _pathTilePrefab;

    // the x=0, y=0 of the maze, we will place everything in the maze based on this starting point
    [SerializeField]
    private Transform _maze00;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private NavGrid _navGrid;

    [SerializeField]
    private MazeGenerator mazeGenerator;

    [SerializeField]
    private float _spaceBetweenNodesOnGrid = 1.5f;

    void Start()
    {
        int[,] maze = mazeGenerator.Maze;

        _navGrid.Grid = SetupMazePieces(maze);

        PutPlayerAtStartingPoint(mazeGenerator.StartingPoint, _navGrid.Grid);
    }

    private void PutPlayerAtStartingPoint(int[] start, NavGridPathNode[,] grid)
    {
        NavGridPathNode node = grid[start[0], start[1]];

        Vector3 position = _player.transform.position;
        _player.transform.position = new Vector3(node.Position.x, node.Position.y, node.Position.z);
        _player.CurrentNode = node;
    }

    private NavGridPathNode[,] SetupMazePieces(int[,] maze)
    {
        NavGridPathNode[,] grid = new NavGridPathNode[33, 33];

        for (int i = 0; i < maze.GetLength(1); i++)
        {
            for (int j = 0; j < maze.GetLength(0); j++)
            {
                bool blocking = maze[i, j] == 1;

                Vector3 thePosition = GetGridPosition(i, j);
                NavGridPathNode node = new NavGridPathNode(i, j, thePosition, blocking);
                node.blocking = blocking;

                grid[i, j] = node;

                if (blocking)
                {
                    // placing blocks to build the walls of the maze.
                    Instantiate(_blockPrefab, node.Position, Quaternion.identity);
                } 
                else
                {
                    // place tiles we can walk on
                    GameObject tile = Instantiate(_pathTilePrefab, node.Position, Quaternion.identity);
                    TileController controller = tile.GetComponent<TileController>();
                    controller.Player = _player;
                    controller.Node = node;
                }
            }
        }

        return grid;
    }

    private Vector3 GetGridPosition(int x, int y)
    {
        return new Vector3(_maze00.position.x + _spaceBetweenNodesOnGrid * x, _maze00.position.y, _maze00.position.z + _spaceBetweenNodesOnGrid * y);
    }
}
