using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TravelersManager : MonoBehaviour, IInitializable
{
	[Header("Settings")]
	[SerializeField] private float followersStartDelay;

	[Header("References")]
	[SerializeField] private List<TravelerController> travelers;

	[Header("Assets")]
	[SerializeField] private AssetReference factorsSettingsAddressable;

	public static TravelersManager Instance;

	private const int SEARCHING_OWN_TARGET_MAX_TRIES = 100;

	private bool initialized;
	private TravelerController selectedTraveler;

	public TravelerController SelectedTraveler { get => selectedTraveler; }

	public bool Initialized => initialized;

	private void Awake()
	{
		Instance = this;
	}

	public void Initialize()
	{
		SetTravelersFactors();
	}

	private async void SetTravelersFactors()
	{
		FactorsSettings factorsSettings;
		AsyncOperationHandle<FactorsSettings> asyncOperationHandle = Addressables.LoadAssetAsync<FactorsSettings>(factorsSettingsAddressable);
		await asyncOperationHandle.Task;
		factorsSettings = asyncOperationHandle.Result;

		for (int i = 0; i < travelers.Count; i++)
		{
			travelers[i].SetFactors(factorsSettings.DrawRandomSpeed(), factorsSettings.DrawRandomTurnSpeed());
		}

		initialized = true;
	}

	public void LoadTravelers(List<List<float>> travelersData)
	{
		for (int i = 0; i < travelersData.Count; i++)
		{
			for (int j = 0; j < travelers.Count; j++)
			{
				if (travelersData[i][0] == travelers[j].TravelerId)
				{
					travelers[j].LoadTraveler(travelersData[i]);

					break;
				}
			}
		}

		initialized = true;
	}

	public List<List<float>> GetTravelersData()
	{
		List<List<float>> travelersData = new List<List<float>>();
		for (int i = 0; i < travelers.Count; i++)
		{
			travelersData.Add(travelers[i].GetTravelerData());
		}

		return travelersData;
	}

	public void SelectTraveler(int travelerId)
	{
		selectedTraveler?.SetTravelerSelected(false);
		for (int i = 0; i < travelers.Count; i++)
		{
			if (travelers[i].TravelerId == travelerId)
			{
				selectedTraveler = travelers[i];

				break;
			}
		}
		selectedTraveler.SetTravelerSelected(true);
	}

	public void MoveToTarget(Vector3 target)
	{
		for (int i = 0; i < travelers.Count; i++)
		{
			travelers[i].StopMoving();
			travelers[i].AdjustSpeed(selectedTraveler.Speed);
		}

		List<TravelerController> followers = new List<TravelerController>();
		followers.AddRange(travelers);
		followers.Remove(selectedTraveler);
		for (int i = 1; i < followers.Count; i++)
		{
			for (int j = 0; j < i; j++)
			{
				if (followers[i].Speed > followers[j].Speed)
				{
					TravelerController temp = followers[i];
					followers.RemoveAt(i);
					followers.Insert(j, temp);
					break;
				}
			}
		}

		List<Vector3> leaderPath = NavigationManager.Instance.FindPath(selectedTraveler.transform.position, target);
		selectedTraveler.MoveToTarget(leaderPath);

		for (int i = 0; i < followers.Count; i++)
		{
			List<Vector3> path = new List<Vector3>();
			for (int j = 0; j < leaderPath.Count; j++)
			{
				if (Vector3.Distance(leaderPath[j], target) < Vector3.Distance(followers[i].transform.position, target))
				{
					path = NavigationManager.Instance.FindPath(followers[i].transform.position, leaderPath[j]);
					if (j + 1 < leaderPath.Count)
					{
						path.AddRange(leaderPath.GetRange(j + 1, leaderPath.Count - j - 1));
					}

					break;
				}
			}
			int tries = 0;
			while (tries < SEARCHING_OWN_TARGET_MAX_TRIES)
			{
				Vector3 direction = Quaternion.AngleAxis(Utilities.RandomAngle(), Vector3.up) * Vector3.forward;
				Vector3 ownTarget = target + direction * 2.0f;
				if (NavigationManager.Instance.CanBeTarget(ownTarget) && MapManager.Instance.IsPointInsideMap(ownTarget))
				{
					Vector3 startToOwnTarget = path.Count > 1 ? path[^2] : followers[i].transform.position;
					List<Vector3> pathToOwnTarget = NavigationManager.Instance.FindPath(startToOwnTarget, ownTarget);
					if (pathToOwnTarget.Count > 3)
					{
						tries++;

						continue;
					}
					path.RemoveAt(path.Count - 1);
					path.AddRange(pathToOwnTarget);

					break;
				}
				else
				{
					tries++;
				}
			}
			followers[i].MoveToTarget(path, (i + 1) * followersStartDelay);
		}
	}
}
