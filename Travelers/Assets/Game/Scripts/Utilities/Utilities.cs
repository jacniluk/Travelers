using UnityEngine;

public class Utilities : MonoBehaviour
{
	#region Random
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
