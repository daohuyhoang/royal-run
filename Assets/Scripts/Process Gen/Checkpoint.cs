using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float checkpointTimeExtension = 5f;
    
    GameManager gameManager;
    PlayerController playerController;

    const string stringPlayer = "Player";

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(stringPlayer))
        {
            gameManager.IncreaseTime(checkpointTimeExtension);
        }
    }
}
