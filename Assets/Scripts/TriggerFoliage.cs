using UnityEngine;

public class TriggerFoliageSpawner : MonoBehaviour
{
    public FoliageManager foliageManager;

    // Method called when the specific event occurs (e.g., player picks up an item)
    public void TriggerFoliageSpawn()
    {
        if (foliageManager != null)
        {
            Debug.Log("Triggering foliage spawn...");
            foliageManager.SpawnFoliageNearPlayer();
        }
        else
        {
            Debug.LogError("FoliageManager not assigned in TriggerFoliageSpawner.");
        }
    }
}
