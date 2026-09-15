using UnityEngine;
using UnityEngine.InputSystem;

public class CompanionCharacter : BasicCharacter
{
    [SerializeField]
    private InputActionAsset _inputAsset;
    [SerializeField]
    private InputActionReference _verticalMovementAction; // Reference for up/down movement input
    [SerializeField]
    private InputActionReference _horizontalMovementAction; // Reference for left/right movement input

    private bool _isControlEnabled = true;
    private float _verticalInput = 0f;   // Store the vertical input (W/S)
    private float _horizontalInput = 0f; // Store the horizontal input (A/D)

    [SerializeField]
    private float _flySpeed = 5f; // Speed for flying up/down

    protected override void Awake()
    {
        base.Awake();
        if (_inputAsset == null) return;

        // Subscribe to input actions
        _verticalMovementAction.action.performed += HandleVerticalMovementInput;
        _verticalMovementAction.action.canceled += HandleVerticalMovementInput;

        _horizontalMovementAction.action.performed += HandleHorizontalMovementInput;
        _horizontalMovementAction.action.canceled += HandleHorizontalMovementInput;
    }

    private void OnEnable()
    {
        if (_inputAsset == null) return;
        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;
        _inputAsset.Disable();
    }

    private void Update()
    {
        if (!_isControlEnabled) return;
        ApplyMovement(); // Apply combined vertical and horizontal movement each frame
    }

    private void HandleVerticalMovementInput(InputAction.CallbackContext context)
    {
        // Store the vertical input value for W/S
        _verticalInput = context.ReadValue<float>();
    }

    private void HandleHorizontalMovementInput(InputAction.CallbackContext context)
    {
        // Store the horizontal input value for A/D
        _horizontalInput = context.ReadValue<float>();
    }

    private void ApplyMovement()
    {
        if (_movementBehaviour == null) return;

        // Combine vertical and horizontal input for smooth movement
        Vector3 verticalMovement = _verticalInput * Vector3.up * _flySpeed * Time.deltaTime;
        Vector3 horizontalMovement = _horizontalInput * Vector3.right * _flySpeed * Time.deltaTime; // Use X-axis for A/D

        // Update position for flying along both axes
        transform.position += verticalMovement + horizontalMovement;
    }

    public override void EnableControl()
    {
        // Reset inputs when control is enabled
        _isControlEnabled = true;
        _verticalInput = 0f;   // Reset vertical input to prevent unintended upward movement
        _horizontalInput = 0f; // Reset horizontal input to prevent unintended movement
    }

    public override void DisableControl()
    {
        _isControlEnabled = false;
        _verticalInput = 0f;
        _horizontalInput = 0f;
    }

    protected void OnDestroy()
    {
        // Unsubscribe from input actions to prevent memory leaks
        _verticalMovementAction.action.performed -= HandleVerticalMovementInput;
        _verticalMovementAction.action.canceled -= HandleVerticalMovementInput;
        _horizontalMovementAction.action.performed -= HandleHorizontalMovementInput;
        _horizontalMovementAction.action.canceled -= HandleHorizontalMovementInput;
    }
}