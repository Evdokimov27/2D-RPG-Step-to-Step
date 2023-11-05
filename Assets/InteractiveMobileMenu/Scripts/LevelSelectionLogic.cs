using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class LevelSelectionLogic : MonoBehaviour {

	public Sprite finishImg;
	public Sprite unlockImg;
	public Sprite lockImg;
	public GameObject levelMain;
	public GameObject levelSub;

	public List<LevelList> levelList = new List<LevelList>(); //List of level class;
	public List<LevelList> subLevelList = new List<LevelList>(); //List of level class;
	private Vector2 touchPos;
	private CameraControls cam; 		// Camera controls reference;
	private float touchTime;			// How long touch time;
	private float reactTime = 0.25F;		// Fixed touch time to level loading happens;

	void Awake()
	{
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		cam = Camera.main.GetComponent<CameraControls>();
		if (PlayerPrefs.HasKey("currentCamPos") && cam.cameraPosition == CameraControls.CameraPosition.SaveCurrent)
		{
			cam.defaultPosition = PlayerPrefsX.GetVector3("currentCamPos");
		}
		int lvlIndex = 4;
		int lvlNomber = 0;
		//Check if player prefs have any data with levels indexes, if so - assign;
		foreach (LevelList level in levelList)
		{
			level.LevelObject = levelMain.transform.GetChild(lvlNomber).GetComponent<SpriteRenderer>();
			lvlNomber += 1;
			level.LevelIndex = lvlIndex;
			lvlIndex += 1;

			if (PlayerPrefs.HasKey("isFinished" + level.LevelIndex.ToString()))
			{
				level.isFinished = PlayerPrefsX.GetBool("isFinished" + level.LevelIndex.ToString());
			}
		}
		lvlNomber = 0;
		foreach (LevelList level in subLevelList)
		{
			level.LevelObject = levelSub.transform.GetChild(lvlNomber).GetComponent<SpriteRenderer>();
			lvlNomber += 1;
			lvlIndex += 1;
			level.LevelIndex = lvlIndex;

			if (PlayerPrefs.HasKey("isFinished" + level.LevelIndex.ToString()))
			{
				level.isFinished = PlayerPrefsX.GetBool("isFinished" + level.LevelIndex.ToString());
			}
		}

		for (int i = 0; i < levelList.Count; i++)
		{
			if (levelList[i].requiredUnlockLevel > 0 && levelList[i].requiredUnlockLevel <= levelList.Count)
			{
				levelList[i].unlocked = levelList[levelList[i].requiredUnlockLevel - 1].isFinished;
			}
			if (levelList[i].isFinished)
			{
				
				if(i+1 <= levelList.Count-1)
				{
					levelList[i+1].unlocked = true;
					if(cam.cameraPosition == CameraControls.CameraPosition.WithNextLevel)
						cam.defaultPosition = levelList[i+1].LevelObject.transform.position;
				}
			}

			//draw unlock level sprite if level is unlocked and conversely;

			if (levelList[i].isFinished && levelList[i].unlocked)
			{
				levelList[i].LevelObject.sprite = finishImg;
			}
			if (levelList[i].unlocked && !levelList[i].isFinished)
			{
				levelList[i].LevelObject.sprite = unlockImg;
			}
			if(!levelList[i].isFinished && !levelList[i].unlocked)
			{
				levelList[i].LevelObject.sprite = lockImg;
			}
		}

		for (int i = 0; i < subLevelList.Count; i++)
		{
			if (subLevelList[i].isFinished)
			{
				subLevelList[i + 1].unlocked = true;
				if (cam.cameraPosition == CameraControls.CameraPosition.WithNextLevel)
					cam.defaultPosition = subLevelList[i + 1].LevelObject.transform.position;
			}

			if (subLevelList[i].isFinished && subLevelList[i].unlocked)
			{
				subLevelList[i].LevelObject.sprite = finishImg;
			}
			if (subLevelList[i].unlocked && !subLevelList[i].isFinished)
			{
				subLevelList[i].LevelObject.sprite = unlockImg;
			}
			if (!subLevelList[i].isFinished && !subLevelList[i].unlocked)
			{
				subLevelList[i].LevelObject.sprite = lockImg;
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Detecting if LevelObject sprite contains touch position and if so, load it if unlocked;

		if(Input.GetMouseButton(0))
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchTime += Time.deltaTime;
		}

		if(Input.GetMouseButtonUp(0))
		{
			for (int i = 0; i < levelList.Count; i++)
			{
				if(!levelList[i].isFinished && levelList[i].unlocked && levelList[i].LevelObject.bounds.Contains(touchPos) && touchTime < reactTime)
				{
					Application.LoadLevel(levelList[i].LevelIndex);
					PlayerPrefsX.SetVector3("currentCamPos", cam.transform.position);
				}
			}
			touchTime = 0;
		}
		if (Input.GetMouseButtonUp(0))
		{
			for (int i = 0; i < subLevelList.Count; i++)
			{
				if (!subLevelList[i].isFinished && subLevelList[i].unlocked && subLevelList[i].LevelObject.bounds.Contains(touchPos) && touchTime < reactTime)
				{
					Application.LoadLevel(subLevelList[i].LevelIndex);
					PlayerPrefsX.SetVector3("currentCamPos", cam.transform.position);
				}
			}
			touchTime = 0;
		}
	}
	
}


	//Level class;
	[System.Serializable]
public class LevelList
{
	public SpriteRenderer LevelObject;			//Level object renderer in the scene (assign from hierarchy panel);
	public int LevelIndex;
	public bool needrequiredUnlockLevel;
	public int requiredUnlockLevel;
	public bool unlocked;						//Is level unlocked? you can unlock it as default;
	public bool isFinished;						//Is level finished?
	//some bools for editor script purposes;
	public bool expandLevelList = true;
	public bool expandLevelSettings = true;
}
