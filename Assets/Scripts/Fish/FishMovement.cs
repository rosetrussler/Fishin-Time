using System;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float m_fishSpeed;
    [SerializeField] private Vector3 m_fishPoolLocation;
    private Vector3 m_fishMovementDirection;
    bool m_isActive = false;

    private void Awake()
    {
        //subscribe to events
        transform.GetComponent<FishDetectCatch>().OnFishCaught += HandleOnFishCaught;
    }

    private void Update()
    {
        transform.position += m_fishMovementDirection * m_fishSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FishSink"))
        {
            HandleDespawn();
        }
    }

    public void HandleOnFishSpawn(Vector3 spawnPoint, Vector3 directionOfTravel)
    {
        Debug.Log("Spawn");
        m_fishMovementDirection = directionOfTravel;
        transform.position = spawnPoint;
        m_isActive = true;
    }

    private void HandleDespawn()
    {
        Debug.Log("Despawn");
        m_fishMovementDirection = new Vector3(0, 0, 0);
        transform.position = m_fishPoolLocation;
        m_isActive = false;
    }

    public void HandleOnFishCaught()
    {
       
    }

    public bool IsFishActive()
    {
        return m_isActive;
    }

    private void OnDestroy()
    {
        //unsubscribe to events
        transform.parent.GetComponent<FishDetectCatch>().OnFishCaught -= HandleOnFishCaught;
    }

}
