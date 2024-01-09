using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private List<TravelerSelecter> travelerSelecters;

	public static HudManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void SetSelectedTraveler(int travelerId)
	{
		for (int i = 0; i < travelerSelecters.Count; i++)
		{
			if (travelerSelecters[i].TravelerId == travelerId)
			{
				travelerSelecters[i].SelectTraveler();

				break;
			}
		}
	}

    public void RestartGame()
    {
		GameManager.Instance.RestartGame();
	}

    public void ExitGame()
    {
		GameManager.Instance.ExitGame();
	}
}
