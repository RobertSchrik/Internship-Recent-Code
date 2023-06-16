using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DancerAbility", menuName = "Boss Abilities/Dancer Ability")]
public class DancerAbility : BossAbility
{
    [Header("Interchangable values")]
    public float statIncreaseInterval; // Interval between stat increases
    public int additionalAttackPerInterval; // Additional attack gained per interval
    public int additionalAgilityPerInterval; // Additional agility gained per interval

    [Header("Skill Status")]
    public bool isSkillExecuting = true;
}
