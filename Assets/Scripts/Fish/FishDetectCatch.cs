using UnityEngine;
using System;

public class FishDetectCatch : MonoBehaviour
{
    public enum SequenceType
    {
        parent, 
    }

    [SerializeField] private SequenceType m_fishSequence;

    public event Action OnFishCaught;
    public event Action<SequenceType> OnStartSequence;

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
            OnStartSequence?.Invoke(m_fishSequence);
            OnFishCaught?.Invoke();
        }
    }

}
