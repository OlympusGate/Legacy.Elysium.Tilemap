using UnityEngine;
using System.Collections.Generic;

namespace Elysium.AI.Pathfinding
{
	public class Node
    {
		public Vector3 position;

		public int f;
		public int g;
		public int h;

		public Node cameFrom;
		public List<Vector3> neighbours;

		public Node(Vector3 _position, List<Vector3> _neighbours)
        {
			this.position = _position;
			this.neighbours = _neighbours;
        }

		public void CalculateFCost() => f = g + h;
	}
}