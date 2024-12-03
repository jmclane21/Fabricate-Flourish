using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the PauseMenu Canvas
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the center
        Cursor.visible = false; // Hide the cursor
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before switching scenes
        Cursor.lockState = CursorLockMode.None; // Ensure cursor is unlocked
        Cursor.visible = true; // Make the cursor visible
        SceneManager.LoadScene("UI"); // Replace "MainMenu" with your scene name
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
