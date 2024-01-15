using System.Collections;
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
		StartCoroutine(InitializeGame());
	}

	private IEnumerator InitializeGame()
	{
		HudManager.Instance.SetSelectedTraveler(defaultTravelerId);

		bool gameLoaded = false;
		if (SaveManager.ShouldLoadData)
		{
			gameLoaded = SaveManager.Instance.LoadData();
		}
		if (gameLoaded == false)
		{
			MapManager.Instance.Initialize();
			TravelersManager.Instance.Initialize();
		}

		yield return new WaitUntil(() => MapManager.Instance.Initialized);

		NavigationManager.Instance.BuildNavigationSystem();

		yield return new WaitUntil(() => TravelersManager.Instance.Initialized);

		LoadingScreen.Instance.Hide();
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
