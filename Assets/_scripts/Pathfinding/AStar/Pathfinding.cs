using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.AI.Pathfinding
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 999;

        private List<Node> nodes = new List<Node>();
        private List<Node> openList;
        private List<Node> closedList;

        public void AddNode(Vector3 _position, List<Vector3> _neighbours)
        {
            Node node = new Node(_position, _neighbours);
            nodes.Add(node);
        }

        public Node GetNode(Vector3 _position) => nodes.SingleOrDefault(x => x.position == _position);

        public List<Node> FindPath(Vector3 _start, Vector3 _end)
        {
            Node start = GetNode(_start);
            Node end = GetNode(_end);

            openList = new List<Node> { start };
            closedList = new List<Node>();

            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                node.g = int.MaxValue;
                node.CalculateFCost();
                node.cameFrom = null;
            }

            start.g = 0;
            start.h = CalculateDistanceCost(start, end);
            start.CalculateFCost();

            while(openList.Count > 0)
            {
                Node current = GetLowestFCostNode(openList);
                if (current == end) { return CalculatePath(end); }

                openList.Remove(current);
                closedList.Add(current);

                foreach (Node neighbour in GetNeighbourList(current))
                {
                    if (closedList.Contains(neighbour)) { continue; }

                    int tentativeGCost = current.g + CalculateDistanceCost(current, neighbour);
                    if (tentativeGCost < neighbour.g)
                    {
                        neighbour.cameFrom = current;
                        neighbour.g = tentativeGCost;
                        neighbour.h = CalculateDistanceCost(neighbour, end);
                        neighbour.CalculateFCost();

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            // PATH FAILED
            return null;
        }

        private List<Node> GetNeighbourList(Node _current)
        {
            List<Node> neighbours = new List<Node>();
            for (int i = 0; i < nodes.Count; i++)
            {
                if (_current.neighbours.Contains(nodes[i].position))
                {
                    neighbours.Add(nodes[i]);
                }
            }

            return neighbours;
        }

        private List<Node> CalculatePath(Node _end)
        {
            List<Node> path = new List<Node>();
            path.Add(_end);
            Node current = _end;

            while (current.cameFrom != null)
            {
                path.Add(current.cameFrom);
                current = current.cameFrom;
            }

            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(Node _a, Node _b)
        {
            int xDistance = (int)Mathf.Abs(_a.position.x - _b.position.x);
            int yDistance = (int)Mathf.Abs(_a.position.y - _b.position.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private Node GetLowestFCostNode(List<Node> _nodes)
        {
            Node lowestFCost = _nodes[0];
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].f < lowestFCost.f)
                {
                    lowestFCost = _nodes[i];
                }
            }

            return lowestFCost;
        }
    }
}