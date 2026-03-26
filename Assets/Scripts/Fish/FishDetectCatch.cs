using UnityEngine;
using System;

public class FishDetectCatch : MonoBehaviour
{
    public enum SequenceType
    {
        parent, 
        seaBass,
    }

    [SerializeField] private SequenceType m_fishSequence;
    private FishDetectCatch m_self;

    public event Action OnFishCaught;
    public event Action<SequenceType, FishDetectCatch> OnStartSequence;
    public event Action OnPauseMovement;
    public event Action OnReelFish;
    public event Action OnFishEscape;

    private void Awake()
    {
        //bind to sequence manager
        OnStartSequence += FindFirstObjectByType<SequenceManager>().HandleOnStartSequence;
    }

    private void Start()
    {
        m_self = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("FishDetectCatch: OnTriggerEnter with " + collision.gameObject.name);
        if (collision.CompareTag("Player"))
        {
            //be caught
            Debug.Log("fish caught");
            OnStartSequence?.Invoke(m_fishSequence, m_self);
            OnFishCaught?.Invoke();
        }
       
    }

    /// <summary>
    /// Returns the event that the fish was caught, this is then returned from Fish Dectect Catch to Fish Movement
    /// </summary>
    public void ReelFishHandler()
    {
        OnReelFish?.Invoke();
        Debug.Log("REEL FISH IN DETECT CATCH!!!");
    }

    /// <summary>
    /// Returns the event that the fish escaped, this is then returned from Fish Detect Catch to the Fish Movement
    /// </summary>
    public void FishEscapeHandler()
    {
        OnFishEscape?.Invoke();
    }

}
