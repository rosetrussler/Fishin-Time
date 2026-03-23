using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This handle the spawning of fish in the source node
/// </summary>
public class SpawnFish : MonoBehaviour
{
    private Vector3 m_fishMoveDirection;
    private Vector3 m_fishSpawnPoint;
    [SerializeField] private FishMovement[] m_fishPool;
    private int m_fishPoolLength;
    public event Action<Vector3, Vector3> OnSpawnFish;

    private void Awake()
    {
        //get values for calculations in the program

        //get direction of fish movement 
        Vector3 sourceNodeVector = transform.position;                    //aka vector OA
        Vector3 sinkNodeVector = transform.parent.GetChild(1).position;   //aka vector OB

        //AB = AO + OB == AB = OB - OA == vector of source to sink 
        Vector3 distanceVector = sinkNodeVector - sourceNodeVector;
        //normailzed vector = fish move direction
        m_fishMoveDirection = Vector3.Normalize(distanceVector);    //this won't change throughout runtime as the position of the source and sink nodes should be static

        //get fish spawnpoint 
        m_fishSpawnPoint = sourceNodeVector;

        //get number of fish in pool
        m_fishPoolLength = m_fishPool.Length;

        //bind to events 
        transform.parent.GetChild(1).GetComponent<FishOnSink>().OnFishReachedSink += FishReachedSinkHandler;
    }

    private void Start()
    {
        SpawnRandomFish();
    }

    /// <summary>
    /// Call a random fish from the pool to spawn
    /// </summary>
    private void SpawnRandomFish()
    {
        bool validSpawn = false;
        while (!validSpawn)
        {
            int fishToSpawn = GetRandomFishFromPool();
            if (fishToSpawn >= 0)
            {
                //check if fish is active, if so find another
                if (m_fishPool[fishToSpawn].IsFishActive() == false)
                {
                    //bind to event, spawn, then unbind
                    OnSpawnFish += m_fishPool[fishToSpawn].HandleOnFishSpawn;
                    OnSpawnFish?.Invoke(m_fishSpawnPoint, m_fishMoveDirection);
                    OnSpawnFish -= m_fishPool[fishToSpawn].HandleOnFishSpawn;
                    validSpawn = true;
                }
            }
            else
            {
                Debug.Log("[!]ERROR: No fish in pool");
                validSpawn = true;
            }
        }
    }

    /// <summary>
    /// Get a random index in the pool array
    /// </summary>
    /// <returns></returns>
    private int GetRandomFishFromPool()
    {
        if (m_fishPoolLength == 0)
        {
            return -1; //if no fish in pool do nothing 
        }
        else
        {
            return UnityEngine.Random.Range(0, m_fishPoolLength - 1);
        }
    }

    public void FishReachedSinkHandler()
    {
        SpawnRandomFish();
    }
}
