using UnityEngine;
public class ItemUpgrade : MonoBehaviour
{
    public Player player;
    public float baseSuccessChance = 0.95f; // ������� ���� ��������� ���������
    public float levelBonusMultiplier = 0.05f; // ��������� ���������


    private string[] armorPrefixes = { "��������", "��������", "�������", "���������", "����������", "��������", "����������", "���������", "��������", "������", "����������", "�����������", "��������", "�����������", "�����������", "������", "�����������", "����������", "����������", "�����������" };
    private string[] helmetPrefixes = { "��������", "��������", "�������", "���������", "�������", "����������", "���������", "��������", "����������", "����������", "�����", "��������", "�������������", "����������" };
    private string[] ringPrefixes = { "�������", "����������", "������", "�����������", "��������", "����������", "���������", "����������", "���������", "�����������", "����������", "������������", "���������", "��������", "�������", "�������", "����������", "��������", "���������", "��������" };
    private string[] healthPrefixes = { "��������", "����������", "�����������", "������������", "��������������", "��������������", "�����������������", "�����������", "������������", "���������������", "����������������", "����������", "���������"};

    private string[] axeWeaponPrefixes = { "������", "�������", "�������", "������������", "��������", "�������", "��������", "��������", "����������", "��������", "�����������", "��������", "������ ��� ������", "���������", "������", "�������", "������", "������", "������", "����������" };
    private string[] daggerWeaponPrefixes = { "�������", "������������", "�������", "����������", "��������", "��������", "��������", "�������", "��������", "�������������", "�������������", "�������", "������", "����������", "������", "���������", "������", "������", "�������", "������" };
    private string[] hammerWeaponPrefixes = { "�����������", "����", "�����", "��������", "���������", "�������", "��������������", "��������", "������������", "������", "��������", "���������", "������", "�������", "������", "�������", "�������������", "����������", "�����������", "����������" };
    private string[] swordWeaponPrefixes = { "�����������", "��������", "���������", "�������", "������", "�������", "����������", "������", "����������", "�����������", "����������", "���������", "���������", "�������", "������������", "���������", "����������", "�����������", "�������������" };

    private string[] armorSuffixes = { "�����", "�������", "����", "���������", "���", "������", "������", "���������", "������", "�������", "��������", "������", "�����", "����", "������", "�����", "��������", "������", "����", "��������" };
    private string[] helmetSuffixes = { "����", "�����", "�����", "������", "�����", "�����", "������", "��������", "����������", "��������", "�����", "����", "�����", "������", "�������", "�����", "���������� �����", "�����-�������", "����������� �����", "������ �����" };
    private string[] ringSuffixes = { "����", "�����", "�����", "������", "�����", "�����" };
    private string[] healthSuffixes = { "�������", "��������", "��������", "��������", "�������", "�����", "�����"};



    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public Items CreateNewItem(Items item, ItemTypes itemType, ItemRares itemRare, WeaponTypes weaponTypes)
    {
        Items newItem = item.GetComponent<Items>();
        newItem.statBonuses = new Bonuses();
        newItem.baseStats = new BaseStats();
        newItem.itemLevel = player.level + Random.Range(-2, 4);
        if (newItem.itemLevel < 1) newItem.itemLevel = 1;
        newItem.itemType = itemType;
        newItem.weaponType = weaponTypes;
        newItem.healthStat = new HealthStat();
        newItem.rare = itemRare;
        newItem.itemValue = (int)(newItem.itemLevel * 1.5);
        newItem.icon = SpriteRandom(item);
        newItem.itemName = GenerateItemName(newItem);
        switch (itemType)
        {
            case ItemTypes.Weapon:
                {
                    newItem.baseStats.damage = (int)System.Math.Round(CalculateItemStat(item)[0]);
                    break;
                }
            case ItemTypes.Armor:
                {
                    newItem.baseStats.armor = (int)System.Math.Round(CalculateItemStat(item)[0]);
                    newItem.durability = Random.Range(30, 101);
                    break;
                }
            case ItemTypes.Helmet:
                {
                    newItem.baseStats.armor = (int)System.Math.Round(CalculateItemStat(item)[0]);
                    newItem.durability = Random.Range(30, 101);
                    break;
                }
            case ItemTypes.Health:
                {
                    newItem.healthStat.health = (int)System.Math.Round(CalculateItemStat(item)[0]);
                    newItem.durability = Random.Range(1, 5);

                    break;
                }
            case ItemTypes.Accessory:
                {
                    newItem.statBonuses.strength = (int)System.Math.Round(CalculateItemStat(item)[0]);
                    newItem.statBonuses.agility = (int)System.Math.Round(CalculateItemStat(item)[1]);
                    newItem.statBonuses.intelligence = (int)System.Math.Round(CalculateItemStat(item)[2]);
                    break;
                }
            case ItemTypes.Money:
                {
                    newItem.itemName = "������";
                    newItem.money.count = Random.Range(1,player.level*2);
                    break;
                }
            case ItemTypes.Sharpening:
                {
                    newItem.sharpening.chanceModifier = (float)CalculateItemStat(item)[0];
                    break;
                }
                break;
        }

        return newItem;

    }

