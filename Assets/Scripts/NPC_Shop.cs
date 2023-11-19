using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC_Shop : MonoBehaviour
{
    public List<Items> items = new List<Items>();
    public Transform shopPanel;
    public Transform inventoryPanel;
    public GameObject shopItemPrefab;
    public GameObject infoPanel;
    public GameObject canvasNPC;
    public bool isOpened;
    public Image icon;
    public Text price;
    public Text itemNameText;
    public Text itemStatsText;
    public Button buyButton;
    public List<Button> itemButtons = new List<Button>();

    public GameObject player;
    public Transform playerInventory;
    public ItemUpgrade upgradeItem;
    public ItemTypes[] itemType;
    private WeaponTypes[] weaponType = { WeaponTypes.Разрубающий, WeaponTypes.Колющий, WeaponTypes.Дробящий, WeaponTypes.Режущий };
    public UnityEvent itemClickedEvent = new UnityEvent();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = GameObject.FindGameObjectWithTag("playerInventory").GetComponent<Transform>();
        upgradeItem = GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>();
        NewItem();
    }
    private void NewItem()
    {
        if (shopPanel != null && items != null)
        {
            foreach(Items item in items)
            {
                Items currentItem = item;
                ItemTypes randomItemType = itemType[Random.Range(0, itemType.Length)];
                ItemRares randomItemRare = upgradeItem.GetRandomItemRare();
                WeaponTypes randomWeaponType = weaponType[Random.Range(0, weaponType.Length)];
                currentItem = upgradeItem.CreateNewItem(currentItem, randomItemType, randomItemRare, randomWeaponType);

                GameObject newItemObject = Instantiate(currentItem.gameObject, shopPanel);
                newItemObject.GetComponent<SpriteRenderer>().sprite = currentItem.icon;
                newItemObject.name = currentItem.itemName;
                CreateItemButton(newItemObject.GetComponent<Items>());
            }
        }
    }
	private void Update()
	{
        if(GameObject.FindGameObjectWithTag("inventoryUI").transform.GetChild(0).gameObject.activeSelf == true)
		{
            isOpened = true;
            Open();
		}
    }
	public void PopulateInventoryUI()
    {
        //if (!isLocalPlayer) return;
        {
            // Очистить существующие кнопки
            foreach (var button in itemButtons)
            {
                Destroy(button.gameObject);
            }
            itemButtons.Clear();
            // Создать кнопку для каждого предмета
            foreach (var item in player.GetComponent<Inventory>().items)
            {
                GameObject buttonGO = Instantiate(shopItemPrefab, inventoryPanel);
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
    private void CreateItemButton(Items item)
    {
        GameObject itemUI = Instantiate(shopItemPrefab, shopPanel);
        itemUI.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
        if (item.itemType == ItemTypes.Weapon)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);

        }
        else if (item.itemType == ItemTypes.Armor) itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(1.5f, 1.5f);
        else if (item.itemType == ItemTypes.Helmet)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
        }
        else if (item.itemType == ItemTypes.Sharpening)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.5f, 0.5f);
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
        }
        else if (item.itemType == ItemTypes.Health)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.4f, 0.4f);
        }
        itemUI.transform.Find("Icon").GetComponent<Image>().SetNativeSize();
        Button itemButton = itemUI.GetComponent<Button>();
        itemUI.name = item.itemName + "(Кнопка)";
        Items itemCopy = item;
        itemButton.onClick.AddListener(() => SelectItem(itemCopy, itemUI));

    }
    public void SelectItem(Items item, GameObject button = null)
    {
        icon.sprite = item.icon;
        icon.SetNativeSize();
        if (item.itemType == ItemTypes.Helmet) icon.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
        if (item.itemType == ItemTypes.Weapon) icon.gameObject.transform.localScale = new Vector3(0.75f, 0.75f);
        if (item.itemType == ItemTypes.Armor) icon.gameObject.transform.localScale = new Vector3(1.5f, 1.5f);
        if (item.itemType == ItemTypes.Accessory) icon.gameObject.transform.localScale = new Vector3(0.75f, 0.75f);
        if (item.itemType == ItemTypes.Sharpening) icon.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
        if (item.itemType == ItemTypes.Health) icon.gameObject.transform.localScale = new Vector3(0.4f, 0.4f);

        itemNameText.text = item.itemName;
        switch (item.itemType)
        {
            case ItemTypes.Weapon:
                itemStatsText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nТип оружия: {item.weaponType}\nУрон: {item.baseStats.damage}\nТребуемый уровень: {item.skillRequirements.requiredLevel}";
                break;
            case ItemTypes.Helmet:
                itemStatsText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}";
                break;
            case ItemTypes.Armor:
                itemStatsText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЗащита: {item.baseStats.armor}\nТребуемый уровень: {item.skillRequirements.requiredLevel}";
                break;
            case ItemTypes.Accessory:
                itemStatsText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nДополнительные хар-ки:\nСила: +{item.statBonuses.strength}\nЛовкость: +{item.statBonuses.agility}\nИнтеллект: +{item.statBonuses.intelligence}\nТребуемый уровень: {item.skillRequirements.requiredLevel}";
                break;
            case ItemTypes.Sharpening:
                itemStatsText.text = $"Редкость: {item.rare}\nМножитель шанса улучшения: {item.sharpening.chanceModifier}";
                break;
            case ItemTypes.Health:
                itemStatsText.text = $"Уровень предмета: {item.itemLevel}\nРедкость: {item.rare}\nЛечение:{item.healthStat.health}\nТребуемый уровень: {item.skillRequirements.requiredLevel}";
                break;
        }
        if (item.itemType != ItemTypes.Money) infoPanel.SetActive(true);
        else infoPanel.SetActive(false);
        buyButton.onClick.RemoveAllListeners();
        if (item.transform.parent.name == "Shop")
        {
            buyButton.gameObject.transform.GetChild(2).GetComponent<Text>().text = "Купить";
            price.text = "Цена:" + (item.itemValue * 10).ToString();
            buyButton.onClick.AddListener(() => BuyItem(item, item.itemValue*10, button));
        }
        if (item.transform.parent.name == "Inventory")
        {
            buyButton.gameObject.transform.GetChild(2).GetComponent<Text>().text = "Продать";
            price.text = "Цена:" + (item.itemValue).ToString();
            buyButton.onClick.AddListener(() => SellItem(item));
        }
        itemClickedEvent.Invoke();
    }
    public void Open()
    {

        if (!isOpened)
        {
            canvasNPC.SetActive(true);
            player.GetComponent<Player>().canMove = false;
            isOpened = true;
            infoPanel.SetActive(false);
        }
        else
        {
            player.GetComponent<Player>().canMove = true;
            canvasNPC.SetActive(false);
            isOpened = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject button = GameObject.FindGameObjectWithTag("buttonOpen");
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => Open());
        button.GetComponent<Button>().interactable = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject button = GameObject.FindGameObjectWithTag("buttonOpen");
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().interactable = false;
        canvasNPC.SetActive(false);
        isOpened = false;
    }
    public void BuyItem(Items item, int price, GameObject button)
    {
        if (player.GetComponent<Inventory>().money >= price)
        {
            Debug.Log("Куплено " + item.itemName + "за " + price + " монет");
            player.GetComponent<Inventory>().money -= price;
            item.transform.SetParent(playerInventory);
            
            Destroy(button);
        }
        else Debug.Log("Недостаточно средств");
    }
    public void SellItem(Items item)
    {
        Debug.Log("Продано " + item.itemName + "за " + item.itemValue + " монет");
        player.GetComponent<Inventory>().money += item.itemValue;
        player.GetComponent<Inventory>().RemoveItem(item);
        infoPanel.SetActive(false);

    }
}
