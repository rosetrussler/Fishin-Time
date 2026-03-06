using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float m_fishSpeed;
    [SerializeField] private Vector3 m_fishPoolLocation;
    bool m_isActive = false;

    private void Awake()
    {
        //subscribe to events
        transform.parent.GetComponent<FishDetectCatch>().OnFishCaught += HandleOnFishCaught;
    }

    private void Update()
    {
        //if active, move in direction of travel
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FishSink"))
        {
            HandleDespawn();
        }
    }

    private void HandleSpawn(Vector2 spawnPoint, Vector2 directionOfTravel)
    {
        //move to given spawn (fish spawn that spawned the fish)
        //activate fish
        //start moving in given direction
    }

    private void HandleDespawn()
    {
        //stop moving
        //deactivate fish
        //move to pool
    }

    private void HandleOnFishCaught()
    {
        //deactivate fish
        //start catch sequence
    }

    private void OnDestroy()
    {
        //unsubscribe to events
        transform.parent.GetComponent<FishDetectCatch>().OnFishCaught -= HandleOnFishCaught;
    }
}
