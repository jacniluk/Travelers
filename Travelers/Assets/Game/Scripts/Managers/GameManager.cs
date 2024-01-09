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
	}

	private void Start()
	{
		InitializeGame();
	}

	private void InitializeGame()
	{
		MapManager.Instance.Initialize();
		TravelersManager.Instance.Initialize();

		HudManager.Instance.SetSelectedTraveler(defaultTravelerId);
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
