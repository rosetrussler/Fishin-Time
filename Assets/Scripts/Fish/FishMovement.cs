using System;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float m_fishSpeed;
    [SerializeField] private Vector3 m_fishPoolLocation;
    private Vector3 m_fishMovementDirectionTempHolder;
    private Vector3 m_fishMovementDirection;
    bool m_isActive = false;

    public event Action OnFishDespawn;

    private void Awake()
    {
        //subscribe to events
        FishDetectCatch fishDetectCatch = transform.GetComponent<FishDetectCatch>();
        fishDetectCatch.OnFishCaught += HandleOnFishCaught;
        fishDetectCatch.OnReelFish += HandleOnFishReel;
        fishDetectCatch.OnFishEscape += HandleOnFishRelease;
    }

    private void Update()
    {
        transform.position += m_fishMovementDirection * m_fishSpeed * Time.deltaTime;
    }

    public void HandleOnFishSpawn(Vector3 spawnPoint, Vector3 directionOfTravel)
    {
        Debug.Log("Spawn");
        m_fishMovementDirection = directionOfTravel;
        transform.position = spawnPoint;
        m_isActive = true;
    }

    public void HandleDespawn()
    {
        Debug.Log("Despawn");
        m_fishMovementDirection = new Vector3(0, 0, 0);
        transform.position = m_fishPoolLocation;
        m_isActive = false;
        OnFishDespawn?.Invoke();
    }

    private void HandleOnFishCaught()
    {
        PauseMovement();
    }
    private void PauseMovement()
    {
        m_fishMovementDirectionTempHolder = m_fishMovementDirection;
        m_fishMovementDirection = Vector3.zero;
    }

    private void ResumeMovement()
    {
        m_fishMovementDirection = m_fishMovementDirectionTempHolder;
    }

    public void HandleOnFishReel()
    {
        Debug.Log("FISH REEL!!!!!");
        HandleDespawn();
    }
    public void HandleOnFishRelease()
    {
        ResumeMovement();
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
