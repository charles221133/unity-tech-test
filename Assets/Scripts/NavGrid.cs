using System.Collections.Generic;
using UnityEngine;

// Uses Breadth First Search to find the fastest path between any 2 points on a grid.
public class NavGrid : MonoBehaviour
{
    private NavGridPathNode[,] _grid;

    // the 0,0 coordinates of the grid.
    [SerializeField]
    private Transform _grid00;

    /// <summary>
    /// Given the current and desired location, return a path to the destination
    /// </summary>
    public NavGridPathNode[] GetPath(NavGridPathNode startNode, NavGridPathNode endNode)
    {
        NavGridPathNode[] path = FindPath(_grid, startNode, endNode);
        return path;
    }

    private NavGridPathNode[] FindPath(NavGridPathNode[,] grid, NavGridPathNode startNode, NavGridPathNode endNode)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        bool[,] visited = new bool[width, height];
        Dictionary<NavGridPathNode, NavGridPathNode?> parentMap = new Dictionary<NavGridPathNode, NavGridPathNode?>();
        Queue<NavGridPathNode> queue = new Queue<NavGridPathNode>();

        queue.Enqueue(startNode);
        visited[startNode.X, startNode.Z] = true;
        parentMap[startNode] = null;

        while (queue.Count > 0)
        {
            NavGridPathNode currentNode = queue.Dequeue();
            if (currentNode.X == endNode.X && currentNode.Z == endNode.Z)
            {
                List<NavGridPathNode> list = ConstructPath(currentNode, parentMap);
                return list.ToArray();
            }

            var neighbors = GetNeighbors(currentNode, grid, visited);
            foreach (var neighbor in neighbors)
            {
                queue.Enqueue(neighbor);
                visited[neighbor.X, neighbor.Z] = true;
                parentMap[neighbor] = currentNode;
            }
        }    

        return new NavGridPathNode[0]; // if we fail to find a path, return an empty list
    }

    private static IEnumerable<NavGridPathNode> GetNeighbors(NavGridPathNode node, NavGridPathNode[,] grid, bool[,] visited)
    {
        // for checking all directions Up, Down, Left, Right
        var directions = new (int, int)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

        foreach (var (dx, dz) in directions)
        {
            int newX = node.X + dx;
            int newZ = node.Z + dz;

            if (newX >= 0 && newX < grid.GetLength(0) && newZ >= 0 && newZ < grid.GetLength(1) && !visited[newX, newZ] && !grid[newX, newZ].blocking)
            {
                yield return grid[newX, newZ];
            }
        }
    }

    private static List<NavGridPathNode> ConstructPath(NavGridPathNode endNode, Dictionary<NavGridPathNode, NavGridPathNode?> parentMap)
    {
        List<NavGridPathNode> path = new List<NavGridPathNode>();
        NavGridPathNode? currentNode = endNode;

        while (currentNode.HasValue)
        {
            path.Add(currentNode.Value);
            currentNode = parentMap[currentNode.Value];
        }

        path.Reverse();
        return path;
    }

    public NavGridPathNode[,] Grid
    {
        set { _grid = value; }
        get { return _grid; }
    }
}
