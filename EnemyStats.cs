using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy stats")]
    public int attack;
    public int health;
    public int agility;
    public int defense;
    public int luck;

    [Header("Enemy rewards")]
    public int DropCoinAmount;

    [Header("Enemy details")]
    public Sprite sprite;
    public string floorTextObject;
    public string stageTextObject;
}
