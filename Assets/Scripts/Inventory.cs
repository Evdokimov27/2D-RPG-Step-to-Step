using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int money; // ������ ��������� � ���������
    public List<Items> startItems = new List<Items>(); // ������ ��������� � ���������
    public List<Items> items = new List<Items>(); // ������ ��������� � ���������
    public InventoryUI inventoryUI = new InventoryUI(); // ������ ��������� � ���������
    public int maxItems = 30; // ������������ ���������� ��������� � ���������
    public ItemUpgrade upgradeItem;
    public bool notFull = true;

    private Player player; // ������������ ���������� ��������� � ���������
    private ItemTypes[] itemType = { ItemTypes.Accessory, ItemTypes.Weapon, ItemTypes.Armor }; // ������������ ���������� ��������� � ���������
    public void Awake()
    {
        player = this.GetComponent<Player>();

        if (GameObject.FindGameObjectWithTag("upgradeItem"))
        {
            upgradeItem = GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>();
            AddItem();
        }
    }
    // ���������� �������� � ���������
    public void AddItem()
    {
        items.Clear();
        Items[] foundItems = GameObject.FindGameObjectWithTag("playerInventory").GetComponentsInChildren<Items>();

        foreach (Items item in foundItems)
        {
            if (!items.Contains(item))
            {
                if (items.Count < maxItems)
                {
                    items.Add(item);
                    notFull = true;
                }
                else
                {
                    notFull = false;
                }
            }
        }
    }
    public void RemoveItem(Items itemR)
    {
        Debug.Log("�����");
        if (items.Contains(itemR))
        {

            Destroy(itemR.gameObject);
            AddItem();
        }
    }
    // ����� ������ ��������� � ���������

    // ������ ������������� ���������
    private void Update()
    {
        //if (!isLocalPlayer) return;
        {
            AddItem();

            if (Input.GetKeyDown(KeyCode.I)) // ������: �������� �������� �� ��������� �� ������� ������� R
            {
                inventoryUI.transform.GetChild(0).gameObject.SetActive(!inventoryUI.transform.GetChild(0).gameObject.activeSelf);
            }
        }
    }

    public void OpenInventory()
	{
        inventoryUI.transform.GetChild(0).gameObject.SetActive(!inventoryUI.transform.GetChild(0).gameObject.activeSelf);
        if (inventoryUI.transform.GetChild(0).gameObject.activeSelf) inventoryUI.itemInfoPanel.SetActive(false);
    }
    // ���� ����� ����� �������� �� ������ �������� ��������

}