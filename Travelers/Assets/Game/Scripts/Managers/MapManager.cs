using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MapManager : MonoBehaviour, IInitializable
{
	[Header("Settings")]
	[SerializeField] private int obstaclesAmount;
	[SerializeField] private float middleFreeSpaceRadius;
	[SerializeField] private float targetMarkerTime;

	[Header("References")]
	[SerializeField] private Transform mapRoot;
	[SerializeField] private Transform ground;
	[SerializeField] private GameObject targetMarker;

	[Header("Assets")]
	[SerializeField] private AssetReferenceGameObject obstacleAddressable;

	public static MapManager Instance;

	private const int PLACING_OBSTACLES_MAX_TRIES = 100;

	private bool initialized;
	private float mapBoundX;
	private float mapBoundZ;
	private List<Collider> obstacles;

	public Transform Ground { get => ground; }

	public bool Initialized => initialized;

	private void Awake()
	{
		Instance = this;

		mapBoundX = ground.localScale.x / 2.0f;
		mapBoundZ = ground.localScale.z / 2.0f;
		obstacles = new List<Collider>();
	}

	public void Initialize()
	{
		GenerateMap();
	}

	private async void GenerateMap()
	{
		GameObject obstaclePrefab = await LoadObstaclePrefab();

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
					if (tries >= PLACING_OBSTACLES_MAX_TRIES)
					{
#if UNITY_EDITOR
						Debug.Log("Cannot create more obstacles. Stuck after " + obstacles.Count + ".");
#endif

						break;
					}	
				}
			}

			if (foundRandomPosition)
			{
				Collider obstacle = Instantiate(obstaclePrefab, randomPosition, Utilities.RandomRotationY(), mapRoot).GetComponent<Collider>();
				obstacles.Add(obstacle);
			}
			else
			{
				break;
			}
		}

		initialized = true;
	}

	public async void LoadMap(List<List<float>> obstaclesData)
	{
		GameObject obstaclePrefab = await LoadObstaclePrefab();

		for (int i = 0; i < obstaclesData.Count; i++)
		{
			Collider obstacle = Instantiate(obstaclePrefab,
				new Vector3(obstaclesData[i][0], obstaclePrefab.transform.position.y, obstaclesData[i][1]),
				Quaternion.Euler(0.0f, obstaclesData[i][2], 0.0f),
				mapRoot).GetComponent<Collider>();
			obstacles.Add(obstacle);
		}

		initialized = true;
	}

	private async Task<GameObject> LoadObstaclePrefab()
	{
		GameObject obstaclePrefab;
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(obstacleAddressable);
		await asyncOperationHandle.Task;
		obstaclePrefab = asyncOperationHandle.Result;

		return obstaclePrefab;
	}

	public List<List<float>> GetObstaclesData()
	{
		List<List<float>> obstaclesData = new List<List<float>>();
		for (int i = 0; i < obstacles.Count; i++)
		{
			obstaclesData.Add(new List<float>
				{
					obstacles[i].transform.position.x,
					obstacles[i].transform.position.z,
					obstacles[i].transform.eulerAngles.y
				}
			);
		}

		return obstaclesData;
	}

	public void ShowTargetMarker(Vector3 target)
	{
		if (targetMarker.activeSelf)
		{
			CancelInvoke(nameof(HideTargetMarkerInvoke));
		}

		targetMarker.transform.position = target;
		targetMarker.SetActive(true);

		Invoke(nameof(HideTargetMarkerInvoke), targetMarkerTime);
	}

	private void HideTargetMarkerInvoke()
	{
		targetMarker.SetActive(false);
	}

	public bool IsPointInsideMap(Vector3 point)
	{
		return point.x > -mapBoundX && point.x < mapBoundX && point.z > -mapBoundZ && point.z < mapBoundZ;
	}
}
