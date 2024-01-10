using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private List<TravelerSelecter> travelerSelecters;
	[SerializeField] private GameObject loadButton;

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

	public void SaveGame()
	{
		SaveManager.Instance.SaveGame();
	}

	public void LoadGame()
	{
		SaveManager.Instance.LoadGame();
	}

	public void RestartGame()
    {
		GameManager.Instance.RestartGame();
	}

    public void ExitGame()
    {
		GameManager.Instance.ExitGame();
	}

	public void ShowLoadButton()
	{
		loadButton.SetActive(true);
	}

	public void HideLoadButton()
	{
		loadButton.SetActive(false);
	}
}
