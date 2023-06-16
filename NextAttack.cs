using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextAttack : MonoBehaviour
{
    [Header("Connections")]
    public Player player;
    public EnemyManager enemyManager;
    public ManaManager manaManager;
    public PlayerDeathScreen playerDeathScreen;
    public GameEndState gameEndState;

    [Header("Player Attack Slider")]
    public Slider playerNextAttackBar;

    [Header("Attack Logic")]
    public int playerFullNextAttack;
    public int rechargeSpeed = 20; //Primary value used to calculate speed.
    public int rechargeWeight = 1; //Secondary value to devide recharge speed/

    [Header("Attack Progress & Slider")]
    public float playerNextAttackProgress;
    public float playerLevelComplete;
    public float fillSpeed;

    private bool coroutineRunning = false;

    private void Start()
    {
        playerNextAttackBar.maxValue = playerFullNextAttack;
        fillSpeed = rechargeSpeed * player.agility;
    }
    // Update is called once per frame
    void Update()
    {
        //First Check, if player has died or level has ended stop attack.
        if (playerDeathScreen.playerDeathConfirmed || gameEndState.playerLevelComplete)
        {
            StopCoroutine(FillNextAttack());
            playerNextAttackProgress = 0;
            return;
        }
        //Second Check, if player attack is equal to player full attack initiate attack.
        if (playerNextAttackProgress >= playerFullNextAttack)
        {
            PlayerAttacks();
            fillSpeed = rechargeSpeed * player.agility;
        }
        else if (!coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(FillNextAttack());
        }
    }

    IEnumerator FillNextAttack()
    {
        // player fills up charge for next 
        while (playerNextAttackProgress < playerFullNextAttack)
        {
            playerNextAttackBar.value = playerNextAttackProgress / rechargeWeight;
            playerNextAttackProgress += Time.deltaTime * fillSpeed;
            yield return null;
        }
        coroutineRunning = false;
    }

    public void PlayerAttacks()
    {
        //Stop any running counter.
        StopCoroutine(FillNextAttack());
        //Player Does Attack and value resets to 0.
        playerNextAttackProgress = 0;
        //Attack is dealth to the enemy.
        int critRoll = Random.Range(1, 100);

        if (critRoll <= player.luck)
        {
            enemyManager.enemyCurrentHealth -= (player.attack * 225)/100;
        }else
        {
            enemyManager.enemyCurrentHealth -= player.attack;
        }

        enemyManager.enemyHealthBar.value = enemyManager.enemyCurrentHealth;
        //Mana is generated from hit(total amount of damage to mana) atm.
        manaManager.manaCount += player.attack;
        manaManager.manaPool.text = manaManager.manaCount.ToString();
    }
}
