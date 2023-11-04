using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using Mirror;
public class InventoryUI : NetworkBehaviour
{
    [Header("Инициализация")]
    public GameObject player; // Ссылка на компонент Inventory
    public Inventory inventory; // Ссылка на компонент Inventory
    public Transform playerInventory;
    public EquipmentManager equipmentManager; // Ссылка на компонент EquipmentManager
    public Transform itemButtonParent; // Родительский объект для кнопок предметов в инвентаре
    public GameObject itemButtonPrefab; // Префаб кнопки предмета
    [Header("Деньги")]
    public Text moneyText; // Список предметов в инвентаре
    [Header("Статы")]
    public Text statsNameText; // Список предметов в инвентаре
    public Text statsValueText; // Список предметов в инвентаре
    [Header("Информационная панель")]
    public GameObject itemInfoPanel; // Панель с информацией о предмете
    public Text itemNameText; // Текст с именем предмета
    public Text itemInfoText; // Текст с именем предмета
    public Image itemIconImage; // Изображение предмета
    public Text equipText; // Изображение предмета
    public Text priceText; // Изображение предмета
    public Button equipButton; // Кнопка "Надеть"
    public Button dropButton; // Кнопка "Выбросить"


    public List<Button> itemButtons = new List<Button>();
    private Items selectedItem; // Выбранный предмет
    private int previousItemCount = 0;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        equipmentManager = player.GetComponent<EquipmentManager>();
        inventory = player.GetComponent<Inventory>();
        PopulateInventoryUI();
    }

    public void DownClick(GameObject obj)
    {
        if (obj.GetComponent<Button>().interactable) obj.transform.localScale = new Vector3(0.4f, 0.4f);
    }
    public void UpClick(GameObject obj)
    {
        if (obj.GetComponent<Button>().interactable) obj.transform.localScale = new Vector3(0.5f, 0.5f);
    }
    // Создание кнопок для каждого предмета в инвентаре


    // Создание кнопок для каждого предмета в инвентаре
    public void PopulateInventoryUI()
    {
        //if (!isLocalPlayer) return;
        {
            inventory.AddItem();
            // Очистить существующие кнопки
            foreach (var button in itemButtons)
            {
                Destroy(button.gameObject);
            }
            itemButtons.Clear();
            // Создать кнопку для каждого предмета
            foreach (var item in inventory.items)
            {
                GameObject buttonGO = Instantiate(itemButtonPrefab, itemButtonParent);
                Button button = buttonGO.GetComponent<Button>();
                buttonGO.name = item.itemName;
                buttonGO.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;

                if (item.itemType == ItemTypes.Weapon)
                {
                    buttonGO.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
                    buttonGO.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
                    buttonGO.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);

                }
                else if (item.itemType == ItemTypes.Armor) buttonGO.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(2, 2);
                else if (item.itemType == ItemTypes.Helmet)
                {
                    buttonGO.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
                    buttonGO.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
                    buttonGO.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
                }

                // Добавьте обработчик нажатия для взаимодействия с предметом
                button.onClick.AddListener(() => SelectItem(item));

                itemButtons.Add(button);
            }
        }
    }
    public void SelectItem(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            selectedItem = item;
            itemInfoPanel.SetActive(true);
            itemNameText.text = item.itemName;
            switch (item.itemType)
            {
                case ItemTypes.Weapon:
                    itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nТип оружия: {item.weaponType}\nУрон: {item.baseStats.damage}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nЦена: {item.itemValue}";
                    break;

                case ItemTypes.Helmet:
                    itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nЦена: {item.itemValue}";
                    break;
                case ItemTypes.Armor:
                    itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nЦена: {item.itemValue}";
                    break;
                case ItemTypes.Accessory:
                    itemInfoText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nДополнительные хар-ки:\nСила: +{item.statBonuses.strength}\nЛовкость: +{item.statBonuses.agility}\nИнтеллект: +{item.statBonuses.intelligence}\nТребуемый уровень: {item.skillRequirements.requiredLevel}\nЦена: {item.itemValue}";
                    break;
                case ItemTypes.Sharpening:
                    itemInfoText.text = $"Редкость: {item.rare}\nМножитель шанса улучшения: {item.sharpening}";
                    break;
            }
            equipText.text = "Надеть";

            if (item.itemType == ItemTypes.Weapon)
            {
                itemIconImage.GetComponent<Image>().preserveAspect = true;
                itemIconImage.GetComponent<Image>().type = Image.Type.Simple;
                itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
            }
            else if (item.itemType == ItemTypes.Armor) itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(2, 2);
            else if (item.itemType == ItemTypes.Helmet)
            {
                itemIconImage.GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
                itemIconImage.GetComponent<Image>().preserveAspect = true;
            }
            itemIconImage.sprite = item.icon;
            priceText.text = item.itemValue.ToString();
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => EquipItem(item));
            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(() => DropItem(item));
        }
    }

    public void EquipItem(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            // Передаем предмет EquipmentManager для экипировки
            equipmentManager.EquipItem(item);
            // Закрываем панель с информацией о предмете
            itemInfoPanel.SetActive(false);
        }

    }
    // Обработчик нажатия кнопки "Выбросить"
    public void DropItem(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            Debug.Log("кик");
            // Удалить предмет из инвентаря и закрыть панель
            inventory.RemoveItem(item);
            itemInfoPanel.SetActive(false);
            PopulateInventoryUI(); // Обновляем интерфейс после удаления предмета
        }
    }
    // Обработчик нажатия кнопки предмета
    public void InteractWithItem(Items item)
    {

    }

    // Удалить предмет из инвентаря
    public void RemoveItemFromInventory(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            PopulateInventoryUI(); // Обновляем интерфейс после удаления предмета
            if (inventory.items.Contains(item))
            {
                inventory.AddItem();
            }
        }
    }
    // Добавить предмет в 

    void Update()
    {
        //if (!isLocalPlayer) return;
        {
            statsNameText.text = "\nСила:\nЛовкость:\nИнтелект:\nБроня:\nУрон:";
            statsValueText.text = $"\n{equipmentManager.currentStrength}\n{equipmentManager.currentAgility}\n{equipmentManager.currentIntelligence}\n{equipmentManager.currentArmor}\n{equipmentManager.currentDamage}";
            moneyText.text = inventory.money.ToString();
            int itemCountInInventory = playerInventory.childCount;
            int itemCountInButtons = itemButtonParent.childCount;
            if (itemCountInInventory != itemCountInButtons)
            {
                PopulateInventoryUI(); // Обновляем интерфейс, если изменилось количество предметов
            }
        }
    }
}