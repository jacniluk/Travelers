using System.IO;
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

	public void SaveGame()
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

		file.Close();

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
		if (File.Exists(path))
		{
			file = File.OpenRead(path);
		}
		else
		{
			OnSaveFileMissing();

			return false;
		}

		file.Close();

		return true;
	}

	private void OnSaveFileMissing()
	{
		HudManager.Instance.HideLoadButton();

		Debug.LogError("Save file is missing.");
	}
}