using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private NavGridPathNode[] _currentPath = Array.Empty<NavGridPathNode>();
    private int _currentPathIndex = 0;
    private NavGridPathNode _currentNode;

    [SerializeField]
    private NavGrid _grid;

    [SerializeField]
    private float _speed = 10.0f;

    void Update()
    {
        // Traverse
        if (_currentPathIndex < _currentPath.Length)
        {
            _currentNode = _currentPath[_currentPathIndex];
            Vector3 targetPoint = _currentNode.Position;

            if (_currentPathIndex < _currentPath.Length - 1)
            {
                Vector3 nextNode = _currentPath[_currentPathIndex + 1].Position;
                float lookAhead = 0.5f;
                targetPoint = Vector3.Lerp(targetPoint, nextNode, lookAhead);
            }

            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

            float distanceToNode = Vector3.Distance(transform.position, _currentNode.Position);
            bool areOnLastNode = (_currentPathIndex == _currentPath.Length - 1);

            // we check if we're arrived at our Node like this to prevent overshooting errors.
            if ( (areOnLastNode && distanceToNode < .01f) || (!areOnLastNode && distanceToNode < 1f) )
            {
                _currentPathIndex++;
            }
        }

    }

    public void MoveToPointSelected(NavGridPathNode node)
    {
        _currentPath = _grid.GetPath(CurrentNode, node);
        _currentPathIndex = 0;
    }

    public NavGridPathNode CurrentNode
    {
        set { _currentNode = value; }
        get { return _currentNode; }
    }
}
