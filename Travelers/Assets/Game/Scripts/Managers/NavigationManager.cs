using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
	public static NavigationManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public List<Vector3> FindPath(Vector3 start, Vector3 stop)
	{
		List<Vector3> path = new List<Vector3>();
		path.Add(stop);

		return path;
	}
}
