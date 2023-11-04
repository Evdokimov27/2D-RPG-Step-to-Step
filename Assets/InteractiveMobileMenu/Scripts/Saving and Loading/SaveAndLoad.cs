using UnityEngine;

public static class Data	
{
	//Static function for saving data
	//levelIndex - index of current scene;
	//isFinished - if true - next level will be unlock;
	//starsCount - how much stars we gained (3 is max);

	public static void SaveData(int levelIndex, bool isFinished)
	{
		PlayerPrefsX.SetBool("isFinished"+levelIndex.ToString(), isFinished);

	}

}