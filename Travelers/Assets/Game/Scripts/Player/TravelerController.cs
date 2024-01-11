using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int travelerId;

    [Header("References")]
    [SerializeField] private GameObject highlight;

    private float speed;
    private float turnSpeed;
	private IEnumerator moveToTargetCoroutine;
	private float adjustedSpeed;

	public float TravelerId { get => travelerId; }
	public float Speed { get => speed; }

	public void SetFactors(float _speed, float _turnSpeed)
    {
        speed = _speed;
        turnSpeed = _turnSpeed;
	}

	public void LoadTraveler(List<float> travelerData)
	{
		transform.position = new Vector3(travelerData[1], transform.position.y, travelerData[2]);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, travelerData[3], transform.eulerAngles.z);
		SetFactors(travelerData[4], travelerData[5]);
	}

	public List<float> GetTravelerData()
	{
		return new List<float>()
		{
			travelerId,
			transform.position.x,
			transform.position.z,
			transform.eulerAngles.y,
			speed,
			turnSpeed
		};
	}

	public void SetTravelerSelected(bool selected)
    {
		highlight.SetActive(selected);
	}

	public void AdjustSpeed(float maxSpeed)
	{
		adjustedSpeed = maxSpeed > speed ? speed : maxSpeed;
	}

	public void MoveToTarget(Vector3 target)
	{
		if (moveToTargetCoroutine != null)
		{
			StopCoroutine(moveToTargetCoroutine);
		}
		moveToTargetCoroutine = MoveToTargetCoroutine(target);
		StartCoroutine(moveToTargetCoroutine);
	}

	private IEnumerator MoveToTargetCoroutine(Vector3 target)
	{
		float rotationToTarget = Quaternion.LookRotation((target - transform.position).normalized).eulerAngles.y;
		float offsetLeft = Utilities.AnglesDifference(transform.eulerAngles.y, rotationToTarget);
		float turnDirection = offsetLeft > 0.0f ? 1.0f : -1.0f;
		while (offsetLeft != 0.0f)
		{
			float offset = turnSpeed * Time.deltaTime * turnDirection;
			offsetLeft -= offset;
			if (turnDirection == 1.0f && offsetLeft < 0.0f || turnDirection == -1.0f && offsetLeft > 0.0f)
			{
				offsetLeft = 0.0f;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationToTarget, transform.eulerAngles.z);
			}
			else
			{
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + offset, transform.eulerAngles.z);
			}

			yield return null;
		}

		while (transform.position != target)
		{
			Vector3 distance = target - transform.position;
			Vector3 direction = distance.normalized;
			Vector3 offset = adjustedSpeed * Time.deltaTime * direction;
			if (offset.magnitude > distance.magnitude)
			{
				offset = distance;
			}
			transform.position += offset;

			yield return null;
		}

		moveToTargetCoroutine = null;
	}
}
