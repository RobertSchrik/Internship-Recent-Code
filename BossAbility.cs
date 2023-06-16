using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAbility : ScriptableObject
{
    public string abilityName;

    [Range(-100, 100)] public int alteredStatsAttack;
    [Range(-100, 100)] public int alteredStatsHealth;
    [Range(-100, 100)] public int alteredStatsAgility;
    [Range(-100, 100)] public int alteredStatsDefence;
}