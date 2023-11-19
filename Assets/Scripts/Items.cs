using System;
using UnityEditor;
using UnityEngine;

// ������������ ��� ����� ���������
[Serializable]
public enum ItemTypes
{
    Weapon,
    Helmet,
    Armor,
    Accessory,
    Sharpening,
    Money,
    Health,
    Item
}
[Serializable]
public enum ItemRares
{
    �������,
    ���������,
    ������,
    ���������,
    �����������,
    Creative
}
[Serializable]
public enum WeaponTypes
{
    �����������,
    �������,
    ��������,
    �������
}

[Serializable]
public class Items : MonoBehaviour
{
    public ItemTypes itemType;
    public WeaponTypes weaponType;

    [Header("Sharpening")]
    public Sharpening sharpening;
    [Header("Money")]
    public Money money;
    [Header("Other")]
    public Sprite icon; // ������� ��������
    public ItemRares rare; // ������� ��������
    public int levelUpgrade = 0; // ������� ��������
    public int itemLevel; // ������� ��������
    public string itemName; // �������� ��������
    public int itemValue; // ���� ��������
    public int durability; // ��������� ��������
    public HealthStat healthStat; // ������� �������������� ��������
    public BaseStats baseStats; // ������� �������������� ��������
    public Bonuses statBonuses; // ������ � ��������������� ��������
    public SkillRequirements skillRequirements; // ���������� � �������
    [Header("Icon")]
    public SharpeningIcon sharpeningIcon;
    public AxeIcon axe;
    public SwordIcon sword;
    public DaggerIcon dagger;
    public HammerIcon hammer;
    public HelmetIcon helmet;
    public RingIcon ring;
    public ArmorIcon armor;
    public PotionIcon potion;
}

// ����� ��� ������� ������������� ��������
[System.Serializable]
public class BaseStats
{
    [Header("StatItem")]
    public int damage; // ���� ��� ������
    public int armor; // ����� ��� �����
}
[System.Serializable]
public class HealthStat
{
    [Header("HealthItem")]
    public int health; // ���� ��� ������
}
[System.Serializable]
public class Money
{
    public Sprite icon; // ���� ��� ������
    public int count; // ���� ��� ������
}
[System.Serializable]

public class Sharpening
{
    public float chanceModifier;
}
[System.Serializable]
public class SharpeningIcon
{
    [Header("Sharpening")]
    public Sprite sharpeningIconCommon;
    public Sprite sharpeningIconUncommon;
    public Sprite sharpeningIconRare;
    public Sprite sharpeningIconEpic;
    public Sprite sharpeningIconLegendary;
}
[System.Serializable]
public class AxeIcon
{
    [Header("Weapon �����������")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class DaggerIcon
{
    [Header("Weapon �������")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class HammerIcon
{
    [Header("Weapon ��������")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class SwordIcon
{
    [Header("Weapon �������")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class HelmetIcon
{
    [Header("Helmet")]
    public Sprite[] helmetIconCommon;
    public Sprite[] helmetIconUncommon;
    public Sprite[] helmetIconRare;
    public Sprite[] helmetIconEpic;
    public Sprite[] helmetIconLegendary;
}
[System.Serializable]
public class ArmorIcon
{
    [Header("Armor")]
    public Sprite[] armorIconCommon;
    public Sprite[] armorIconUncommon;
    public Sprite[] armorIconRare;
    public Sprite[] armorIconEpic;
    public Sprite[] armorIconLegendary;
}
[System.Serializable]
public class RingIcon
{
    [Header("Ring")]
    public Sprite[] ringIconCommon;
    public Sprite[] ringIconUncommon;
    public Sprite[] ringIconRare;
    public Sprite[] ringIconEpic;
    public Sprite[] ringIconLegendary;
}
[System.Serializable]
public class PotionIcon
{
    [Header("Ring")]
    public Sprite[] potionIconCommon;
    public Sprite[] potionIconUncommon;
    public Sprite[] potionIconRare;
    public Sprite[] potionIconEpic;
    public Sprite[] potionIconLegendary;
}
// ����� ��� ������� � ��������������� ��������
[System.Serializable]
public class Bonuses
{
    [Header("BonusesStat")]
    public int strength; // ����� � ����
    public int agility; // ����� � ��������
    public int intelligence; // ����� � ���������� � �.�.
}

// ����� ��� ���������� � �������
[System.Serializable]
public class SkillRequirements
{
    public int requiredLevel; // ������� ������, ����������� ��� �������������
}




