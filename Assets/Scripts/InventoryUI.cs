using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using Mirror;
public class InventoryUI : NetworkBehaviour
{
    [Header("�������������")]
    public GameObject player; // ������ �� ��������� Inventory
    public Inventory inventory; // ������ �� ��������� Inventory
    public Transform playerInventory;
    public EquipmentManager equipmentManager; // ������ �� ��������� EquipmentManager
    public Transform itemButtonParent; // ������������ ������ ��� ������ ��������� � ���������
    public GameObject itemButtonPrefab; // ������ ������ ��������
    [Header("������")]
    public Text moneyText; // ������ ��������� � ���������
    [Header("�����")]
    public Text statsNameText; // ������ ��������� � ���������
    public Text statsValueText; // ������ ��������� � ���������
    [Header("�������������� ������")]
    public GameObject itemInfoPanel; // ������ � ����������� � ��������
    public Text itemNameText; // ����� � ������ ��������
    public Text itemInfoText; // ����� � ������ ��������
    public Image itemIconImage; // ����������� ��������
    public Text equipText; // ����������� ��������
    public Text priceText; // ����������� ��������
    public Button equipButton; // ������ "������"
    public Button dropButton; // ������ "���������"


    public List<Button> itemButtons = new List<Button>();
    private Items selectedItem; // ��������� �������
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
    // �������� ������ ��� ������� �������� � ���������


    // �������� ������ ��� ������� �������� � ���������
    public void PopulateInventoryUI()
    {
        //if (!isLocalPlayer) return;
        {
            inventory.AddItem();
            // �������� ������������ ������
            foreach (var button in itemButtons)
            {
                Destroy(button.gameObject);
            }
            itemButtons.Clear();
            // ������� ������ ��� ������� ��������
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

                // �������� ���������� ������� ��� �������������� � ���������
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
                    itemInfoText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n��� ������: {item.weaponType}\n����: {item.baseStats.damage}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                    break;

                case ItemTypes.Helmet:
                    itemInfoText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n������: {item.baseStats.armor}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                    break;
                case ItemTypes.Armor:
                    itemInfoText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n������: {item.baseStats.armor}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                    break;
                case ItemTypes.Accessory:
                    itemInfoText.text = $"������� ��������: {item.itemLevel}\n��������: {item.rare}\n�������������� ���-��:\n����: +{item.statBonuses.strength}\n��������: +{item.statBonuses.agility}\n���������: +{item.statBonuses.intelligence}\n��������� �������: {item.skillRequirements.requiredLevel}\n����: {item.itemValue}";
                    break;
                case ItemTypes.Sharpening:
                    itemInfoText.text = $"��������: {item.rare}\n��������� ����� ���������: {item.sharpening}";
                    break;
            }
            equipText.text = "������";

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
            // �������� ������� EquipmentManager ��� ����������
            equipmentManager.EquipItem(item);
            // ��������� ������ � ����������� � ��������
            itemInfoPanel.SetActive(false);
        }

    }
    // ���������� ������� ������ "���������"
    public void DropItem(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            Debug.Log("���");
            // ������� ������� �� ��������� � ������� ������
            inventory.RemoveItem(item);
            itemInfoPanel.SetActive(false);
            PopulateInventoryUI(); // ��������� ��������� ����� �������� ��������
        }
    }
    // ���������� ������� ������ ��������
    public void InteractWithItem(Items item)
    {

    }

    // ������� ������� �� ���������
    public void RemoveItemFromInventory(Items item)
    {
        //if (!isLocalPlayer) return;
        {
            PopulateInventoryUI(); // ��������� ��������� ����� �������� ��������
            if (inventory.items.Contains(item))
            {
                inventory.AddItem();
            }
        }
    }
    // �������� ������� � 

    void Update()
    {
        //if (!isLocalPlayer) return;
        {
            statsNameText.text = "\n����:\n��������:\n��������:\n�����:\n����:";
            statsValueText.text = $"\n{equipmentManager.currentStrength}\n{equipmentManager.currentAgility}\n{equipmentManager.currentIntelligence}\n{equipmentManager.currentArmor}\n{equipmentManager.currentDamage}";
            moneyText.text = inventory.money.ToString();
            int itemCountInInventory = playerInventory.childCount;
            int itemCountInButtons = itemButtonParent.childCount;
            if (itemCountInInventory != itemCountInButtons)
            {
                PopulateInventoryUI(); // ��������� ���������, ���� ���������� ���������� ���������
            }
        }
    }
}