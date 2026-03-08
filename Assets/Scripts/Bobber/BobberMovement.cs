using UnityEngine;
using UnityEngine.InputSystem;

public class BobberMovement : MonoBehaviour
{
    [SerializeField] private float m_bobberMoveSpeed;

    //this is the magnitude of the vector got by mouse position
    private Vector2 m_moveDirection;

    private void Update()
    {
           transform.position += new Vector3(m_moveDirection.x * m_bobberMoveSpeed * Time.deltaTime, m_moveDirection.y * m_bobberMoveSpeed * Time.deltaTime, 0);
    }

    public void HandleMove(InputAction.CallbackContext ctx) //this is called ALOT per second, in future improve this
    {
        //should only do when cast
        if (ctx.performed)
        {
            Debug.Log("Move: " + ctx.ReadValue<Vector2>());
            m_moveDirection = ctx.ReadValue<Vector2>();
        }

    }

    public void HandleCast(InputAction.CallbackContext ctx)
    {
        //can only do when not already cast
        if (ctx.performed)
        {
            Debug.Log("Cast performed");
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
}
