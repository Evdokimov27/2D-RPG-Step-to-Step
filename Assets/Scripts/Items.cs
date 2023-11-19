using System;
using UnityEditor;
using UnityEngine;

// Перечисление для типов предметов
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
    Обычная,
    Необычная,
    Редкая,
    Эпическая,
    Легендарная,
    Creative
}
[Serializable]
public enum WeaponTypes
{
    Разрубающий,
    Колющий,
    Дробящий,
    Режущий
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
    public Sprite icon; // Уровень предмета
    public ItemRares rare; // Уровень предмета
    public int levelUpgrade = 0; // Уровень апгрейда
    public int itemLevel; // Уровень предмета
    public string itemName; // Название предмета
    public int itemValue; // Цена предмета
    public int durability; // Прочность предмета
    public HealthStat healthStat; // Базовые характеристики предмета
    public BaseStats baseStats; // Базовые характеристики предмета
    public Bonuses statBonuses; // Бонусы к характеристикам предмета
    public SkillRequirements skillRequirements; // Требования к навыкам
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

// Класс для базовых характеристик предмета
[System.Serializable]
public class BaseStats
{
    [Header("StatItem")]
    public int damage; // Урон для оружия
    public int armor; // Броня для брони
}
[System.Serializable]
public class HealthStat
{
    [Header("HealthItem")]
    public int health; // Урон для оружия
}
[System.Serializable]
public class Money
{
    public Sprite icon; // Урон для оружия
    public int count; // Урон для оружия
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
    [Header("Weapon Разрубающий")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class DaggerIcon
{
    [Header("Weapon Колющий")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class HammerIcon
{
    [Header("Weapon Дробящий")]
    public Sprite[] weaponIconCommon;
    public Sprite[] weaponIconUncommon;
    public Sprite[] weaponIconRare;
    public Sprite[] weaponIconEpic;
    public Sprite[] weaponIconLegendary;
}
[System.Serializable]
public class SwordIcon
{
    [Header("Weapon Режущий")]
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
// Класс для бонусов к характеристикам предмета
[System.Serializable]
public class Bonuses
{
    [Header("BonusesStat")]
    public int strength; // Бонус к силе
    public int agility; // Бонус к ловкости
    public int intelligence; // Бонус к интеллекту и т.д.
}

// Класс для требований к навыкам
[System.Serializable]
public class SkillRequirements
{
    public int requiredLevel; // Уровень навыка, необходимый для использования
}




