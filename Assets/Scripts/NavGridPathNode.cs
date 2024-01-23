using UnityEngine;

public struct NavGridPathNode
{
    public int X;
    public int Z;

    /// <summary>
    /// World position of the node
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// can we move through this tile or does it block our movement?
    /// </summary>
    public bool blocking;

    public NavGridPathNode(int x, int z, Vector3 position, bool blocks)
    {
        X = x;
        Z = z;
        Position = position;
        blocking = blocks;
    }
}