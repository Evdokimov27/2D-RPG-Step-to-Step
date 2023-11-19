using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using YG;

public class FinishLevel : MonoBehaviour
{

	public int nextLevelIndex;          //The next scene index;
	private int levelIndex;             //This scene index

	// Use this for initialization
	void Start()
	{
		levelIndex = Application.loadedLevel-2;   //Getting current level index for saving needs;
	}

	// Examples on how to finish level and save stats;

	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isHub)
			{
				if (levelIndex < 33) YandexGame.savesData.levelList[levelIndex].isFinished = true;
				if (levelIndex > 32) YandexGame.savesData.subLevelList[levelIndex].isFinished = true;
			}
			collider.gameObject.GetComponent<SaveLoadManager>().SaveItems();
			YandexGame.savesData.level = collider.GetComponent<Player>().level;
			YandexGame.savesData.exp = collider.GetComponent<Player>().exp;
			YandexGame.savesData.freePointStats = collider.GetComponent<Player>().freePointStats;
			YandexGame.savesData.strength = collider.GetComponent<Player>().strength;
			YandexGame.savesData.agility = collider.GetComponent<Player>().agility;
			YandexGame.savesData.intelligence = collider.GetComponent<Player>().intelligence;
			YandexGame.SaveProgress();
			Destroy(GameObject.FindGameObjectWithTag("Player"));
			LoadNextLevel();
		}
	}

	//What should we load depends on the OnFinish enum choice;
	public void LoadNextLevel()
	{
		Application.LoadLevel(nextLevelIndex);
	}
}
