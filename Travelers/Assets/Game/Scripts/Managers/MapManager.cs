using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapManager : MonoBehaviour, IInitializable
{
	[Header("Settings")]
	[SerializeField] private int obstaclesAmount;
	[SerializeField] private float middleFreeSpaceRadius;

	[Header("References")]
	[SerializeField] private Transform mapRoot;
	[SerializeField] private Transform ground;

	[Header("Prefabs")]
	[SerializeField] private AssetReferenceGameObject obstacleAddressable;

	public static MapManager Instance;

	private const int MAX_TRIES = 100;

	private List<Collider> obstacles;

	private void Awake()
	{
		Instance = this;

		obstacles = new List<Collider>();
	}

	public void Initialize()
	{
		GenerateMap();
	}

	private async void GenerateMap()
	{
		GameObject obstaclePrefab;
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(obstacleAddressable);
		await asyncOperationHandle.Task;
		obstaclePrefab = asyncOperationHandle.Result;

		float obstacleSize = Mathf.Max(obstaclePrefab.transform.localScale.x, obstaclePrefab.transform.localScale.z);
		float maxRangeX = (ground.localScale.x - obstacleSize) / 2.0f;
		float maxRangeZ = (ground.localScale.z - obstacleSize) / 2.0f;

		for (int i = 0; i < obstaclesAmount; i++)
		{
			Vector3 randomPosition = new Vector3();
			bool foundRandomPosition = false;
			int tries = 0;
			while (foundRandomPosition == false)
			{
				randomPosition = new Vector3(
					Utilities.RandomValue(middleFreeSpaceRadius, maxRangeX) * (Utilities.RandomState() ? 1.0f : -1.0f),
					obstaclePrefab.transform.position.y,
					Utilities.RandomValue(middleFreeSpaceRadius, maxRangeZ) * (Utilities.RandomState() ? 1.0f : -1.0f)
				);
				bool validation = true;
				for (int j = 0; j < obstacles.Count; j++)
				{
					if (Vector3.Distance(randomPosition, obstacles[j].transform.position) <= obstacleSize)
					{
						validation = false;

						break;
					}
				}
				if (validation)
				{
					foundRandomPosition = true;
				}
				else
				{
					tries++;
					if (tries >= MAX_TRIES)
					{
#if UNITY_EDITOR
						Debug.Log("Cannot create more obstacles. Stuck after " + obstacles.Count + ".");
#endif

						return;
					}	
				}
			}

			Collider obstacle = Instantiate(obstaclePrefab, randomPosition, Utilities.RandomRotationY(), mapRoot).GetComponent<Collider>();
			obstacles.Add(obstacle);
		}
	}
}
