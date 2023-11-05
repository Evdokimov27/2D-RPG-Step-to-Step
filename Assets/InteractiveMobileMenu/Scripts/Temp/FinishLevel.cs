using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{

	public int nextLevelIndex;          //The next scene index;
	private int levelIndex;             //This scene index

	// Use this for initialization
	void Start()
	{
		levelIndex = Application.loadedLevel;   //Getting current level index for saving needs;
	}

	// Examples on how to finish level and save stats;

	public void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			Data.SaveData(levelIndex, true);
			Destroy(GameObject.FindGameObjectWithTag("Player"));
			LoadNextLevel();
		}
	}
	public void OnMouseEnter()
	{
		gameObject.transform.localScale = new Vector3(2.4f,1.8f);
	}
	public void OnMouseExit()
	{
		gameObject.transform.localScale = new Vector3(2f, 1.4f);
	}
	public void OnMouseDown()
	{
		if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3) LoadNextLevel();
	}
	//What should we load depends on the OnFinish enum choice;
	public void LoadNextLevel()
	{
		Application.LoadLevel(nextLevelIndex);
	}
}
