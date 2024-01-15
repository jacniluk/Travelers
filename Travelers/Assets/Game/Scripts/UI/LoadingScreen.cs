using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;

    public static LoadingScreen Instance;

	private void Awake()
    {
        Instance = this;
    }

    public void Hide()
    {
        panel.SetActive(false);
	}
}
