using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBars : MonoBehaviour
{
    [Header("Connections")]
    public Player player;
    public PlayerDeathScreen playerDeathScreen;

    [Header("Health Slider Player")]
    public Slider playerHealthBar;

    [Header("Player Health Values")]
    public int playerFullHealth;
    public int playerCurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        playerFullHealth = player.health;
        playerHealthBar.maxValue = player.health;
        playerHealthBar.value = playerFullHealth;
        playerCurrentHealth = playerFullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthBar.value = playerCurrentHealth;
    }

    public void PlayerUpdateHealth()
    {
        playerFullHealth = player.health;
        playerHealthBar.maxValue = player.health;
    }

    public void Heal()
    {
        int currentHealth = playerCurrentHealth;

        // Calculate 20% of maxHealth
        int healAmount = Mathf.RoundToInt(playerFullHealth * 0.2f);

        // Add the healAmount to currentHealth
        currentHealth += healAmount;

        // Make sure currentHealth does not exceed maxHealth
        currentHealth = Mathf.Min(currentHealth, playerFullHealth);

        // Update playerStats with the new currentHealth value
        playerCurrentHealth = currentHealth;
    }
}