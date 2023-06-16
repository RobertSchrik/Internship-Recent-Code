using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlasherAbility", menuName = "Boss Abilities/Slasher Ability")]
public class SlasherAbility : BossAbility
{
    [Header("Interchangable values")]
    public float boostInterval; // Interval between temporary boosts
    public float boostDuration; // Duration of the boost
    public float attackSpeedMultiplier; // Attack speed multiplier during the boost

    [Header("Skill Status")]
    public bool isSkillExecuting = true;
}
