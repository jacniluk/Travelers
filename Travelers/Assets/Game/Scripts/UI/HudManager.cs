using UnityEngine;

public class HudManager : MonoBehaviour
{
    public void RestartGame()
    {
		GameManager.Instance.RestartGame();
	}

    public void ExitGame()
    {
		GameManager.Instance.ExitGame();
	}
}