    string GenerateItemName(Items item)
    {
        string prefix = "";
        string suffix = "";

        switch (item.itemType)
        {
            case ItemTypes.Sharpening:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        prefix = "�������";
                        suffix = "������ �������";
                        break;
                    case ItemRares.���������:
                        prefix = "�������";
                        suffix = "������ �������";
                        break;
                    case ItemRares.������:
                        prefix = "�������";
                        suffix = "������ �������";
                        break;
                    case ItemRares.���������:
                        prefix = "��������";
                        suffix = "������ �������";
                        break;
                    case ItemRares.�����������:
                        prefix = "������������";
                        suffix = "������ �������";
                        break;
                }
                break;
            case ItemTypes.Armor:
                prefix = armorPrefixes[Random.Range(0, armorPrefixes.Length)];
                suffix = armorSuffixes[Random.Range(0, armorSuffixes.Length)];
                break;
            case ItemTypes.Helmet:
                prefix = helmetPrefixes[Random.Range(0, helmetPrefixes.Length)];
                suffix = helmetSuffixes[Random.Range(0, helmetSuffixes.Length)];
                break;
            case ItemTypes.Accessory:
                prefix = ringPrefixes[Random.Range(0, ringPrefixes.Length)];
                suffix = ringSuffixes[Random.Range(0, ringSuffixes.Length)];
                break;
            case ItemTypes.Health:
                prefix = healthPrefixes[Random.Range(0, healthPrefixes.Length)];
                suffix = healthSuffixes[Random.Range(0, healthSuffixes.Length)];
                break;
            case ItemTypes.Weapon:
                switch (item.weaponType)
                {
                    case WeaponTypes.�����������:
                        {
                            prefix = axeWeaponPrefixes[Random.Range(0, axeWeaponPrefixes.Length)];
                            suffix = "�����";
                            break;
                        }
                    case WeaponTypes.�������:
                        {
                            prefix = swordWeaponPrefixes[Random.Range(0, swordWeaponPrefixes.Length)];
                            suffix = "���";
                            break;
                        }
                    case WeaponTypes.�������:
                        {
                            prefix = daggerWeaponPrefixes[Random.Range(0, daggerWeaponPrefixes.Length)];
                            suffix = "������";

                            break;
                        }
                    case WeaponTypes.��������:
                        prefix = hammerWeaponPrefixes[Random.Range(0, hammerWeaponPrefixes.Length)];
                        suffix = "�����";
                        break;
                        // �������� ������ �������� ��������� ��� ������ ����� ������ �� ���� �������������.
                }
                break;
        }

