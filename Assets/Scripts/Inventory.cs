using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Inventory : MonoBehaviour
{
    public int money; // Список предметов в инвентаре
    public List<Items> startItems = new List<Items>(); // Список предметов в инвентаре
    public List<Items> items = new List<Items>(); // Список предметов в инвентаре
    public InventoryUI inventoryUI = new InventoryUI(); // Список предметов в инвентаре
    public int maxItems = 30; // Максимальное количество предметов в инвентаре
    public ItemUpgrade upgradeItem;
    public bool notFull = true;

    private Player player; // Максимальное количество предметов в инвентаре
    private ItemTypes[] itemType = { ItemTypes.Accessory, ItemTypes.Weapon, ItemTypes.Armor }; // Максимальное количество предметов в инвентаре
    public void Awake()
    {
        player = this.GetComponent<Player>();

        if (GameObject.FindGameObjectWithTag("upgradeItem"))
        {
            upgradeItem = GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>();
            AddItem();
        }
    }
    // Добавление предмета в инвентарь
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
        Debug.Log("удане");
        if (items.Contains(itemR))
        {

            Destroy(itemR.gameObject);
            AddItem();
        }
    }
    // Вывод списка предметов в инвентаре

    // Пример использования инвентаря
    private void Update()
    {
        //if (!isLocalPlayer) return;
        {
            AddItem();

            if (Input.GetKeyDown(KeyCode.I)) // Пример: удаление предмета из инвентаря по нажатию клавиши R
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
    // Этот метод нужно заменить на логику создания предмета

}