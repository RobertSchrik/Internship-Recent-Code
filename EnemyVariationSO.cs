using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Variation", menuName = "Enemy/Variation")]
public class EnemyVariationSO : ScriptableObject
{
    public string attributeName;

    [Header("AttackValues")]
    [Range (-100, 100)]public int minAttack;
    [Range(-100, 100)] public int maxAttack;
    [Header("HealthValues")]
    [Range(-100, 100)] public int minHealth;
    [Range(-100, 100)] public int maxHealth;
    [Header("AgilityValues")]
    [Range(-100, 100)] public int minAgility;
    [Range(-100, 100)] public int maxAgility;
    [Header("DefenseValues")]
    [Range(-100, 100)] public int minDefense;
    [Range(-100, 100)] public int maxDefense;
    [Header("LuckValues")]
    [Range(-100, 100)] public int minLuck;
    [Range(-100, 100)] public int maxLuck;
}
