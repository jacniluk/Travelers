using UnityEngine;
using UnityEngine.UI;

public class ResolutionScaler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasScaler canvasScaler;

	private void Awake()
	{
		SetResolution();
	}

	private void SetResolution()
	{
		float referenceResolutionQuotient = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;
		canvasScaler.matchWidthOrHeight = Screen.width / Screen.height < referenceResolutionQuotient ? 0.0f : 1.0f;
	}
}
