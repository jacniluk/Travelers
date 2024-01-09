using UnityEngine;

public class TravelerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int travelerId;

    [Header("References")]
    [SerializeField] private GameObject highlight;

    private float speed;
    private float turnSpeed;

    public float TravelerId { get => travelerId; }

    public void SetFactors(float _speed, float _turnSpeed)
    {
        speed = _speed;
        turnSpeed = _turnSpeed;
	}

    public void SetTravelerSelected(bool selected)
    {
		highlight.SetActive(selected);
	}
}
