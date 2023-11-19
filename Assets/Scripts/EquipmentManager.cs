using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class EquipmentManager : MonoBehaviour
{
	public List<Items> startItems = new List<Items>(); // Список предметов в инвентаре

	public Player player; // Слот для шлема
	public Button useButton; // Слот для шлема
	public Button armorButton; // Слот для шлема
	public Button attackButton; // Слот для шлема
	public SaveLoadManager saveLoadManager; // Слот для шлема
	public SpriteRenderer weaponSlot; // Слот для оружия
	public SpriteRenderer armorSlot; // Слот для брони
	public SpriteRenderer helmetSlot; // Слот для шлема

	public Sprite freeSlot; // Слот для брони
	public Sprite defaultArmor; // Слот для брони
	public GameObject slotArmor; // Слот для шлема
	public GameObject slotHelmet; // Слот для шлема
	public GameObject slotWeapon; // Слот для шлема
	public GameObject slotPotion; // Слот для шлема
	public GameObject slotNecklace; // Слот для шлема
	public GameObject slotRing; // Слот для шлема
	public List<Items> items; // Слот для шлема
	public InventoryUI inventoryUI; // Слот для шлема
	public Transform playerInventory;
	public Sprite sprite_null;
	public int currentDamage;
	public int currentArmor;
	public int currentHealth;
	public int currentStrength;
	public int currentAgility;
	public int currentIntelligence;


	public Transform equipmentPanel; // Панель для слотов экипировки
	public Dictionary<ItemTypes, Transform> equipmentSlots = new Dictionary<ItemTypes, Transform>();
	public Dictionary<ItemTypes, Button> equipmentSlotPrefabs = new Dictionary<ItemTypes, Button>();

    private Items selectedItem; // Выбранный предмет

	public void Awake()
	{
		equipmentSlots.Add(ItemTypes.Weapon, equipmentPanel.Find("WeaponSlot"));
		equipmentSlots.Add(ItemTypes.Helmet, equipmentPanel.Find("HelmetSlot"));
		equipmentSlots.Add(ItemTypes.Armor, equipmentPanel.Find("ArmorSlot"));
		equipmentSlots.Add(ItemTypes.Accessory, equipmentPanel.Find("AccessorySlot"));
		equipmentSlots.Add(ItemTypes.Health, equipmentPanel.Find("HealthSlot"));

		equipmentSlotPrefabs.Add(ItemTypes.Weapon, equipmentSlots[ItemTypes.Weapon].GetComponent<Button>());
		equipmentSlotPrefabs.Add(ItemTypes.Helmet, equipmentSlots[ItemTypes.Helmet].GetComponent<Button>());
		equipmentSlotPrefabs.Add(ItemTypes.Armor, equipmentSlots[ItemTypes.Armor].GetComponent<Button>());
		equipmentSlotPrefabs.Add(ItemTypes.Accessory, equipmentSlots[ItemTypes.Accessory].GetComponent<Button>());
		equipmentSlotPrefabs.Add(ItemTypes.Health, equipmentSlots[ItemTypes.Health].GetComponent<Button>());
		
		CountStats();
	}
	// Метод для экипировки предмета
	public void CountStats()
	{
		int playerStrength = player.strength;
		int playerAgility = player.agility;
		int playerIntelligence = player.intelligence;
		int playerArmor = player.armor;
		int playerDamage = player.damage;

		currentDamage = 0;
		currentArmor = 0;
		currentStrength = 0;
		currentAgility = 0;
		currentIntelligence = 0;
		currentHealth = 0;
		Dictionary<ItemTypes, Items> equipmentItems = GetEquipmentItems();

		foreach (var kvp in equipmentItems)
		{
			ItemTypes itemType = kvp.Key;
			Items equippedItem = kvp.Value;

			if (equippedItem != null)
			{
				currentDamage += equippedItem.baseStats.damage;
				currentArmor += equippedItem.baseStats.armor;
				currentStrength += equippedItem.statBonuses.strength;
				currentAgility += equippedItem.statBonuses.agility;
				currentIntelligence += equippedItem.statBonuses.intelligence;
				currentHealth += equippedItem.healthStat.health;
			}
		}

		// Добавляем текущие характеристики игрока
		currentStrength += playerStrength;
		currentAgility += playerAgility;
		currentIntelligence += playerIntelligence;
		currentArmor += playerArmor;
		currentDamage += playerDamage;
	}

	public void SelectItem(Items item)
	{
		
		selectedItem = item;
		inventoryUI.itemInfoPanel.SetActive(true);
		inventoryUI.itemNameText.text = item.itemName;
		switch (item.itemType)
		{
			case ItemTypes.Weapon:
				inventoryUI.itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nТип оружия: {item.weaponType}\nУрон: {item.baseStats.damage}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nПрочность: {item.durability}";
				break;
			case ItemTypes.Helmet:
				inventoryUI.itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nПрочность: {item.durability}";
				break;
			case ItemTypes.Armor:
				inventoryUI.itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nПрочность: {item.durability}";
				break;
			case ItemTypes.Accessory:
				inventoryUI.itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nДополнительные хар-ки:\nСила: +{item.statBonuses.strength}\nЛовкость: +{item.statBonuses.agility}\nПрочность: {item.durability}\nИнтеллект: +{item.statBonuses.intelligence}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nПрочность: {item.durability}";
				break;
			case ItemTypes.Sharpening:
				inventoryUI.itemInfoText.text = $"Редкость: {item.rare}\nОсталось использований: {item.durability}\nМножитель шанса улучшения: {item.sharpening.chanceModifier}";
				break;	
			case ItemTypes.Health:
				inventoryUI.itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЛечение:{item.healthStat.health}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nОсталось использований: {item.durability}";
				break;
		}
		inventoryUI.equipText.text = "Снять";

		if (item.itemType == ItemTypes.Weapon)
		{
			inventoryUI.itemIconImage.GetComponent<Image>().preserveAspect = true;
			inventoryUI.itemIconImage.GetComponent<Image>().type = Image.Type.Simple;
			inventoryUI.itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
		}
		else if (item.itemType == ItemTypes.Armor) inventoryUI.itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(2, 2);
		else if (item.itemType == ItemTypes.Sharpening) inventoryUI.itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(2.5f, 2.5f);
		else if (item.itemType == ItemTypes.Helmet)
		{
			inventoryUI.itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
			inventoryUI.itemIconImage.GetComponent<Image>().preserveAspect = true;
		}
		inventoryUI.itemIconImage.sprite = item.icon;
		inventoryUI.equipButton.onClick.RemoveAllListeners();
		inventoryUI.equipButton.onClick.AddListener(() => UnequipItem(item));
		inventoryUI.dropButton.onClick.RemoveAllListeners();
		inventoryUI.dropButton.onClick.AddListener(() => DropItem(item));

	}

	public void DropItem(Items item)
	{
		UnequipItem(item);
		inventoryUI.inventory.RemoveItem(item);
		inventoryUI.itemInfoPanel.SetActive(false);
		inventoryUI.PopulateInventoryUI(); // Обновляем интерфейс после удаления предмета
	}
	public Dictionary<ItemTypes, Items> GetEquipmentItems()
	{
		Dictionary<ItemTypes, Items> equipmentItems = new Dictionary<ItemTypes, Items>();

		foreach (var kvp in equipmentSlots)
		{
			ItemTypes itemType = kvp.Key;
			Transform slot = kvp.Value;

			if (slot != null && slot.childCount > 0)
			{
				Items equippedItem = slot.GetChild(0).GetComponent<Items>();
				if (equippedItem != null)
				{
					equipmentItems[itemType] = equippedItem;
				}
			}
		}

		return equipmentItems;
	}

	public void GetTotalStats()
	{
		currentDamage = 0;
		currentArmor = 0;
		currentStrength = 0;
		currentAgility = 0;
		currentIntelligence = 0;

		Dictionary<ItemTypes, Items> equipmentItems = GetEquipmentItems();

		foreach (var kvp in equipmentItems)
		{
			ItemTypes itemType = kvp.Key;
			Items equippedItem = kvp.Value;

			if (equippedItem != null)
			{
				currentDamage += equippedItem.baseStats.damage;
				currentArmor += equippedItem.baseStats.armor;
				currentStrength += equippedItem.statBonuses.strength;
				currentAgility += equippedItem.statBonuses.agility;
				currentIntelligence += equippedItem.statBonuses.intelligence;
			}
		}
	}
	public void CheckItem()
	{
		foreach (var slot in equipmentSlots)
		{
			Items[] foundItems = slot.Value.GetComponentsInChildren<Items>();
			foreach (Items item in foundItems)
			{
				if (!items.Contains(item))
				{
					EquipItem(item);
				}
			}
		}
	}
	public void EquipItem(Items item)
	{
		if (item == null)
			return;
		if (equipmentSlots.ContainsKey(item.itemType))
		{
			Transform slot = equipmentSlots[item.itemType];
			Button slotPrefab = equipmentSlotPrefabs[item.itemType];

			if (slot != null && slotPrefab != null)
			{

				// Проверяем, есть ли дочерние предметы
				if (slot.transform.childCount > 0)
				{
					UnequipItem(slot.transform.GetChild(0).GetComponent<Items>()); 
				}

				// Перемещаем предмет из инвентаря в слот экипировки
				item.transform.SetParent(slot);
				item.transform.localPosition = Vector3.zero;
				item.gameObject.SetActive(true);
				slotPrefab.image.sprite = item.icon;
				slotPrefab.onClick.AddListener(() => SelectItem(item));
				MoveItemToSlot(item, slot);
				inventoryUI.RemoveItemFromInventory(item); // Удаляем предмет из инвентаря
				items.Add(item);
				switch (item.itemType)
				{
					case ItemTypes.Weapon:
						slotWeapon.SetActive(false);
						break;
					case ItemTypes.Helmet:
						slotHelmet.SetActive(false);
						break;
					case ItemTypes.Armor:
						slotArmor.SetActive(false);
						break;
					case ItemTypes.Accessory:
						slotRing.SetActive(false);
						break;
					case ItemTypes.Health:
						slotPotion.SetActive(false);
						break;
				}
				slot.GetComponent<Image>().SetNativeSize();
				if (item.itemType == ItemTypes.Helmet) slot.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
				if (item.itemType == ItemTypes.Weapon) slot.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
				if (item.itemType == ItemTypes.Armor) slot.gameObject.transform.localScale = new Vector3(1f, 1f);
				if (item.itemType == ItemTypes.Accessory) slot.gameObject.transform.localScale = new Vector3(0.75f, 0.75f);
				GetTotalStats();
				CountStats();
			}
		}
	}



	// Метод для снятия предмета
	public void UnequipItem(Items item)
	{
		items.Remove(item);
		inventoryUI.itemInfoPanel.SetActive(false);
		if (item != null)
		{
			switch (item.itemType)
			{
				case ItemTypes.Weapon:
					slotWeapon.SetActive(true);
					break;
				case ItemTypes.Helmet:
					slotHelmet.SetActive(true);
					break;
				case ItemTypes.Armor:
					slotArmor.SetActive(true);
					break;
				case ItemTypes.Accessory:
					slotRing.SetActive(true);
					break;
				case ItemTypes.Health:
					slotPotion.SetActive(true);
					break;
			}
			RemoveItemFromSlot(item, equipmentSlots[item.itemType]);
			inventoryUI.PopulateInventoryUI(); // Добавляем предмет обратно в инвентарь
			GetTotalStats();
			CountStats();
		}
	}


	// Проверка, экипирован ли предмет



	// Переместить предмет в слот экипировки
	private void MoveItemToSlot(Items item, Transform slot)
	{
		if (item != null && slot != null)
		{
			item.transform.SetParent(slot);
			item.transform.localPosition = Vector3.zero;
			item.gameObject.SetActive(true);
		}
	}

	// Удалить предмет из слота
	private void RemoveItemFromSlot(Items item, Transform slot)
	{
		if (item != null && slot != null)
		{
			slot.GetComponent<Image>().sprite = sprite_null;
			item.transform.SetParent(playerInventory);
			item.gameObject.SetActive(true);
		}
	}
	private void Update()
	{
		//if (!isLocalPlayer) return;
		{
			useButton.interactable = equipmentSlots[ItemTypes.Health].childCount > 0 && player.actionPoints >= player.useAction && player.currentHealth < player.maxHealth;
			armorButton.interactable = equipmentSlots[ItemTypes.Armor].childCount > 0 && equipmentSlots[ItemTypes.Helmet].childCount > 0 && player.actionPoints >= player.defenceAction;
			attackButton.interactable = player.actionPoints >= player.attackAction;

			CheckItem();
			if (equipmentSlots[ItemTypes.Weapon].GetComponent<Image>().sprite != freeSlot) weaponSlot.sprite = equipmentSlots[ItemTypes.Weapon].GetComponent<Image>().sprite;
			else weaponSlot.sprite = null;
			if (equipmentSlots[ItemTypes.Armor].GetComponent<Image>().sprite != freeSlot) armorSlot.sprite = equipmentSlots[ItemTypes.Armor].GetComponent<Image>().sprite;
			else armorSlot.sprite = defaultArmor;
			if (equipmentSlots[ItemTypes.Helmet].GetComponent<Image>().sprite != freeSlot) helmetSlot.sprite = equipmentSlots[ItemTypes.Helmet].GetComponent<Image>().sprite;
			else helmetSlot.sprite = null;

		}
	}
}
[System.Serializable]
public class SerializableDictionary<TKey, TValue>
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	public Dictionary<TKey, TValue> ToDictionary()
	{
		var dict = new Dictionary<TKey, TValue>();

		for (int i = 0; i < keys.Count; i++)
		{
			if (!dict.ContainsKey(keys[i]))
			{
				dict.Add(keys[i], values[i]);
			}
		}

		return dict;
	}
}
