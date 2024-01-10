using UnityEngine;

public class Utilities
{
	#region Angles
	public static float AnglesDifference(float firstAngle, float secondAngle)
	{
		float difference = secondAngle - firstAngle;
		if (difference > 180.0f)
		{
			difference -= 360.0f;
		}
		else if (difference < -180.0f)
		{
			difference += 360.0f;
		}

		return difference;
	}

	public static float AngleToPoint(Transform transform, Vector3 point)
	{
		float angleRight = Vector3.Angle(transform.right, point - transform.position);
		float angleLeft = Vector3.Angle(-transform.right, point - transform.position);
		float direction = angleRight > angleLeft ? -1.0f : 1.0f;
		float angle = Vector3.Angle(transform.forward, point - transform.position) * direction;
		if (angle < 0.0f)
		{
			angle += 360.0f;
		}

		return angle;
	}
	#endregion

	#region Layers
	public static bool CompareLayers(int layer, LayerMask layerMask)
	{
		return layerMask == (layerMask | (1 << layer));
	}
	#endregion

	#region Random
	public static float RandomAngle()
	{
		return Random.Range(0.0f, 360.0f);
	}

	public static Quaternion RandomRotationY()
	{
		return Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
	}

	public static bool RandomState(float positiveChance = 50.0f)
	{
		if (positiveChance == 100.0f)
		{
			return true;
		}
		else
		{
			return Random.Range(0.0f, 100.0f) < positiveChance;
		}
	}

	public static float RandomValue(float min, float max)
	{
		return Random.Range(min, max);
	}
	#endregion
}
