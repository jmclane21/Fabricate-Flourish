using UnityEngine;
using UnityEngine.SceneManagement; // For scene management

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Reference to the Game Over panel
    public GameObject player; // Reference to the player GameObject
    private bool isGameOver = false; // Track if the game is over

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            RestartGame(); // Restart the game if the game is over
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true; // Set the game over state
        gameOverPanel.SetActive(true); // Show the Game Over panel
        Time.timeScale = 0; // Pause the game

        // Disable the player
        if (player != null)
        {
            player.SetActive(false); // Disable the player GameObject
        }
    }

    public void RestartGame()
    {
        isGameOver = false; // Reset the game over state
        gameOverPanel.SetActive(false); // Hide the Game Over panel
        Time.timeScale = 1; // Resume the game

        // Enable the player when restarting
        if (player != null)
        {
            player.SetActive(true); // Enable the player GameObject
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
