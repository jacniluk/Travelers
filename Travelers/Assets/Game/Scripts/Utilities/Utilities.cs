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
