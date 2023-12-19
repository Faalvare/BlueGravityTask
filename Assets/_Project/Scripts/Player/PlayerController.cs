using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float RunSpeedMultiplier = 2;
    [SerializeField] private LayerMask jumpingLayer;
    [SerializeField] private SpriteAnimator[] spriteAnimators;
    [SerializeField] private Collider2D interactionCollider;
    [SerializeField] private Inventory inventory;
    private Vector2 moveVector;
    private Vector2 characterDirection;
    private bool isJumping;
    private bool isRunning;
    private PlayerInputActions inputActions;
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        inputActions = InputManager.Instance.inputActions;
        inputActions.CharacterControls.Run.performed += OnRunInput;
        inputActions.CharacterControls.Run.canceled += OnRunCanceled;
        inputActions.CharacterControls.Jump.performed += OnJumpInput;
        inputActions.CharacterControls.Interact.performed += OnInteractInput;
        inputActions.CharacterControls.Move.performed += OnMoveInput;
        inputActions.CharacterControls.Move.canceled += OnMoveInputCanceled;
    }
    private void OnDisable()
    {
        inputActions.CharacterControls.Disable();
        inputActions.CharacterControls.Run.performed -= OnRunInput;
        inputActions.CharacterControls.Run.canceled -= OnRunCanceled;
        inputActions.CharacterControls.Jump.performed -= OnJumpInput;
        inputActions.CharacterControls.Interact.performed -= OnInteractInput;
        inputActions.CharacterControls.Move.performed -= OnMoveInput;
        inputActions.CharacterControls.Move.canceled -= OnMoveInputCanceled;
    }
    private void Update()
    {
        Vector2 movement = moveVector * moveSpeed;
        // Add run multiplier
        if (isRunning)
        {
            movement *= RunSpeedMultiplier;
        }

        GetComponent<Rigidbody2D>().velocity = movement;
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
        moveVector = context.ReadValue<Vector2>();
        // Determine the direction based on the moveVector
        if (Mathf.Abs(moveVector.x) > Mathf.Abs(moveVector.y))
        {
            if (moveVector.x > 0)
            {
                if (isRunning)
                    PlayAnimation("RunRight", true);
                else
                    PlayAnimation("WalkRight", true);
                characterDirection = Vector2.right;
            }
            else
            {
                if (isRunning)
                    PlayAnimation("RunLeft", true);
                else
                    PlayAnimation("WalkLeft", true);
                characterDirection = Vector2.left;
            }
        }
        else
        {
            if (moveVector.y > 0)
            {
                if (isRunning)
                    PlayAnimation("RunUp", true);
                else
                    PlayAnimation("WalkUp", true);
                characterDirection = Vector2.up;
            }
            else
            {
                if (isRunning)
                    PlayAnimation("RunDown", true);
                else
                    PlayAnimation("WalkDown", true);
                characterDirection = Vector2.down;
            }
        }
        interactionCollider.offset = characterDirection * 0.25f;

    }
    private void OnMoveInputCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector2.zero;
        if (characterDirection == Vector2.up)
            PlayAnimation("IdleUp",false);
        if (characterDirection == Vector2.down)
            PlayAnimation("IdleDown", false);
        if (characterDirection == Vector2.right)
            PlayAnimation("IdleRight", false);
        if (characterDirection == Vector2.left)
            PlayAnimation("IdleLeft", false);
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
        if (inputActions.CharacterControls.Move.IsPressed())
        {
            if (characterDirection == Vector2.up)
                PlayAnimation("RunUp", true);
            if (characterDirection == Vector2.down)
                PlayAnimation("RunDown", true);
            if (characterDirection == Vector2.right)
                PlayAnimation("RunRight", true);
            if (characterDirection == Vector2.left)
                PlayAnimation("RunLeft", true);
        }
    }
    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        isRunning = false;
        if (inputActions.CharacterControls.Move.IsPressed())
        {
            if (characterDirection == Vector2.up)
                PlayAnimation("WalkUp", true);
            if (characterDirection == Vector2.down)
                PlayAnimation("WalkDown", true);
            if (characterDirection == Vector2.right)
                PlayAnimation("WalkRight", true);
            if (characterDirection == Vector2.left)
                PlayAnimation("WalkLeft", true);
        }
    }
    private void OnInteractInput(InputAction.CallbackContext context)
    {

    }
    #endregion
}