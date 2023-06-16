using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Stats", menuName = "Enemy/Boss Stats")]
public class BossStatistics : ScriptableObject
{
    [Header("Boss Statistics")]
    public int attack;
    public int health;
    public int agility;
    public int defense;
    public int luck;

    [Header("Boss Skills")]
    public List<ScriptableObject> BossSkillList = new List<ScriptableObject>();

    [Header("Boss Image")]
    public Sprite sprite;

    [Header("Boss Rewards")]
    public int minDropCoinAmount;
    public int maxDropCoinAmount;

    [Header("RNG rewards")]
    public bool canDropChest = false; // can the boss drop a loot chest?
    [Range(0, 100)] public int ringDropChance;// the chance for the boss to drop a ring
    public ChestSO[] chestTypes;

    public int GetRandomDropCoinAmount()
    {
        return Random.Range(minDropCoinAmount, maxDropCoinAmount + 1);
    }

}
