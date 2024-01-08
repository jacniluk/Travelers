using UnityEngine;

public class TravelerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int travelerId;

    private float speed;
    private float turnSpeed;

    public void SetFactors(float _speed, float _turnSpeed)
    {
        speed = _speed;
        turnSpeed = _turnSpeed;
	}
}
