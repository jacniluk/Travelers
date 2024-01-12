using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
	public static NavigationManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public List<Vector3> FindPath(Vector3 start, Vector3 target)
	{
        List<Vector3> path = AStar(start, target);
        if (path == null)
        {
            Debug.LogError("Cannot find path from " + start + " to " + target + ".");

            return new List<Vector3>();
        }

		return path;
	}

	private List<Vector3> AStar(Vector3 start, Vector3 goal)
	{
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>() { new Node(start) };
		while (openSet.Count > 0)
		{
			Node x = FindNodeWithLowestF(openSet);
			if (x.position == goal)
			{
				return ReconstructPath(x);
			}
			openSet.Remove(x);
			closedSet.Add(x);
			for (int i = 0; i < x.neighborNodes.Count; i++)
			{
				Node y = x.neighborNodes[i];
				if (closedSet.Contains(y))
				{
					continue;
				}
				float tentativeGScore = x.gScore + Vector3.Distance(x.position, y.position);
				bool tentativeIsBetter = false;
				if (openSet.Contains(y) == false)
				{
					openSet.Add(y);
					y.hScore = Vector3.Distance(y.position, goal);
					tentativeIsBetter = true;
				}
				else if (tentativeGScore < y.gScore)
				{
					tentativeIsBetter = true;
				}
				if (tentativeIsBetter)
				{
					y.cameFrom = x;
					y.gScore = tentativeGScore;
					y.fScore = y.gScore + y.hScore;
				}
			}
		}

		return null;
	}

	private Node FindNodeWithLowestF(List<Node> openSet)
	{
		Node nodeWithLowestF = openSet[0];
		for (int i = 1; i < openSet.Count; i++)
		{
			if (openSet[i].fScore < nodeWithLowestF.fScore)
			{
				nodeWithLowestF = openSet[i];
			}
		}

		return nodeWithLowestF;
	}

	private List<Vector3> ReconstructPath(Node currentNode)
	{
		if (currentNode.cameFrom != null)
		{
			List<Vector3> p = ReconstructPath(currentNode.cameFrom);
			p.Add(currentNode.position);

			return p;
		}
		else
		{
			return new List<Vector3>();
		}
	}

	public class Node
    {
        public Vector3 position;
        public List<Node> neighborNodes;

        public Node cameFrom;
        public float gScore;
        public float hScore;
		public float fScore;

		public Node(Vector3 _position)
        {
            position = _position;
        }
    }
}
