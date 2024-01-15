using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance;
	public static bool ShouldLoadData;

	private const string SAVE_FILENAME = "Save";

	private string path;

	private void Awake()
	{
		Instance = this;

		path = Application.persistentDataPath + "/" + SAVE_FILENAME;
	}

	private void Start()
	{
		PrepareLoadButton();
	}

	private void PrepareLoadButton()
	{
		if (File.Exists(path))
		{
			HudManager.Instance.ShowLoadButton();
		}
	}

	public async void SaveGame()
	{
		List<List<float>> obstaclesData = MapManager.Instance.GetObstaclesData();
		List<List<float>> travelersData = TravelersManager.Instance.GetTravelersData();
		SaveData saveData = new SaveData(obstaclesData, travelersData);

		await Task.Run(() =>
		{
			FileStream file;
			if (File.Exists(path))
			{
				file = File.OpenWrite(path);
			}
			else
			{
				file = File.Create(path);
			}

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(file, saveData);

			file.Close();
		});

		HudManager.Instance.ShowLoadButton();
	}

	public void LoadGame()
	{
		if (File.Exists(path))
		{
			ShouldLoadData = true;
			GameManager.Instance.RestartGame();
		}
		else
		{
			OnSaveFileMissing();
		}
	}

	public bool LoadData()
	{
		ShouldLoadData = false;

		FileStream file;
		if (File.Exists(path) == false)
		{
			OnSaveFileMissing();

			return false;
		}

		file = File.OpenRead(path);

		BinaryFormatter binaryFormatter = new BinaryFormatter();
		SaveData saveData = (SaveData)binaryFormatter.Deserialize(file);

		file.Close();

		MapManager.Instance.LoadMap(saveData.obstaclesData);
		TravelersManager.Instance.LoadTravelers(saveData.travelersData);

		return true;
	}

	private void OnSaveFileMissing()
	{
		HudManager.Instance.HideLoadButton();

		Debug.LogError("Save file is missing.");
	}
}