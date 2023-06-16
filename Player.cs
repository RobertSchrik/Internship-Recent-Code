using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Connections")]
    public ItemValueCounter itemValueCounter;
    public PlayerBars playerBars;

    [Header("Visual UI elements - Shown in Dungeon")]
    public TextMeshProUGUI attack_UI;
    public TextMeshProUGUI health_UI;
    public TextMeshProUGUI agility_UI;
    public TextMeshProUGUI defense_UI;
    public TextMeshProUGUI luck_UI;

    [Header("Player Data")]
    public PlayerActiveInformation playerActiveInformation;

    [Header("Stats Post Calculations")]
    public int attack;
    public int health;
    public int agility;
    public int defense;
    public int luck;
    public int coins;

    [Header("Stats Loaded from PlayerBaseStats")]
    public int base_coins;

    private int base_attack;
    private int base_health;
    private int base_agility;
    private int base_defense;
    private int base_luck;

    [Header("Stat Multiplier")]
    public int attackMultiplier;
    public int healthMultiplier;
    public int agilityMultiplier;
    public int defenseMultiplier;
    public int luckMultiplier;

    void Start()
    {
        LoadBaseStats();
        UpdatePlayerStats();
    }

    public void LoadBaseStats()
    {
        //Stats derived from main menu.
        base_attack = playerActiveInformation.base_attack;
        base_health = playerActiveInformation.base_health;
        base_agility = playerActiveInformation.base_agility;
        base_defense = playerActiveInformation.base_defense;
        base_luck = playerActiveInformation.base_luck;
        base_coins = playerActiveInformation.base_coins;
    }

    public void UpdatePlayerStats()
    {
        //Find all items of layer [ItemLayer] and update stats on combatTotal.
        itemValueCounter.UpdateItemTotals();


        //Calculate stats based on base value + total items of type * attack multiplier.
        attack = ((base_attack + itemValueCounter.attackTotal) * attackMultiplier);
        health = ((base_health + itemValueCounter.healthTotal) * healthMultiplier);
        agility = ((base_agility + itemValueCounter.agilityTotal) * agilityMultiplier);
        defense = ((base_defense + itemValueCounter.defenseTotal) * defenseMultiplier);
        luck = ((base_luck + itemValueCounter.luckTotal) * luckMultiplier);

        //Update stats on UI Text Objects.
        UpdateUI();

        //Update PlayerHealthBar;
        playerBars.PlayerUpdateHealth();
    }

    private void UpdateUI()
    {
        //Update UI based on UpdatePlayerStats combat values.
        attack_UI.text = $"{attack}";
        health_UI.text = $"{health}";
        agility_UI.text = $"{agility}";
        defense_UI.text = $"{defense}";
        luck_UI.text = $"{luck}";
    }
}
