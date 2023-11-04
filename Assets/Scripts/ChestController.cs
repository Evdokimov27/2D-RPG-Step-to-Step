using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using Mirror;
public class ChestController : NetworkBehaviour
{
    public List<Items> items;
    public int maxSlot;
    public int minSlot;
    public GameObject slotPrefab;
    public Transform itemGrid;
    public GameObject canvasChest;
    public Transform playerInventory;
    public GameObject player;
    public GameObject itemInfoPanel;
    public Text itemNameText;
    public Text itemStatsText;
    public Button confirmButton;
    public ItemUpgrade upgradeItem;
    public GameObject openChest;
    public bool full;

    private bool isOpened = false;
    private Items selectedItem;
    public ItemTypes[] itemType;
    private WeaponTypes[] weaponType = { WeaponTypes.�����������, WeaponTypes.�������, WeaponTypes.��������, WeaponTypes.�������};

    public UnityEvent itemClickedEvent = new UnityEvent();
    public UnityEvent confirmButtonClickedEvent = new UnityEvent();

	public override void OnStartLocalPlayer()
	{
        openChest = GameObject.FindGameObjectWithTag("buttonOpen");
        openChest.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
        full = false;
    }
    private void NewItemChest()
    {
        if (!full)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerInventory = GameObject.FindGameObjectWithTag("playerInventory").GetComponent<Transform>();
            upgradeItem = GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>();

            if (itemGrid != null && items != null)
            {
                for (int index = 0; index < Random.Range(minSlot, maxSlot + 1); index++)
                {
                    Items currentItem = items[index];
                    ItemTypes randomItemType = itemType[Random.Range(0, itemType.Length)];
                    ItemRares randomItemRare = upgradeItem.GetRandomItemRare();
                    WeaponTypes randomWeaponType = weaponType[Random.Range(0, weaponType.Length)];
                    currentItem = upgradeItem.CreateNewItem(currentItem, randomItemType, randomItemRare, randomWeaponType);

                    GameObject newItemObject = Instantiate(currentItem.gameObject, itemGrid);
                    newItemObject.GetComponent<SpriteRenderer>().sprite = currentItem.icon;
                    newItemObject.name = currentItem.itemName;
                    CreateItemButton(newItemObject.GetComponent<Items>());
                }
                full = true;
            }
        }
    }


    private void Update()
    {
        if (player != null)
        {
            if (player.GetComponent<Inventory>().inventoryUI.transform.GetChild(0).gameObject.activeSelf) canvasChest.SetActive(false);
            NewItemChest();
        }
    }

	private void CreateItemButton(Items item)
    {
        GameObject itemUI = Instantiate(slotPrefab, itemGrid);
        itemUI.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;
        itemUI.transform.Find("Icon").GetComponent<Image>().transform.localPosition = new Vector3(50, 50);
        if (item.itemType == ItemTypes.Weapon)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);

        }
        else if (item.itemType == ItemTypes.Armor) itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(2, 2);
        else if (item.itemType == ItemTypes.Helmet)
        {
            itemUI.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.75f, 0.75f);
            itemUI.transform.Find("Icon").GetComponent<Image>().type = Image.Type.Simple;
            itemUI.transform.Find("Icon").GetComponent<Image>().preserveAspect = true;
        }
        itemUI.transform.Find("Icon").GetComponent<Image>().SetNativeSize();
        Button itemButton = itemUI.GetComponent<Button>();
        itemUI.name = item.itemName + "(������)";
        Items itemCopy = item;
        itemButton.onClick.AddListener(() => ItemClicked(itemCopy, itemButton.gameObject));
    }

	public void OnTriggerStay2D(Collider2D collider)
	{
        if (collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;
            openChest.GetComponent<Button>().onClick.RemoveAllListeners();
            openChest.GetComponent<Button>().onClick.AddListener(() => Open());
            openChest.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            openChest.GetComponent<Button>().interactable = true;

        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == player.GetComponent<Collider2D>())
        {
            openChest.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0.2f);
            openChest.GetComponent<Button>().interactable = false;

        }
    }

    private void Open()
    {
        if (!isOpened)
        {
            canvasChest.SetActive(true);
            isOpened = true;
            itemInfoPanel.SetActive(false);
        }
        else
        {
            canvasChest.SetActive(false);
            isOpened = false;
        }
    }

    private void ItemClicked(Items item, GameObject itemButton)
    {
        selectedItem = item;
        itemNameText.text = item.itemName;
        switch (item.itemType)
        {
            case ItemTypes.Weapon:
                itemStatsText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n��� ������: {item.weaponType}\n����: {item.baseStats.damage}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                break;
            case ItemTypes.Helmet:
                itemStatsText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n������: {item.baseStats.armor}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                break;
            case ItemTypes.Armor:
                itemStatsText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n������: {item.baseStats.armor}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                break;
            case ItemTypes.Accessory:
                itemStatsText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n�������������� ���-��:\n����: +{item.statBonuses.strength}\n��������: +{item.statBonuses.agility}\n���������: +{item.statBonuses.intelligence}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                break;
            case ItemTypes.Sharpening:
                itemStatsText.text = $"��������: {item.rare}\n��������� ����� ���������: {item.sharpening.chanceModifier}";
                break;
            case ItemTypes.Money:
                player.GetComponent<Inventory>().money += item.money.count;
                Destroy(itemButton);
                Destroy(item);
                break;
        }
        itemInfoPanel.SetActive(true);
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => ConfirmButtonClicked(selectedItem, itemButton));

        itemClickedEvent.Invoke();
    }

    private void ConfirmButtonClicked(Items item, GameObject button)
    {
        player.GetComponent<Inventory>().AddItem();
        if (player.GetComponent<Inventory>().notFull)
        {
            item.transform.SetParent(playerInventory);
            Destroy(button);
            itemInfoPanel.SetActive(false);
            confirmButtonClickedEvent.Invoke();
        }
    }
}
