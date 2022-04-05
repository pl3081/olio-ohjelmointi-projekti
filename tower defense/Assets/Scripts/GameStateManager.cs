using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
class SaveData
{
    public int money;
    public List<int> completedLevels;
}
public static class GameStateManager
{
	public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath
					 + "/SaveData.dat");
		SaveData data = new SaveData();

		data.money = Player.Instance.money;
		data.completedLevels = LevelManager.Instance.Completed;
		bf.Serialize(file, data);

		file.Close();

		Debug.Log("Game data saved!");
	}
	public static void Load()
	{
		if (File.Exists(Application.persistentDataPath
					   + "/SaveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =
					   File.Open(Application.persistentDataPath
					   + "/SaveData.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();

			Player.Instance.money = data.money;
			LevelManager.Instance.LoadCompleted(data.completedLevels);

			Debug.Log("Game data loaded!");
		}
		else
			Debug.LogError("There is no save data!");
	}
	public static void Reset()
	{
		if (File.Exists(Application.persistentDataPath
					  + "/SaveData.dat"))
		{
			File.Delete(Application.persistentDataPath
							  + "/SaveData.dat");
			Debug.Log("Data reset complete!");
		}
		else
			Debug.LogError("No save data to delete.");
	}
}
