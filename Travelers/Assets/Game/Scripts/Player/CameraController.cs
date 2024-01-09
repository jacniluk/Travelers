using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector3 positionOffset;

	public static CameraController Instance;

	private void Awake()
	{
		Instance = this;
	}

	private void FixedUpdate()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		transform.position = TravelersManager.Instance.SelectedTravelerTransform.position + positionOffset;
	}
}
