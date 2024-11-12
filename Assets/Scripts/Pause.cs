using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI elements

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the pause menu panel
    private bool isPaused = false; // Track if the game is paused

    void Update()
    {
        // Check for the P key press to toggle pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true; // Set the game to paused state
        pauseMenuPanel.SetActive(true); // Show the pause menu panel
        Time.timeScale = 0; // Pause the game
    }

    public void ResumeGame()
    {
        isPaused = false; // Reset the paused state
        pauseMenuPanel.SetActive(false); // Hide the pause menu panel
        Time.timeScale = 1; // Resume the game
    }

    public void QuitGame()
    {
        // This can be used to quit to the main menu or exit the application
        // For quitting the application in standalone builds
        Application.Quit();

        // For Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
