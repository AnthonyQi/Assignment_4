using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    
    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedGravity = -2f;
    public float terminalVelocity = -50f;
    
    [Header("Ground Detection")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask = -1; // Everything by default
    
    [Header("Smoothing")]
    public float slopeForceDown = 8f; // Extra downward force on slopes
    
    [Header("References")]
    public Transform orientation;
    
    private CharacterController controller;
    private Vector2 moveInput;
    private InputSystem_Actions inputActions;
    private float verticalVelocity = 0f;
    private bool isGrounded;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem_Actions();
    }
    
    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    
    void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
    }
    
    void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        // Better ground check using raycast
        isGrounded = IsGroundedCheck();
        
        // Horizontal movement (camera/orientation relative)
        Vector3 forward = orientation.forward;
        Vector3 right = orientation.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        
        Vector3 move = forward * moveInput.y + right * moveInput.x;
        Vector3 horizontalVelocity = move * moveSpeed;
        
        // Snap to ground if close (eliminates jitter on uneven terrain)
        if (isGrounded && Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 
            out RaycastHit groundHit, groundCheckDistance + 0.6f, groundMask, QueryTriggerInteraction.Ignore))
        {
            float distToGround = groundHit.distance - 0.1f; // Account for offset
            
            // Snap down if hovering above ground
            if (distToGround > controller.skinWidth && distToGround < 0.5f)
            {
                controller.Move(Vector3.down * (distToGround - controller.skinWidth));
            }
            
            verticalVelocity = groundedGravity;
        }
        else
        {
            // In air - apply gravity
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }
        
        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(finalVelocity * Time.deltaTime);
    }
    
    bool IsGroundedCheck()
    {
        // Cast from center bottom of controller
        Vector3 origin = transform.position + Vector3.up * (controller.radius * 0.5f);
        float checkDist = controller.radius + groundCheckDistance;
        
        // Use SphereCast for more reliable ground detection
        return Physics.SphereCast(origin, controller.radius * 0.9f, Vector3.down, 
            out RaycastHit hit, checkDist, groundMask, QueryTriggerInteraction.Ignore);
    }
}