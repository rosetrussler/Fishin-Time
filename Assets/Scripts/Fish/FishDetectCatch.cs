using UnityEngine;
using System;

public class FishDetectCatch : MonoBehaviour
{
    [SerializeField] private string m_sequenceName;

    public event Action OnFishCaught;
    public event Action<string> OnStartSequence;

    private void Awake()
    {
        //bind to sequence manager
        OnStartSequence += FindFirstObjectByType<SequenceManager>().HandleOnStartSequence;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("FishDetectCatch: OnTriggerEnter with " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            //be caught
            Debug.Log("fish caught");
            OnStartSequence?.Invoke(m_sequenceName);
            OnFishCaught?.Invoke();
        }
    }

}
