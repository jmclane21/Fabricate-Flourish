using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public GameObject optionsPanel;
    public GameObject mainMenuPanel; // Reference to the Main Menu panel
    public GameObject creditsPanel; // Reference to the Credits panel

    public void StartGame()
    {
        // Load the game scene (make sure your game scene is added in Build Settings)
        SceneManager.LoadScene("Wk2-125");
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false); // Disable Main Menu

        // Add functionality for options, like opening an options menu or panel
        optionsPanel.SetActive(true);

    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true); // Enable Main Menu

    }

    public void ShowCredits()
    {
         creditsPanel.SetActive(true); // Show Credits Panel
    mainMenuPanel.SetActive(false); // Hide Main Menu

    }

    public void CloseCredits()
    {
    creditsPanel.SetActive(false); // Hide Credits Panel
    mainMenuPanel.SetActive(true); // Show Main Menu
    }
}
