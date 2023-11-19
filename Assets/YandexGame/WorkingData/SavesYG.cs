
using System.Collections.Generic;
using UnityEngine;

namespace YG
{

    [System.Serializable]
    public class SavesYG
    {
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
		// "Технические сохранения" для работы плагина (Не удалять)
		public int level = 1;
		public int exp = 0;
		public int gold = 0;
		public int freePointStats;
		public int strength = 5;
		public int agility = 5;
		public int intelligence = 5;
		public List<SaveItems> itemsEquip = new List<SaveItems>();
        public List<SaveItems> itemsInvent = new List<SaveItems>();
        public List<SaveLevel> levelList = new List<SaveLevel>();
        public List<SaveLevel> subLevelList = new List<SaveLevel>();
        public Vector3 currentCamPos;
    }
	[System.Serializable]
	public class SaveItems
	{
		public ItemTypes itemType;
		public WeaponTypes weaponType;
		public Sharpening sharpening;
		public Sprite icon; // Уровень предмета
		public ItemRares rare; // Уровень предмета
		public int levelUpgrade; // Уровень апгрейда
		public int itemLevel; // Уровень предмета
		public string itemName; // Название предмета
		public int itemValue; // Цена предмета
		public int durability; // Прочность предмета
		public BaseStats baseStats; // Базовые характеристики предмета
		public Bonuses statBonuses; // Бонусы к характеристикам предмета
		public HealthStat statHealth; // Бонусы к характеристикам предмета
		public SkillRequirements skillRequirements; // Требования к навыкам
	}
	[System.Serializable]
	public class SaveLevel
	{                      
		public string nameLevel; 
		public bool isFinished; 
	}
}
