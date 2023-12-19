using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float RunSpeedMultiplier = 2;
    [SerializeField] private LayerMask jumpingLayer;
    [SerializeField] private SpriteAnimator[] spriteAnimators;
    private Vector2 moveInput;
    private bool isJumping;
    private bool isRunning;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.CharacterControls.Enable();
        inputActions.CharacterControls.Move.performed += OnMoveInput;
        inputActions.CharacterControls.Jump.performed += OnJumpInput;
        inputActions.CharacterControls.Run.performed += OnRunInput;
        inputActions.CharacterControls.Move.canceled += OnMoveCanceled;
        inputActions.CharacterControls.Run.performed += OnRunInput;
        inputActions.CharacterControls.Run.canceled += OnRunCanceled;
    }
    private void OnDisable()
    {
        inputActions.CharacterControls.Disable();
        inputActions.CharacterControls.Move.performed -= OnMoveInput;
        inputActions.CharacterControls.Jump.performed -= OnJumpInput;
        inputActions.CharacterControls.Move.canceled -= OnMoveCanceled;
    }
    private void Update()
    {
        // Calculate movement direction
        float horizontalMovement = moveInput.x;
        float verticalMovement = moveInput.y;

        // Add run multiplier
        if (isRunning)
        {
            horizontalMovement *= RunSpeedMultiplier;
            verticalMovement *= RunSpeedMultiplier;
        }

        // Normalize diagonal movement to restrict to four directions
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            if (Mathf.Abs(horizontalMovement) > Mathf.Abs(verticalMovement))
            {
                verticalMovement = 0f;
            }
            else
            {
                horizontalMovement = 0f;
            }
        }

        // Move the character along the X or Y axis based on input
        Vector3 movement = new Vector3(horizontalMovement, verticalMovement, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Rest of your jump logic...
    }
    private void PlayAnimation(string animationName,bool loop)
    {
        foreach (var item in spriteAnimators)
        {
            item.PlayAnimationByName(animationName, loop);
        }
    }

    #region InputCallbacks
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (!isJumping)
        {
            isJumping = true;

            // Change the collider's layer to the jump layer
            gameObject.layer = jumpingLayer;

            // Implement animation logic here

            // For example, you can trigger your own animator logic
            // animator.SetBool("IsJumping", true);
        }
    }
    private void OnRunInput(InputAction.CallbackContext context)
    {
        isRunning = true;
    }
    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        isRunning = false;
    }
    #endregion
}