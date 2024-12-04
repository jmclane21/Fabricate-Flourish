using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMesh Pro support


public class OptionsController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Initialize volume and quality settings
        volumeSlider.value = AudioListener.volume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Sets the global volume
    }
}
