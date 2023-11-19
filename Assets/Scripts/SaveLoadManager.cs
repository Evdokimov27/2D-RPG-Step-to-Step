using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YG;

public class SaveLoadManager : MonoBehaviour
{
	public GameObject itemPrefab;
	public Transform inventoryPanel;
	public Inventory inventory;
	public EquipmentManager manager;
	private void OnEnable() => YandexGame.GetDataEvent += LoadItems;
	private void OnDisable() => YandexGame.GetDataEvent -= LoadItems;
	// Ваш метод для загрузки, который будет запускаться в старте
	private void Awake()
	{
		LoadItems();
	}
	public void LoadItems()
	{
		if (YandexGame.SDKEnabled == true)
		{
			inventory.money = YandexGame.savesData.gold;
			inventory.items.Clear();
			manager.items.Clear();
			if (YandexGame.savesData.itemsInvent.Count > 0)
			{
				foreach (SaveItems item in YandexGame.savesData.itemsInvent)
				{

					GameObject itemSpawn = Instantiate(itemPrefab, inventory.transform.position, Quaternion.identity);
					itemSpawn.name = item.itemName;

					itemSpawn.GetComponent<Items>().baseStats = item.baseStats;
					itemSpawn.GetComponent<Items>().durability = item.durability;
					itemSpawn.GetComponent<Items>().icon = item.icon;
					itemSpawn.GetComponent<Items>().itemLevel = item.itemLevel;
					itemSpawn.GetComponent<Items>().itemName = item.itemName;
					itemSpawn.GetComponent<Items>().itemType = item.itemType;
					itemSpawn.GetComponent<Items>().itemValue = item.itemValue;
					itemSpawn.GetComponent<Items>().levelUpgrade = item.levelUpgrade;
					itemSpawn.GetComponent<Items>().rare = item.rare;
					itemSpawn.GetComponent<Items>().sharpening = item.sharpening;
					itemSpawn.GetComponent<Items>().healthStat = item.statHealth;
					itemSpawn.GetComponent<Items>().skillRequirements = item.skillRequirements;
					itemSpawn.GetComponent<Items>().statBonuses = item.statBonuses;
					itemSpawn.GetComponent<Items>().weaponType = item.weaponType;
					itemSpawn.transform.SetParent(inventoryPanel);


				}
			}
			else
			{
				foreach (Items item in inventory.startItems)
				{
					GameObject startItem = Instantiate(item.gameObject, manager.playerInventory);
				}
			}
			if (YandexGame.savesData.itemsEquip.Count > 0)
			{
				foreach (SaveItems item in YandexGame.savesData.itemsEquip)
				{
					GameObject itemSpawn = Instantiate(itemPrefab, inventory.transform.position, Quaternion.identity);
					itemSpawn.name = item.itemName + " 1";

					itemSpawn.GetComponent<Items>().baseStats = item.baseStats;
					itemSpawn.GetComponent<Items>().durability = item.durability;
					itemSpawn.GetComponent<Items>().icon = item.icon;
					itemSpawn.GetComponent<Items>().itemLevel = item.itemLevel;
					itemSpawn.GetComponent<Items>().itemName = item.itemName;
					itemSpawn.GetComponent<Items>().itemType = item.itemType;
					itemSpawn.GetComponent<Items>().itemValue = item.itemValue;
					itemSpawn.GetComponent<Items>().levelUpgrade = item.levelUpgrade;
					itemSpawn.GetComponent<Items>().rare = item.rare;
					itemSpawn.GetComponent<Items>().sharpening = item.sharpening;
					itemSpawn.GetComponent<Items>().healthStat = item.statHealth;
					itemSpawn.GetComponent<Items>().skillRequirements = item.skillRequirements;
					itemSpawn.GetComponent<Items>().statBonuses = item.statBonuses;
					itemSpawn.GetComponent<Items>().weaponType = item.weaponType;
					itemSpawn.transform.SetParent(inventoryPanel);
					StartCoroutine(EquipItemMethod(itemSpawn.GetComponent<Items>()));
				}
			}
			else
			{
				foreach (Items item in manager.startItems)
				{
					GameObject startItem = Instantiate(item.gameObject, manager.playerInventory);
					manager.EquipItem(startItem.GetComponent<Items>());
				}
			}

			StartCoroutine(LoadHub());
		}
	} 
	IEnumerator EquipItemMethod(Items item)
	{
		while (!manager.equipmentSlots.ContainsKey(item.itemType))
		{
			yield return null; // Ожидаем следующий кадр перед проверкой условия снова
		}
		manager.EquipItem(item);
	}
	IEnumerator LoadHub()
	{
		while (!inventory.gameObject.GetComponent<Player>().isHub)
		{
			yield return null; // Ожидаем следующий кадр перед проверкой условия снова
		}
		inventory.inventoryUI.PopulateInventoryUI();
	}
	public void RevertProgress()
	{
		YandexGame.ResetSaveProgress();
		YandexGame.SaveProgress();
		LoadItems();
	}

	// Допустим, это Ваш метод для сохранения
	public void SaveItems()
	{

		List<SaveItems> saveItemsInventory = new List<SaveItems>();
		List<SaveItems> saveItemsEquip = new List<SaveItems>();
		YandexGame.savesData.gold = inventory.money;

		foreach (Items item in inventory.items)
		{
			
			saveItemsInventory.Add(
				new SaveItems()
				{
					baseStats = item.baseStats,
					durability = item.durability,
					icon = item.icon,
					itemLevel = item.itemLevel,
					itemName = item.itemName,
					itemType = item.itemType,
					itemValue = item.itemValue,
					levelUpgrade = item.levelUpgrade,
					rare = item.rare,
					sharpening = item.sharpening,
					statHealth = item.healthStat,
					skillRequirements = item.skillRequirements,
					statBonuses = item.statBonuses,
					weaponType = item.weaponType
				});
		}
		foreach (Items item in manager.items)
		{
			saveItemsEquip.Add(
				new SaveItems()
				{
					baseStats = item.baseStats,
					durability = item.durability,
					icon = item.icon,
					itemLevel = item.itemLevel,
					itemName = item.itemName,
					itemType = item.itemType,
					itemValue = item.itemValue,
					levelUpgrade = item.levelUpgrade,
					rare = item.rare,
					sharpening = item.sharpening,
					statHealth = item.healthStat,
					skillRequirements = item.skillRequirements,
					statBonuses = item.statBonuses,
					weaponType = item.weaponType
				});
		}
		YandexGame.savesData.itemsInvent = saveItemsInventory;
		YandexGame.savesData.itemsEquip = saveItemsEquip;
		
		YandexGame.SaveProgress();
	}
}