        return prefix + " " + suffix;
    }
    

    public float[] CalculateItemStat(Items item)
    {
       
        float rare = (float)1+GetEnumIndexByValue(item.rare);
        float level = 0;
        float[] finaly = { 0,0,0 };
        ItemTypes type = item.itemType;
        level = item.itemLevel;
        var rareValue = rare * 1.05f - GetEnumIndexByValue(item.rare);
        if (item.rare == ItemRares.�������) rareValue = 1f;
        if (ItemTypes.Armor == type) finaly[0] = rareValue * ((player.agility / 5 * (float)level / 5) +1);
        if (ItemTypes.Helmet == type) finaly[0] = rareValue * ((player.agility / 7 * (float)level / 5) + 1);
        if (ItemTypes.Weapon == type) finaly[0] = rareValue * ((player.strength / 2 * (float)level / 5) + 1);
        if (ItemTypes.Health == type) finaly[0] = rareValue * (((player.strength / 10) * (float)level / 5) + 1);
        if (ItemTypes.Sharpening == type)
        {
            finaly[0] = (float)player.intelligence / 100 * rareValue;
        }
        if (ItemTypes.Accessory == type)
        {
            finaly[0] = Random.Range(0, 1 + rare * 1.1f) * level/10;
            finaly[1] = Random.Range(0, 1 + rare * 1.1f) * level/10;
            finaly[2] = Random.Range(0, 1 + rare * 1.1f) * level/10;
        }

        if (finaly[0] < 1) finaly[0] = 1;
        if (finaly[1] < 1) finaly[1] = 1;
        if (finaly[2] < 1) finaly[2] = 1;
        return finaly;  
    }
    public int GetEnumIndexByValue(ItemRares enumValue)
    {
        ItemRares[] enumValues = (ItemRares[])System.Enum.GetValues(typeof(ItemRares));

        for (int i = 0; i < enumValues.Length; i++)
        {
            if (enumValues[i] == enumValue)
            {
                return i;
            }
        }
        return -1;
    }
    public ItemRares GetRandomItemRare()
    {
        float rarityRoll = Random.Range(1, 117);

        if (rarityRoll <= 80f)
        {
            return ItemRares.�������;
        }
        else if (rarityRoll <= 100f)
        {
            return ItemRares.���������;
        }
        else if (rarityRoll <= 110f)
        {
            return ItemRares.������;
        }
        else if (rarityRoll <= 115f)
        {
            return ItemRares.���������;
        }
        else
        {
            return ItemRares.�����������;
        }
    }




    public Sprite SpriteRandom(Items item)
    {
        Sprite randomSprite = null;
        switch (item.itemType)
        {
            case ItemTypes.Weapon:
                {
                    switch (item.weaponType)
                    {
                        case WeaponTypes.�����������:
                            {
                                switch (item.rare)
                                {
                                    case ItemRares.�������:
                                        randomSprite = item.axe.weaponIconCommon[Random.Range(0, item.axe.weaponIconCommon.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.axe.weaponIconUncommon[Random.Range(0, item.axe.weaponIconUncommon.Length)];
                                        break;
                                    case ItemRares.������:
                                        randomSprite = item.axe.weaponIconRare[Random.Range(0, item.axe.weaponIconRare.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.axe.weaponIconEpic[Random.Range(0, item.axe.weaponIconEpic.Length)];
                                        break;
                                    case ItemRares.�����������:
                                        randomSprite = item.axe.weaponIconLegendary[Random.Range(0, item.axe.weaponIconLegendary.Length)];
                                        break;

                                }
                                break;
                            }
                        case WeaponTypes.�������:
                            {
                                switch (item.rare)
                                {
                                    case ItemRares.�������:
                                        randomSprite = item.dagger.weaponIconCommon[Random.Range(0, item.dagger.weaponIconCommon.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.dagger.weaponIconUncommon[Random.Range(0, item.dagger.weaponIconUncommon.Length)];
                                        break;
                                    case ItemRares.������:
                                        randomSprite = item.dagger.weaponIconRare[Random.Range(0, item.dagger.weaponIconRare.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.dagger.weaponIconEpic[Random.Range(0, item.dagger.weaponIconEpic.Length)];
                                        break;
                                    case ItemRares.�����������:
                                        randomSprite = item.dagger.weaponIconLegendary[Random.Range(0, item.dagger.weaponIconLegendary.Length)];
                                        break;
                                }
                                break;
                            }
                        case WeaponTypes.��������:
                            {
                                switch (item.rare)
                                {
                                    case ItemRares.�������:
                                        randomSprite = item.hammer.weaponIconCommon[Random.Range(0, item.hammer.weaponIconCommon.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.hammer.weaponIconUncommon[Random.Range(0, item.hammer.weaponIconUncommon.Length)];
                                        break;
                                    case ItemRares.������:
                                        randomSprite = item.hammer.weaponIconRare[Random.Range(0, item.hammer.weaponIconRare.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.hammer.weaponIconEpic[Random.Range(0, item.hammer.weaponIconEpic.Length)];
                                        break;
                                    case ItemRares.�����������:
                                        randomSprite = item.hammer.weaponIconLegendary[Random.Range(0, item.hammer.weaponIconLegendary.Length)];
                                        break;
                                }
                                break;
                            }
                        case WeaponTypes.�������:
                            {
                                switch (item.rare)
                                {
                                    case ItemRares.�������:
                                        randomSprite = item.sword.weaponIconCommon[Random.Range(0, item.sword.weaponIconCommon.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.sword.weaponIconUncommon[Random.Range(0, item.sword.weaponIconUncommon.Length)];
                                        break;
                                    case ItemRares.������:
                                        randomSprite = item.sword.weaponIconRare[Random.Range(0, item.sword.weaponIconRare.Length)];
                                        break;
                                    case ItemRares.���������:
                                        randomSprite = item.sword.weaponIconEpic[Random.Range(0, item.sword.weaponIconEpic.Length)];
                                        break;
                                    case ItemRares.�����������:
                                        randomSprite = item.sword.weaponIconLegendary[Random.Range(0, item.sword.weaponIconLegendary.Length)];
                                        break;
                                }
                                break;
                            }
                    }

                    break;
                }
            case ItemTypes.Helmet:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        randomSprite = item.helmet.helmetIconCommon[Random.Range(0, item.helmet.helmetIconCommon.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.helmet.helmetIconUncommon[Random.Range(0, item.helmet.helmetIconUncommon.Length)];
                        break;
                    case ItemRares.������:
                        randomSprite = item.helmet.helmetIconRare[Random.Range(0, item.helmet.helmetIconRare.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.helmet.helmetIconEpic[Random.Range(0, item.helmet.helmetIconEpic.Length)];
                        break;
                    case ItemRares.�����������:
                        randomSprite = item.helmet.helmetIconLegendary[Random.Range(0, item.helmet.helmetIconLegendary.Length)];
                        break;

                }
                break;
            case ItemTypes.Health:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        randomSprite = item.potion.potionIconCommon[Random.Range(0, item.potion.potionIconCommon.Length)];
                        break;                     
                    case ItemRares.���������:      
                        randomSprite = item.potion.potionIconUncommon[Random.Range(0, item.potion.potionIconUncommon.Length)];
                        break;                    
                    case ItemRares.������:        
                        randomSprite = item.potion.potionIconRare[Random.Range(0, item.potion.potionIconRare.Length)];
                        break;                    
                    case ItemRares.���������:     
                        randomSprite = item.potion.potionIconEpic[Random.Range(0, item.potion.potionIconEpic.Length)];
                        break;                    
                    case ItemRares.�����������:   
                        randomSprite = item.potion.potionIconLegendary[Random.Range(0, item.potion.potionIconLegendary.Length)];
                        break;
                }
                break;
            case ItemTypes.Armor:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        randomSprite = item.armor.armorIconCommon[Random.Range(0, item.armor.armorIconCommon.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.armor.armorIconUncommon[Random.Range(0, item.armor.armorIconUncommon.Length)];
                        break;
                    case ItemRares.������:
                        randomSprite = item.armor.armorIconRare[Random.Range(0, item.armor.armorIconRare.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.armor.armorIconEpic[Random.Range(0, item.armor.armorIconEpic.Length)];
                        break;
                    case ItemRares.�����������:
                        randomSprite = item.armor.armorIconLegendary[Random.Range(0, item.armor.armorIconLegendary.Length)];
                        break;

                }
                break;
            case ItemTypes.Accessory:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        randomSprite = item.ring.ringIconCommon[Random.Range(0, item.ring.ringIconCommon.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.ring.ringIconUncommon[Random.Range(0, item.ring.ringIconUncommon.Length)];
                        break;
                    case ItemRares.������:
                        randomSprite = item.ring.ringIconRare[Random.Range(0, item.ring.ringIconRare.Length)];
                        break;
                    case ItemRares.���������:
                        randomSprite = item.ring.ringIconEpic[Random.Range(0, item.ring.ringIconEpic.Length)];
                        break;
                    case ItemRares.�����������:
                        randomSprite = item.ring.ringIconLegendary[Random.Range(0, item.ring.ringIconLegendary.Length)];
                        break;
                }
                break;
            case ItemTypes.Sharpening:
                switch (item.rare)
                {
                    case ItemRares.�������:
                        randomSprite = item.sharpeningIcon.sharpeningIconCommon;

                        break;
                    case ItemRares.���������:
                        randomSprite = item.sharpeningIcon.sharpeningIconRare;

                        break;
                    case ItemRares.������:
                        randomSprite = item.sharpeningIcon.sharpeningIconRare;

                        break;
                    case ItemRares.���������:
                        randomSprite = item.sharpeningIcon.sharpeningIconRare;
                        break;
                    case ItemRares.�����������:
                        randomSprite = item.sharpeningIcon.sharpeningIconRare;

                        break;
                }
                break;
            case ItemTypes.Money:
                {
                    randomSprite = item.money.icon;
                    break;
                }
                break;
        }
        return randomSprite;
    }


    public void UpgradeItem(Items item, Items sharpening)
    {
        // �������� ������� ������ � ��������
        if (player == null || item == null)
        {
            return;
        }

        // �������� ������� ����� �������
        if (sharpening == null || sharpening.itemType != ItemTypes.Sharpening)
        {
            return;
        }
        if (item.levelUpgrade != 8)
        {
            float successChance = baseSuccessChance - (item.levelUpgrade * levelBonusMultiplier);

            // ����� ��������� ����� �� ����� �������
            successChance *= sharpening.sharpening.chanceModifier;
            sharpening.durability -= 1;
            if (sharpening.durability <= 0)
            {
                Destroy(sharpening.gameObject);
            }
            // ��������� ������� ���������
            if (Random.Range(0.000001f, 1f) < successChance)
            {
                ApplyUpgrade(item, item.levelUpgrade);
            }
            else
            {
                DecreaseDurability(item);
            }
        }
        else Debug.Log("����. ��.");
    }


    private void ApplyUpgrade(Items item, int upgradeLevel)
    {
        if (upgradeLevel > 1)
        {
            var name = item.itemName.Substring(0, item.itemName.Length - 3);
            item.itemName = name + " +" + upgradeLevel;
        }
        else item.itemName = item.itemName + " +" + upgradeLevel;
        switch (item.itemType)
        {
            case ItemTypes.Weapon:
                UpgradeWeapon(item, upgradeLevel);
                break;

            case ItemTypes.Armor:
                UpgradeArmor(item, upgradeLevel);
                break;
            case ItemTypes.Helmet:
                UpgradeHelmet(item, upgradeLevel);
                break;
            case ItemTypes.Accessory:
                UpgradeAccessory(item, upgradeLevel);
                break;

            default:
                break;
        }
    }

    private void UpgradeWeapon(Items weapon, int upgradeLevel)
    {
        // ������ ��������� ������
        weapon.baseStats.damage += (int)System.Math.Round((double)Random.Range(1, weapon.itemLevel / 7) + 1 * upgradeLevel);
        weapon.levelUpgrade += 1;
    }

    private void UpgradeArmor(Items armor, int upgradeLevel)
    {
        armor.baseStats.armor += (int)System.Math.Round((double)Random.Range(1, armor.itemLevel / 8) + 1 * upgradeLevel);
        armor.levelUpgrade += 1;

    }
    private void UpgradeHelmet(Items helmet, int upgradeLevel)
    {
        helmet.baseStats.armor += (int)System.Math.Round((double)Random.Range(1, helmet.itemLevel / 10) + 1 * upgradeLevel);
        helmet.levelUpgrade += 1;
    }

    private void UpgradeAccessory(Items accessory, int upgradeLevel)
    {
        // ������ ��������� �����������
        accessory.statBonuses.strength += Random.Range(1, accessory.itemLevel / 5 + 1) * upgradeLevel/10;
        accessory.statBonuses.agility += Random.Range(1, accessory.itemLevel / 5 + 1) * upgradeLevel / 10;
        accessory.statBonuses.intelligence += Random.Range(1, accessory.itemLevel / 5 + 1) * upgradeLevel / 10;
        accessory.levelUpgrade += 1;
    }

    private void Repair(Items item)
    {
       item.durability += Random.Range(25, 50);
    }

    public void DecreaseDurability(Items item)
    {
        // ���������� ��������� ��� ��������� ���������
        item.durability -= Random.Range(5, 11);

        if (item.durability <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<EquipmentManager>().DropItem(item);
            GameObject.FindGameObjectWithTag("inventoryUI").GetComponent<InventoryUI>().PopulateInventoryUI();
        }
    }
    public void UseDurability(Items item, int count)
    {
        // ���������� ��������� ��� ��������� ���������
        item.durability -= count;
        if (item.durability <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<EquipmentManager>().DropItem(item);
            GameObject.FindGameObjectWithTag("inventoryUI").GetComponent<InventoryUI>().PopulateInventoryUI();
        }
    }
}

