using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Upgrade : MonoBehaviour
{
    public Sprite sharpeningSlot;
    public Sprite baseSlot;
    public Image itemSlot;
    public Image sharpeningStoneSlot;
    public Button upgradeButton;
    public GameObject ItemPrefab;
    public GameObject canvasNPC;
    public GameObject player;
    public Transform playerInventory;
    public ItemUpgrade upgradeItem;
    public List<Button> itemButtons = new List<Button>();

    public Items selectedUpgradeItem;
    public Items selectedSharpeningStone;
    private bool isItemSlotFull = false;
    private bool isStoneSlotFull = false;
    private bool isOpened = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        upgradeItem = GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>();
        upgradeButton.onClick.AddListener(UpgradeItem);
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
                GameObject buttonGO = Instantiate(ItemPrefab, playerInventory);
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
        
        if (item.itemType == ItemTypes.Sharpening)
        {
            sharpeningStoneSlot.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            selectedSharpeningStone = item;
            sharpeningStoneSlot.sprite = item.icon;
            isStoneSlotFull = true;
        }
        else if(item.itemType == ItemTypes.Helmet || item.itemType == ItemTypes.Weapon || item.itemType == ItemTypes.Armor|| item.itemType == ItemTypes.Accessory)
		{
            if (item.itemType == ItemTypes.Helmet) itemSlot.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
            if (item.itemType == ItemTypes.Weapon) itemSlot.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);
            if (item.itemType == ItemTypes.Armor) itemSlot.gameObject.transform.localScale = new Vector3(1f, 1f);
            if (item.itemType == ItemTypes.Accessory) itemSlot.gameObject.transform.localScale = new Vector3(0.75f, 0.75f);
            if (item.itemType == ItemTypes.Health) itemSlot.gameObject.transform.localScale = new Vector3(0.4f, 0.4f);
            itemSlot.GetComponent<Image>().color = new Color(1,1,1,1f);
            selectedUpgradeItem = item;
            itemSlot.sprite = item.icon;
            isItemSlotFull= true;
        }
        if (isItemSlotFull && isStoneSlotFull) upgradeButton.interactable = true;
    }
    private void Update()
    {
        itemSlot.SetNativeSize();

        if (GameObject.FindGameObjectWithTag("inventoryUI").transform.GetChild(0).gameObject.activeSelf == true)
        {
            isOpened = true;
            Open();
        }

    }
    public void Open()
    {

        if (!isOpened)
        {
            itemSlot.sprite = baseSlot;
            sharpeningStoneSlot.sprite = sharpeningSlot;
            selectedUpgradeItem = null;
            selectedSharpeningStone = null;
            sharpeningStoneSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            itemSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            canvasNPC.SetActive(true);
            isOpened = true;
        }
        else
        {
            isItemSlotFull = false;
            isStoneSlotFull = false;
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
        isItemSlotFull = false;
        isStoneSlotFull = false;
        canvasNPC.SetActive(false);
        isOpened = false;
    }

    public void UpgradeItem()
    {
        if (isItemSlotFull && isStoneSlotFull)
        {
            itemSlot.sprite = baseSlot;
            sharpeningStoneSlot.sprite = sharpeningSlot;
            
            isItemSlotFull = false;
            isStoneSlotFull = false;
            upgradeItem.UpgradeItem(selectedUpgradeItem, selectedSharpeningStone);
            sharpeningStoneSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            itemSlot.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            upgradeButton.interactable = false;
        }
    }
}
