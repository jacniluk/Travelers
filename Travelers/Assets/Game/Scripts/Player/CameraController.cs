using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Transform defaultTraveler;

	private Transform currentTraveler;
	private Vector3 positionOffset;

	private void Awake()
	{
		currentTraveler = defaultTraveler;
		positionOffset = transform.position - currentTraveler.position;
	}

	private void FixedUpdate()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		transform.position = currentTraveler.position + positionOffset;
	}
}
