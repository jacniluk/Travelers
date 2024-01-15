using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private LayerMask obstacleLayerMask;

	public static NavigationManager Instance;

	private List<Node> nodes;
	private int mapLength;
	private float startX;
	private float startZ;

	private void Awake()
	{
		Instance = this;
	}

	public void BuildNavigationSystem()
	{
		int mapWidth = (int)MapManager.Instance.Ground.transform.localScale.x;
		mapLength = (int)MapManager.Instance.Ground.transform.localScale.z;
		startX = -(mapWidth / 2.0f) + 0.5f;
		startZ = -(mapLength / 2.0f) + 0.5f;

		nodes = new List<Node>();
		for (int i = 0; i < mapWidth; i++)
		{
			for (int j = 0; j < mapLength; j++)
			{
				Vector3 position = new Vector3(startX + i, 0.0f, startZ + j);
				bool hasCollision = Physics.OverlapBox(
					position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, obstacleLayerMask).Length > 0;
				Node node = new Node(position, hasCollision);
				nodes.Add(node);
			}
		}

		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].neighborNodes = new List<Node>();
			SetNeighbor(nodes[i], -1.0f, -1.0f);
			SetNeighbor(nodes[i], 0.0f, -1.0f);
			SetNeighbor(nodes[i], 1.0f, -1.0f);
			SetNeighbor(nodes[i], -1.0f, 0.0f);
			SetNeighbor(nodes[i], 1.0f, 0.0f);
			SetNeighbor(nodes[i], -1.0f, 1.0f);
			SetNeighbor(nodes[i], 0.0f, 1.0f);
			SetNeighbor(nodes[i], 1.0f, 1.0f);
		}
	}

	private void SetNeighbor(Node node, float offsetX, float offsetZ)
	{
		Node neighborNode = FindNodeWithPoint(node.position + new Vector3(offsetX, 0.0f, offsetZ));
		if (neighborNode == null)
		{
			return;
		}

		if (neighborNode.hasCollision == false)
		{
			if (offsetX == 0.0f || offsetZ == 0.0f)
			{
				node.neighborNodes.Add(neighborNode);

				return;
			}
			else
			{
				if (FindNodeWithPoint(node.position + new Vector3(offsetX, 0.0f, 0.0f)).hasCollision == false &&
					FindNodeWithPoint(node.position + new Vector3(0.0f, 0.0f, offsetZ)).hasCollision == false)
				{
					node.neighborNodes.Add(neighborNode);

					return;
				}
			}
		}
	}

	public List<Vector3> FindPath(Vector3 start, Vector3 target)
	{
        List<Vector3> path = AStar(start, FindNodeWithPoint(target).position);

		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].cameFrom = null;
		}

        if (path == null)
        {
            Debug.LogError("Cannot find path from " + start + " to " + target + ".");

            return new List<Vector3>();
        }

		if (path.Count > 0 && path[^1] != target)
		{
			path.RemoveAt(path.Count - 1);
			path.Add(target);
		}

		return path;
	}

	private List<Vector3> AStar(Vector3 start, Vector3 goal)
	{
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>() { FindNodeWithPoint(start) };
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

	private Node FindNodeWithPoint(Vector3 point)
	{
		float x = point.x + 0.5f;
		float z = point.z + 0.5f;
		x = Mathf.Round(x) - 0.5f;
		z = Mathf.Round(z) - 0.5f;
		if (x < startX)
		{
			x += 1.0f;
		}
		else if (x > -startX)
		{
			x -= 1.0f;
		}
		if (z < startZ)
		{
			z += 1.0f;
		}
		else if (z > -startZ)
		{
			z -= 1.0f;
		}

		int widthFactor = (int)(x - startX);
		widthFactor *= mapLength;
		int lengthFactor = (int)(z - startZ);
		int index = widthFactor + lengthFactor;

		return nodes[index];
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

	public bool CanBeTarget(Vector3 point)
	{
		return Physics.OverlapBox(point + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.75f, 0.75f, 0.75f), Quaternion.identity, obstacleLayerMask).Length == 0;
	}

	public class Node
    {
        public Vector3 position;
        public List<Node> neighborNodes;
		public bool hasCollision;

        public Node cameFrom;
        public float gScore;
        public float hScore;
		public float fScore;

		public Node(Vector3 _position, bool _hasCollision)
        {
            position = _position;
			hasCollision = _hasCollision;
        }
    }
}
