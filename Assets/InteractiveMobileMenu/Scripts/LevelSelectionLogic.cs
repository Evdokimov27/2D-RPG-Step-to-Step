using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using YG;

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
	private float reactTime = 0.25F;        // Fixed touch time to level loading happens;
	private bool loadSave = true;
	public void Load() => YandexGame.LoadProgress();

	void Awake()
	{
		if ((YandexGame.savesData.levelList.Count == 0 || YandexGame.savesData.subLevelList.Count == 0))
		{
			for (int i = 0; i < levelList.Count; i++)
			{
				YandexGame.savesData.levelList.Add(new SaveLevel() { nameLevel = "Main " + i, isFinished = levelList[i].isFinished });
			}
			levelList[0].unlocked = true;

			for (int i = 0; i < subLevelList.Count; i++)
			{
				YandexGame.savesData.subLevelList.Add(new SaveLevel() { nameLevel = "Sub " + i, isFinished = subLevelList[i].isFinished });
			}
		}
		Destroy(GameObject.FindGameObjectWithTag("Player"));
		cam = Camera.main.GetComponent<CameraControls>();
		cam.defaultPosition = YandexGame.savesData.currentCamPos;

		

		int lvlIndex = 2;
		for (int i = 0; i < levelList.Count; i++)
		{
			levelList[i].LevelObject = levelMain.transform.GetChild(i).GetComponent<SpriteRenderer>();
			levelList[i].LevelIndex = lvlIndex;
			lvlIndex += 1;
			if (YandexGame.savesData.levelList.Count > 0)
			{
				levelList[i].isFinished = YandexGame.savesData.levelList[i].isFinished;
			}
		}



		for (int i = 0; i < subLevelList.Count; i++)
		{
			subLevelList[i].LevelObject = levelSub.transform.GetChild(i).GetComponent<SpriteRenderer>();
			subLevelList[i].LevelIndex = lvlIndex;
			lvlIndex += 1;
			if (YandexGame.savesData.subLevelList.Count > 0)
			{
				subLevelList[i].isFinished = YandexGame.savesData.subLevelList[i].isFinished;
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

				if (i + 1 <= levelList.Count - 1)
				{
					levelList[i + 1].unlocked = true;
					if (cam.cameraPosition == CameraControls.CameraPosition.WithNextLevel)
						cam.defaultPosition = levelList[i + 1].LevelObject.transform.position;
				}
			}



			if (levelList[i].isFinished && levelList[i].unlocked)
			{
				levelList[i].LevelObject.sprite = finishImg;
			}
			if (levelList[i].unlocked && !levelList[i].isFinished)
			{
				levelList[i].LevelObject.sprite = unlockImg;
			}
			if (!levelList[i].isFinished && !levelList[i].unlocked)
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
		if (Input.GetMouseButton(0))
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchTime += Time.deltaTime;
		}

		if(Input.GetMouseButtonDown(0))
		{
			for (int i = 0; i < levelList.Count; i++)
			{
				if(!levelList[i].isFinished && levelList[i].unlocked && levelList[i].LevelObject.bounds.Contains(touchPos))
				{
					Application.LoadLevel(levelList[i].LevelIndex);
					YandexGame.savesData.currentCamPos = cam.transform.position;
					YandexGame.SaveProgress();
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
					YandexGame.savesData.currentCamPos = cam.transform.position;
					YandexGame.SaveProgress();

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
