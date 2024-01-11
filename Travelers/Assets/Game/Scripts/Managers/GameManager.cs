using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private int defaultTravelerId;

	public static GameManager Instance;

	private void Awake()
	{
		Instance = this;

		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		InitializeGame();
	}

	private async void InitializeGame()
	{
		HudManager.Instance.SetSelectedTraveler(defaultTravelerId);

		bool gameLoaded = false;
		if (SaveManager.ShouldLoadData)
		{
			gameLoaded = await SaveManager.Instance.LoadData();
		}
		if (gameLoaded == false)
		{
			MapManager.Instance.Initialize();
			TravelersManager.Instance.Initialize();
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
	}
}
