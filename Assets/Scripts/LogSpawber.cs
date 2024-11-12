using System.Collections;
using UnityEngine;
using TMPro;  // For TextMeshPro support

// thank you chatgpt :3
public class LogGeneratorWithPrefab : MonoBehaviour
{
    public GameObject logPrefab;  // Prefab of the log UI element
    public Transform logParent;   // Parent object to organize log prefabs in the UI
    public int logCount = 50;     // Number of log entries to generate
    public float noiseLevel = 0.3f;  // Amount of noise (0 = no noise, 1 = high noise)
    public float logInterval = 1f;   // Time interval between log entries

    private string[] logMessages = {
        "Player position updated",
        "Enemy detected nearby",
        "Mountain range spawned",
        "Health updated",
        "Resource collected",
        "Level up",
        "Weapon fired"
    };

    void Start()
    {
        StartCoroutine(GenerateLogsWithNoise());
    }

    // Coroutine that generates logs with random noise at intervals
    IEnumerator GenerateLogsWithNoise()
    {
        for (int i = 0; i < logCount; i++)
        {
            string logEntry = GenerateLogEntryWithNoise();
            DisplayLogWithPrefab(logEntry);  // Display log using the prefab
            yield return new WaitForSeconds(logInterval);
        }
    }

    // Generate a random log entry with noise
    private string GenerateLogEntryWithNoise()
    {
        // Pick a random base message
        string baseMessage = logMessages[Random.Range(0, logMessages.Length)];

        // Add noise to the message
        string noisyMessage = AddNoiseToMessage(baseMessage);

        return noisyMessage;
    }

    // Add random noise to the log message
    private string AddNoiseToMessage(string message)
    {
        string noise = "";
        if (Random.value < noiseLevel)
        {
            // Generate random noisy data (e.g., random characters)
            int noiseLength = Random.Range(5, 15);  // Random length for noise
            for (int i = 0; i < noiseLength; i++)
            {
                noise += (char)Random.Range(33, 126);  // Random printable ASCII characters
            }
        }

        // Inject noise either before, after, or inside the message
        switch (Random.Range(0, 3))
        {
            case 0: return noise + " " + message;    // Noise before
            case 1: return message + " " + noise;    // Noise after
            case 2: return InsertNoiseInMessage(message, noise);  // Noise in the middle
            default: return message;
        }
    }

    // Insert noise in a random position within the message
    private string InsertNoiseInMessage(string message, string noise)
    {
        int insertPosition = Random.Range(0, message.Length);
        return message.Substring(0, insertPosition) + noise + message.Substring(insertPosition);
    }

    // Display the log using a prefab
    private void DisplayLogWithPrefab(string logEntry)
    {
        // Instantiate the log prefab
        GameObject logObject = Instantiate(logPrefab, logParent);

        // Find the TextMeshPro or UI Text component on the prefab and set the log entry
        TextMeshProUGUI logText = logObject.GetComponentInChildren<TextMeshProUGUI>();
        if (logText != null)
        {
            logText.text = logEntry;
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on log prefab.");
        }

        // Optionally, you can customize the position or appearance of the log here
    }
}
