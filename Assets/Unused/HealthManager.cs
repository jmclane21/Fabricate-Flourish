using UnityEngine;
using UnityEngine.UI; // Needed for UI elements

public class HealthManager : MonoBehaviour
{
    public Image healthBar; // Reference to the health bar UI element
    public GameManager gameManager; // Reference to the GameManager script
    public float maxHealth = 100f; // Max health value
    public float currentHealth; // Current health value
    public float depletionRate = 30f; // Health depletion time in seconds
    public float depletionAmount = 10f; // Amount to deplete each interval

    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        healthBar.fillAmount = currentHealth / maxHealth; // Set initial fill amount
        InvokeRepeating(nameof(DepleteHealth), depletionRate, depletionRate); // Start health depletion
    }

    private void DepleteHealth()
    {
        currentHealth -= depletionAmount; // Deplete health by the specified amount
        healthBar.fillAmount = currentHealth / maxHealth; // Update health bar

        // Debug logs to check health values
        Debug.Log($"Current Health: {currentHealth}"); // Log current health

        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go below zero
            gameManager.ShowGameOver(); // Trigger Game Over
        }
    }
}
