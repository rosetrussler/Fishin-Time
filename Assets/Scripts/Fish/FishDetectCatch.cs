using UnityEngine;
using System;

public class FishDetectCatch : MonoBehaviour
{
    public event Action OnFishCaught;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FishDetectCatch: OnTriggerEnter with " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            //be caught
            Debug.Log("fish caught");
            OnFishCaught?.Invoke();
        }
    }

}
