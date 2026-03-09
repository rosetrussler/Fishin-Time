using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BobberMovement : MonoBehaviour
{

    [SerializeField] private float m_bobberMoveSpeed;
    [SerializeField] private float m_moveCooldown;
    private float m_moveTimer;

    //this is the magnitude of the vector got by mouse position
    private Vector2 m_moveDirection;


    public event Action OnCast;
    public event Action OnStartCast;
    public event Action OnCancelCast;

    private void Awake()
    {
        GetComponent<BobberState>().OnStopMovement += StopMovementHandler;
    }

    private void Update()
    {
        transform.position += new Vector3(m_moveDirection.x * m_bobberMoveSpeed * Time.deltaTime, m_moveDirection.y * m_bobberMoveSpeed * Time.deltaTime, 0);
        m_moveTimer = m_moveCooldown;
    }

    public void HandleMove(InputAction.CallbackContext ctx) //this is called ALOT per second, in future improve this
    {

        if (Time.time > m_moveTimer)   //only do when not in cooldown
        {
            //should only do when cast
            if (ctx.performed)
            {
                Debug.Log("Move: " + ctx.ReadValue<Vector2>());
                m_moveDirection = ctx.ReadValue<Vector2>();
                m_moveTimer = Time.time + m_moveCooldown;
            }
        }


    }

    public void HandleCast(InputAction.CallbackContext ctx)
    {
        //can only do when not already cast
        if (ctx.performed)
        {
            Debug.Log("Cast performed");
            OnCast?.Invoke();
        }
        else if (ctx.started)
        {
            OnStartCast?.Invoke();
        }
        else if (ctx.canceled)
        {
            OnCancelCast?.Invoke();
        }
    }

    public void HandleClickOnce(InputAction.CallbackContext ctx)
    {
        //can only do when already cast
        if (ctx.performed)
        {
            Debug.Log("ClickOnce performed");
        }
    }

    /// <summary>
    /// set the move vector to 0
    /// </summary>
    private void StopMovementHandler()
    {
        m_moveDirection = Vector3.zero;
    }

    private void OnDestroy()
    {
        //unsub from events
        BobberState bobberState = GetComponent<BobberState>();
        if (bobberState != null)
        {
            bobberState.OnStopMovement -= StopMovementHandler;
        }
    }
}
