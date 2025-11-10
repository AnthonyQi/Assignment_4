using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedGravity = -2f; // small downward force when grounded to keep contact
    public float terminalVelocity = -50f;

    [Header("References")]
    public Transform orientation; // assign the orientation from PlayerCam

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
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled  += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled  -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // horizontal (camera/orientation relative)
        Vector3 forward = orientation.forward;
        Vector3 right   = orientation.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();
        Vector3 move = forward * moveInput.y + right * moveInput.x;
        Vector3 horizontalVelocity = move * moveSpeed;

        // gravity / vertical velocity
        if (controller.isGrounded)
        {
            // small downward force to keep controller "stuck" to slopes/ground
            if (verticalVelocity < 0f)
                verticalVelocity = groundedGravity;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }

        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        // CharacterController.Move expects displacement (meters), not velocity
        controller.Move(finalVelocity * Time.deltaTime);
    }
}
