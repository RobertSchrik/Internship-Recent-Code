using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNextAttack : MonoBehaviour
{
    [Header("Connections")]
    public EnemyManager enemyManager;
    public PlayerBars playerBars;
    public PlayerDeathScreen playerDeathScreen;
    public GameEndState gameEndState;

    [Header("Enemy attack logic")]
    public int enemyFullNextAttack;
    public int rechargeSpeed;
    public int enemyrechargeWeight = 1;

    [Header("Enemy Attack Progress & Slider")]
    public float enemyNextAttackProgress;
    public float fillSpeed;
    public Slider enemyNextAttackBar;

    private bool coroutineRunning = false;

    private void Start()
    {
        enemyNextAttackBar.maxValue = enemyFullNextAttack;
    }
    // Update is called once per frame
    public void Update()
    {
        //First check, has level concluded.
        if (gameEndState.playerLevelComplete)
        {
            StopCoroutine(FillNextAttack());
            enemyNextAttackProgress = 0;
            return;
        }

        //Second check, has player died.
        if (playerDeathScreen.playerDeathConfirmed)
        {
            StopCoroutine(FillNextAttack());
            gameEndState.FloorIsFailed();
            return;
        }

        //Final check, has enemy attack bar filled.
        if (enemyNextAttackProgress >= enemyFullNextAttack)
        {
            fillSpeed = rechargeSpeed * enemyManager.agility;
            EnemyAttacks();
        }
        else if (!coroutineRunning && enemyManager.enemyStatsLoaded)
        {
            coroutineRunning = true;
            StartCoroutine(FillNextAttack());
        }
    }

    public void ResetCoroutine()
    {
        //Stop any running counter.
        StopCoroutine(FillNextAttack());
        //Enemy Does Attack and value resets to 0.
        enemyNextAttackProgress = 0;
    }

    IEnumerator FillNextAttack()
    {
        // player fills up charge for next attack
        while (enemyNextAttackProgress < enemyFullNextAttack)
        {
            fillSpeed = rechargeSpeed * enemyManager.agility;
            enemyNextAttackBar.value = enemyNextAttackProgress / enemyrechargeWeight;
            enemyNextAttackProgress += Time.deltaTime * fillSpeed;
            yield return null;
        }
        coroutineRunning = false;
    }

    public void EnemyAttacks()
    {
        ResetCoroutine();
        ActiveSkillActivation();
        //Enemy attacks.
        playerBars.playerCurrentHealth -= enemyManager.attack;
        playerBars.playerHealthBar.value = playerBars.playerCurrentHealth;
        if (playerBars.playerCurrentHealth <= 0)
        {
            gameEndState.FloorIsFailed();
        }
    }

    public void ActiveSkillActivation()
    {
        if (!enemyManager.bossIsHere)
        {
            return;
        }else if(enemyManager.bossIsHere)
        {
            int randomIndex = Random.Range(0, enemyManager.activeBossSkillList.Count);
            ScriptableObject randomSelectedSkill = enemyManager.activeBossSkillList[randomIndex];

            foreach (ScriptableObject scriptableObject in enemyManager.activeBossSkillList)
            {
                if (randomSelectedSkill is DancerAbility dancerAbility)
                {
                    Debug.Log("Ability: Dancer");
                    if (!dancerAbility.isSkillExecuting)
                    {
                        dancerAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteDancerAbility(enemyManager, dancerAbility));
                    }
                }
                if (randomSelectedSkill is SlasherAbility slasherAbility)
                {
                    if (!slasherAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Slasher");
                        slasherAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteSlasherAbility(enemyManager, slasherAbility));
                    }
                }
                if (randomSelectedSkill is BunkerAbility bunkerAbility)
                {
                    if (!bunkerAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Bunker");
                        bunkerAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteBunkerAbility(enemyManager, bunkerAbility));
                    }
                }
                if (randomSelectedSkill is ManaMageAbility manaMageAbility)
                {
                    if (!manaMageAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Mana Mage");
                        manaMageAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteManaMageAbility(enemyManager, manaMageAbility));
                    }
                }
                if (randomSelectedSkill is NocturnalAbility nocturnalAbility)
                {
                    if (!nocturnalAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Nocturnal");
                        nocturnalAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteNocturnalAbility(enemyManager, nocturnalAbility));
                    }
                }
                if (randomSelectedSkill is SlimerAbility slimerAbility)
                {
                    if (!slimerAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Slimer");
                        slimerAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteSlimerAbility(enemyManager, slimerAbility));
                    }
                }
                if (randomSelectedSkill is SpikedAbility spikedAbility)
                {
                    if (!spikedAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Spiked");
                        spikedAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteSpikedAbility(enemyManager, spikedAbility));
                    }
                }
                if (randomSelectedSkill is UnstableSpawnAbility unstableSpawnAbility)
                {
                    if (!unstableSpawnAbility.isSkillExecuting)
                    {
                        Debug.Log("Ability: Unstable Spawn");
                        unstableSpawnAbility.isSkillExecuting = true;
                        StartCoroutine(ExecuteUnstableSpawnAbility(enemyManager, unstableSpawnAbility));
                    }
                }
            }
        }
    }

    private IEnumerator ExecuteDancerAbility(EnemyManager enemyManager, DancerAbility dancerAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(dancerAbility.statIncreaseInterval);

        enemyManager.attack += dancerAbility.additionalAttackPerInterval;
        enemyManager.agility += dancerAbility.additionalAgilityPerInterval;

        Debug.Log("Ability Executed.");
        dancerAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteSlasherAbility(EnemyManager enemyManager, SlasherAbility slasherAbility)
    {
        Debug.Log("Executing Ability.");
        int originalAgility = enemyManager.agility;

        yield return new WaitForSeconds(slasherAbility.boostInterval);
        enemyManager.agility = Mathf.RoundToInt(originalAgility * slasherAbility.attackSpeedMultiplier);
        Debug.Log("Slasher Active.");
        yield return new WaitForSeconds(slasherAbility.boostDuration);
        enemyManager.agility = originalAgility;
        Debug.Log("Slasher Inactive.");
        slasherAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteBunkerAbility(EnemyManager enemyManager, BunkerAbility bunkerAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        bunkerAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteManaMageAbility(EnemyManager enemyManager, ManaMageAbility manaMageAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        manaMageAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteNocturnalAbility(EnemyManager enemyManager, NocturnalAbility nocturnalAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        nocturnalAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteSlimerAbility(EnemyManager enemyManager, SlimerAbility slimerAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        slimerAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteSpikedAbility(EnemyManager enemyManager, SpikedAbility spikedAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        spikedAbility.isSkillExecuting = false;
    }

    private IEnumerator ExecuteUnstableSpawnAbility(EnemyManager enemyManager, UnstableSpawnAbility unstableSpawnAbility)
    {
        Debug.Log("Executing Ability.");
        yield return new WaitForSeconds(1);
        unstableSpawnAbility.isSkillExecuting = false;
    }
}
