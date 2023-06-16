using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("Boss stats")]
    public int attack;
    public int health;
    public int agility;
    public int defense;
    public int luck;

    [Header("Boss rewards")]
    public int DropCoinAmount;  
    [Range(0, 100)]
    public int ChestDropChance;
    public string[] ChestDropRarity = new[] { "Common", "Uncommon", "Rare", "Epic", "Legendary", "Mythic", "Godly"};

    [Header("Boss details")]
    public Sprite sprite;
    public string floorTextObject;
    public string stageTextObject;

}
