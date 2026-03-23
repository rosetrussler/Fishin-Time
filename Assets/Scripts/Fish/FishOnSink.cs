using System;
using UnityEngine;

public class FishOnSink : MonoBehaviour
{
    public event Action OnFishReachedSink;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            OnFishReachedSink?.Invoke();
        }
    }
}
