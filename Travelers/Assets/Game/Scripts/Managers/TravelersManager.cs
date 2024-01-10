using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TravelersManager : MonoBehaviour, IInitializable
{
	[Header("Settings")]
	[SerializeField] private float startDelayAfterLeader;
	[SerializeField] private float stopDistanceFromLeader;

	[Header("References")]
	[SerializeField] private List<TravelerController> travelers;

	[Header("Assets")]
	[SerializeField] private AssetReference factorsSettingsAddressable;

	public static TravelersManager Instance;

	private TravelerController selectedTraveler;
	private Vector3 target;
	private IEnumerator moveToTargetCoroutine;

	public Transform SelectedTravelerTransform { get => selectedTraveler.transform; }

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

	public void MoveToTarget(Vector3 _target)
	{
		if (moveToTargetCoroutine != null)
		{
			StopCoroutine(moveToTargetCoroutine);
		}
		target = _target;
		moveToTargetCoroutine = MoveToTargetCoroutine();
		StartCoroutine(moveToTargetCoroutine);
	}

	private IEnumerator MoveToTargetCoroutine()
	{
		for (int i = 0; i < travelers.Count; i++)
		{
			travelers[i].AdjustSpeed(selectedTraveler.Speed);
		}

		selectedTraveler.MoveToTarget(target);

		yield return new WaitUntil(() => IsLeaderClosestToTarget());
		yield return new WaitForSeconds(startDelayAfterLeader);

		for (int i = 0; i < travelers.Count; i++)
		{
			if (travelers[i] == selectedTraveler) continue;

			Vector3 direction = Quaternion.AngleAxis(Utilities.RandomAngle(), Vector3.up) * Vector3.forward;
			Vector3 commonTarget = target + direction * stopDistanceFromLeader;
			travelers[i].MoveToTarget(commonTarget);
		}

		moveToTargetCoroutine = null;
	}

	private bool IsLeaderClosestToTarget()
	{
		float leaderDistance = Vector3.Distance(selectedTraveler.transform.position, target);
		for (int i = 0; i < travelers.Count; i++)
		{
			if (travelers[i] == selectedTraveler) continue;

			if (Vector3.Distance(travelers[i].transform.position, target) <= leaderDistance)
			{
				return false;
			}
		}

		return true;
	}
}
