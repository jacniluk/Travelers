using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TravelersManager : MonoBehaviour, IInitializable
{
	[Header("References")]
	[SerializeField] private List<TravelerController> travelers;

	[Header("Assets")]
	[SerializeField] private AssetReference factorsSettingsAddressable;

	public static TravelersManager Instance;

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
}
