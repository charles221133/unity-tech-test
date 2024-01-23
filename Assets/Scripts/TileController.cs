using UnityEngine;

// this is a walkable space on the grid.
public class TileController : MonoBehaviour
{
    // the node within the grid that represents this tile.
    private NavGridPathNode _node;
    private Player _player;

    public Player Player
    {
        set { _player = value; }
        get { return _player; }
    }

    public NavGridPathNode Node
    {
        set { _node = value; }
        get { return _node; }
    }

    private void OnMouseUpAsButton()
    {
        Player.MoveToPointSelected(Node);
    }
}
