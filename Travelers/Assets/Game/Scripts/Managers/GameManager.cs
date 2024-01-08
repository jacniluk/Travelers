using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		InitializeGame();
	}

	private void InitializeGame()
	{
		MapManager.Instance.Initialize();
		TravelersManager.Instance.Initialize();
	}
}
