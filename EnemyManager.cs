using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    [Header("Connections")]
    public EnemyNextAttack enemyNextAttack;
    public GameEndState gameEndState;
    public PlayerBars playerBars;
    public ProgressAnimation progerssAnimation;

    [Header("Scriptable object lists")]
    public List<ScriptableObject> enemySOList = new List<ScriptableObject>();
    public List<ScriptableObject> floorSOList = new List<ScriptableObject>();
    public List<ScriptableObject> activeBossSkillList = new List<ScriptableObject>();

    [Header("Player information")]
    public PlayerActiveInformation playerActiveInformation;
    public Player player;

    [Header("Enemy & Floor Index")]
    public int currentEnemyIndex = 0;
    public int currentFloorIndex = 0;
    public TextMeshProUGUI floorText;
    public TextMeshProUGUI stageText;

    [Header("Enemy stats post calculations")]
    public int attack;
    public int health;
    public int agility;
    public int defense;
    public int luck;

    [Header("Enemy rewards")]
    public int coinDrops;
    public int coinTotalThisRun;
    public int ringChance;
    public int ringRoll;

    private int skillAlteredAttack;
    private int skillAlteredHealth;
    private int skillAlteredAgility;
    private int skillAlteredDefence;

    public List<ScriptableObject> ChestList = new List<ScriptableObject>();

    [Header("Enemy Health Pool & Slider")]
    public int enemyFullHealth;
    public int enemyCurrentHealth;
    public Slider enemyHealthBar;

    [Header("True statements")]
    public bool isActiveWin = true;
    public bool rewardGiven = false;
    public bool enemyStatsLoaded = false;
    public bool bossIsHere = false;

    public void Start()
    {
        //Load First floor into EnemyList
        LoadFloorEnemies();
    }
        
    public void Update()
    {
        if (enemyCurrentHealth <= 0)
        {
            enemyStatsLoaded = false;
            if (rewardGiven == false)
            {
                //Add coins to total to coinTotalThisRun.
                coinTotalThisRun += coinDrops;
                rewardGiven = true;
            }
            if  (currentEnemyIndex < enemySOList.Count)
            {
                activeBossSkillList = null;
                SwitchToNextEnemy();
                enemyNextAttack.ResetCoroutine();
                enemyNextAttack.enemyNextAttackProgress = 0;
                gameEndState.playerLevelComplete = false;
            }
            else if (currentEnemyIndex >= enemySOList.Count)
            {
                //currentEnemyIndex -= currentEnemyIndex; //repeating loop.
                if (isActiveWin == true)
                {
                    gameEndState.FloorIsCleared();
                    isActiveWin = false;
                }
            }

        }
        //enemyHealthBar.value = enemyCurrentHealth;
    }


    void LoadEnemyStatsFromSO(ScriptableObject enemySO)
    {
        if (enemySO is EnemyStatistics)
        {
            EnemyStatistics enemyStats = (EnemyStatistics)enemySO;

            //Check amount of variations and select a random one.
            int randomIndex = Random.Range(0, enemyStats.EnemyVariation.Count);
            EnemyVariationSO randomVariation = enemyStats.EnemyVariation[randomIndex] as EnemyVariationSO;

            // Randomize the stats using the values from the selected variation
            int attackPercentage = Random.Range(randomVariation.minAttack, randomVariation.maxAttack + 1);
            int healthPercentage = Random.Range(randomVariation.minHealth, randomVariation.maxHealth + 1);
            int agilityPercentage = Random.Range(randomVariation.minAgility, randomVariation.maxAgility + 1);
            int defensePercentage = Random.Range(randomVariation.minDefense, randomVariation.maxDefense + 1);
            int luckPercentage = Random.Range(randomVariation.minLuck, randomVariation.maxLuck + 1);

            // Load the stats from the EnemyStatistics combined with random offset into the EnemyManager
            attack = ((enemyStats.attack * (attackPercentage + 100)) / 100);
            health = ((enemyStats.health * (healthPercentage + 100)) / 100);
            agility = ((enemyStats.agility * (agilityPercentage + 100)) / 100);
            defense = ((enemyStats.defense * (defensePercentage + 100)) / 100);
            luck = ((enemyStats.luck * (luckPercentage + 100)) / 100);
            coinDrops = enemyStats.GetRandomDropCoinAmount();
            enemyStatsLoaded = true;

            enemyFullHealth = enemyStats.health;
            enemyCurrentHealth = enemyStats.health;
            enemyHealthBar.maxValue = enemyFullHealth;
            enemyHealthBar.value = enemyFullHealth;
        }
        else if (enemySO is BossStatistics)
        {
            BossStatistics bossStats = (BossStatistics)enemySO;
            bossIsHere = true;
            // Load the stats from the BossStatistics object into the EnemyManager
            activeBossSkillList = bossStats.BossSkillList;
            ResetSkillCooldown(); //Reset cooldown of any skill that might have still been set as true
            AlteredStatsThroughSkill(); //Grab altered Stats from boss skills
            Debug.Log(bossStats.health + "This" + skillAlteredHealth + "Formula" + ((bossStats.health * (skillAlteredHealth + 100)) / 100));
            attack = ((bossStats.attack * (skillAlteredAttack + 100)) /100);
            health = ((bossStats.health * (skillAlteredHealth + 100)) / 100);
            agility = ((bossStats.agility * (skillAlteredAgility + 100)) / 100);
            defense = ((bossStats.defense * (skillAlteredDefence + 100)) / 100);
            luck = bossStats.luck;
            // Load the boss rewards.
            coinDrops = bossStats.GetRandomDropCoinAmount();
            enemyStatsLoaded = true;
            //set ring stats + RNG roll.
            ringChance = bossStats.ringDropChance;
            ringRoll = Random.Range(1, 100);
            foreach (ChestSO chestType in bossStats.chestTypes)
            {
                ChestList.Add(chestType);
            }

            enemyFullHealth = bossStats.health;
            enemyCurrentHealth = bossStats.health;
            enemyHealthBar.maxValue = enemyFullHealth;
            enemyHealthBar.value = enemyFullHealth;
        }
        else
        {
            Debug.LogError("Unknown ScriptableObject type: " + enemySO.GetType());
        }
    }

    void SwitchToNextEnemy()
    {
        bossIsHere = false;
        if (currentEnemyIndex >= enemySOList.Count)
        {
            Debug.Log("Last enemy of level has been defeated.");
            gameEndState.FloorIsCleared();
            return;
        }

        // Get the next enemy ScriptableObject
        ScriptableObject nextEnemySO = enemySOList[currentEnemyIndex];

        // Increase the currentEnemyIndex for the next time the function is called
        currentEnemyIndex++;
        playerBars.Heal();
        // Update the stageText
        int currentEnemyIndexInList = currentEnemyIndex;
        int floorCount = enemySOList.Count - 1;
        if (floorCount < currentEnemyIndexInList)
        {
            stageText.text = "BOSS";
            progerssAnimation.LevelProgressAnimation();
        }
        else
        {
            stageText.text = currentEnemyIndexInList + "/" + floorCount;
            progerssAnimation.LevelProgressAnimation();
        }


        // Load the stats for the next enemy
        LoadEnemyStatsFromSO(nextEnemySO);
        rewardGiven = false;
    }

    public void LoadFloorEnemies()
    {
        // Check if currentFloorIndex is within the bounds of the floorSOList
        if (currentFloorIndex >= floorSOList.Count)
        {
            Debug.Log("No more floors.");
            enemySOList.Clear(); // Clear the list before adding enemies
            return;
        }

        // Get the current floor
        FloorSO currentFloor = floorSOList[currentFloorIndex] as FloorSO;
        floorText.text = currentFloor.FloorName;
        // Add all the enemies and bosses from the current floor to the enemySOList
        currentFloor.AddEnemiesToEnemySOList(enemySOList);
        isActiveWin = true;
    }

    public void ResetSkillCooldown()
    {
        foreach (ScriptableObject bossSkill in activeBossSkillList)
        {
            if (bossSkill is DancerAbility dancerAbility)
            {
                dancerAbility.isSkillExecuting = false;
            }
            if (bossSkill is SlasherAbility slasherAbility)
            {
                slasherAbility.isSkillExecuting = false;
            }
            if (bossSkill is BunkerAbility bunkerAbility)
            {
                bunkerAbility.isSkillExecuting = false;
            }
            if (bossSkill is ManaMageAbility manaMageAbility)
            {
                manaMageAbility.isSkillExecuting = false;
            }
            if (bossSkill is NocturnalAbility nocturnalAbility)
            {
                nocturnalAbility.isSkillExecuting = false;
            }
            if (bossSkill is SlimerAbility slimerAbility)
            {
                slimerAbility.isSkillExecuting = false;
            }
            if (bossSkill is SpikedAbility spikedAbility)
            {
                spikedAbility.isSkillExecuting = false;
            }
            if (bossSkill is UnstableSpawnAbility unstableSpawnAbility)
            {
                unstableSpawnAbility.isSkillExecuting = false;
            }
        }
    }

    private void AlteredStatsThroughSkill()
    {

        foreach (ScriptableObject bossSkill in activeBossSkillList)
        {
            if (bossSkill is BossAbility bossAbility)
            {
                // Accumulate the altered stats from each skill
                skillAlteredAttack += bossAbility.alteredStatsAttack;
                skillAlteredHealth += bossAbility.alteredStatsHealth;
                skillAlteredAgility += bossAbility.alteredStatsAgility;
                skillAlteredDefence += bossAbility.alteredStatsDefence;
            }
        }
    }
}
