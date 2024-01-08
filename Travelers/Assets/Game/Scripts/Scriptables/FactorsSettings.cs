using UnityEngine;

[CreateAssetMenu(fileName = "FactorsSettings", menuName = "Scriptables/Factors Settings")]
public class FactorsSettings : ScriptableObject
{
    [Header("Factors")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minTurnSpeed;
    [SerializeField] private float maxTurnSpeed;

    public float DrawRandomSpeed()
    {
        return Utilities.RandomValue(minSpeed, maxSpeed);
    }

	public float DrawRandomTurnSpeed()
	{
		return Utilities.RandomValue(minTurnSpeed, maxTurnSpeed);
	}
}
