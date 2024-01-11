using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
	public List<List<float>> obstaclesData; // positionX, positionZ, rotationY
	public List<List<float>> travelersData; // id, positionX, positionZ, rotationY, speed, turnSpeed

	public SaveData(List<List<float>> _obstaclesData, List<List<float>> _travelersData)
	{
		obstaclesData = _obstaclesData;
		travelersData = _travelersData;
	}
}