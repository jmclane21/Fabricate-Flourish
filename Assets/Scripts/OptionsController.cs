using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMesh Pro support


public class OptionsController : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown graphicsDropdown; // Change to TMP_Dropdown

    void Start()
    {
        // Initialize volume and quality settings
        volumeSlider.value = AudioListener.volume;
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        volumeSlider.onValueChanged.AddListener(SetVolume);
        graphicsDropdown.onValueChanged.AddListener(SetGraphicsQuality);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Sets the global volume
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // Adjusts quality based on dropdown
    }
}
