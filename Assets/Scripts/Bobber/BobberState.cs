using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// The state of the bobber, this requires BobberMovement and PlayerInput components
/// </summary>
[RequireComponent(typeof(BobberMovement))]
[RequireComponent (typeof(PlayerInput))]
public class BobberState : MonoBehaviour
{
    //what the bobber is doing
    enum BobberStateMachine
    {
        outOfWater,
        casting,
        inWater,
        reeling
    }

    private BobberStateMachine m_bobberState;
    private PlayerInput m_playerInput;

    public event Action OnStopMovement;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        BobberMovement bobberMovement = GetComponent<BobberMovement>();
        bobberMovement.OnCast += SwitchToInWater;
        bobberMovement.OnStartCast += SwitchToCasting;
        bobberMovement.OnCancelCast += SwitchToOutOfWater;
        
    }

    private void Start()
    {
        //bobber starts out of water 
        SwitchToOutOfWater();
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //if bobber touches fish it is reeling time
    {
        if (collision.CompareTag("Fish"))
        {
            SwitchToReeling();
        }
    }

    /// <summary>
    /// Switch the state machine and handle what controls are enabled
    /// </summary>
    private void SwitchToOutOfWater()
    {
        if (m_bobberState != BobberStateMachine.inWater)    //this is to stop the release of the cast button from setting the bobber to out of water, this shouldn't affect anything else as these states should not directly transition between eachother
        {
            m_bobberState = BobberStateMachine.outOfWater;
            m_playerInput.actions.FindAction("Move").Disable();
            m_playerInput.actions.FindAction("Cast").Enable();
        }
    }

    /// <summary>
    /// Switch the state machine to casting 
    /// </summary>
    private void SwitchToCasting()
    {
        m_bobberState = BobberStateMachine.casting;
    }

    /// <summary>
    /// Switch the state machine to inWater and handle what controls are enabled
    /// </summary>
    private void SwitchToInWater()
    {
        m_bobberState = BobberStateMachine.inWater;
        m_playerInput.actions.FindAction("Cast").Disable();
        m_playerInput.actions.FindAction("Move").Enable();
    }

    private void SwitchToReeling()
    {
        m_bobberState = BobberStateMachine.reeling;
        m_playerInput.actions.FindAction("Move").Disable();
        OnStopMovement?.Invoke();
    }

    private void OnDestroy()
    {
        //unsubscribe from events
        BobberMovement bobberMovement = GetComponent<BobberMovement>();
        if (bobberMovement != null)
        {
            bobberMovement.OnCast -= SwitchToInWater;
            bobberMovement.OnStartCast -= SwitchToCasting;
            bobberMovement.OnCancelCast -= SwitchToOutOfWater;
        }
    }
}
