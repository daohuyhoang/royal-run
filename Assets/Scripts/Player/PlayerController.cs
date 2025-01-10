using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float xClamp = 3f;
    [SerializeField] float zClamp = 3f;
    [SerializeField] float moveSpeed = 5f;
    
    Vector2 movement;
    Rigidbody rigibody;

    void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement(); 
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 currentPosition = rigibody.position;
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);
        
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);
        
        rigibody.MovePosition(newPosition);
    }
}
