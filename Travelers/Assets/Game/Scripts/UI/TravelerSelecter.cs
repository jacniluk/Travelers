using UnityEngine;
using UnityEngine.UI;

public class TravelerSelecter : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int travelerId;

    [Header("References")]
    [SerializeField] private Button selectButton;

    private static TravelerSelecter CurrentTravelerSelecter;

	public int TravelerId { get => travelerId; }

	public void SelectTraveler()
    {
		if (CurrentTravelerSelecter != null)
		{
			CurrentTravelerSelecter.selectButton.interactable = true;
		}
		CurrentTravelerSelecter = this;
		selectButton.interactable = false;

		TravelersManager.Instance.SelectTraveler(travelerId);
	}
}
