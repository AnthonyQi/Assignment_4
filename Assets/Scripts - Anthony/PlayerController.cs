using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    
    [Header("Gravity")]
    public float gravity = -9.81f;
    public float terminalVelocity = -50f;
    
    [Header("References")]
    public Transform orientation;
    
    private CharacterController controller;
    private Vector2 moveInput;
    private InputSystem_Actions inputActions;
    private float verticalVelocity = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        // Unsubscribe first to prevent duplicate subscriptions
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        // Now subscribe
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Horizontal movement based on orientation (camera direction)
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;
        
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;
        Vector3 horizontalVelocity = moveDirection * moveSpeed;
        
        // Apply gravity constantly
        verticalVelocity += gravity * Time.deltaTime;
        
        // Clamp to terminal velocity
        if (verticalVelocity < terminalVelocity)
            verticalVelocity = terminalVelocity;
        
        // Reset vertical velocity when grounded
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // Small downward force to stay grounded
        }
        
        // Combine horizontal and vertical movement
        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;
        
        // Move the character
        controller.Move(finalVelocity * Time.deltaTime);
    }
}