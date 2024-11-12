using UnityEngine;

// CODE AND TUTORIAL FROM: https://www.youtube.com/watch?v=f473C43s8nE&ab_channel=Dave%2FGameDevelopment

public class MoveCam : MonoBehaviour
{

    public Transform camPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = camPosition.position;
    }
}
