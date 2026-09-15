using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField]
    private InputActionAsset _inputAsset;
    [SerializeField]
    private InputActionReference _movementAction;
    private InputAction _jumpAction;
    private InputAction _shootAction;
    public InputAction _interactAction; 

    private bool _isControlEnabled = true;

    private Transform _weaponTransform;  
    private AttackBehaviour _playerAttackBehaviour;  
    private ButtonDoor _currentButtonDoor; 

    protected override void Awake()
    {
        base.Awake();

        _playerAttackBehaviour = GetComponent<AttackBehaviour>();
        if (_playerAttackBehaviour != null)
        {
            // Get the weapon's transform
            _weaponTransform = _playerAttackBehaviour.WeaponTransform; 
          
        }
      

        if (_inputAsset == null) return;

        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _shootAction = _inputAsset.FindActionMap("Gameplay").FindAction("Shoot");
        _interactAction = _inputAsset.FindActionMap("Gameplay").FindAction("Interact");

    

        _jumpAction.performed += HandleJumpInput;
        _interactAction.performed += HandleInteractInput;
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

    private void Start()
    {
       
    }

    private void Update()
    {
        if (!_isControlEnabled) return;

        HandleMovementInput(); 
        HandleAttackInput();   
        HandleAimingInput();   
    }

    private void HandleAimingInput()
    {
        if (_weaponTransform == null)
        {
          
            return;

        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z); // Correct Z distance from camera to player

        // Convert the mouse position to world space
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0; // Keep Z coordinate at 0, assuming game takes place in the XY plane

        // Calculate the direction from the player to the mouse position in world space
        Vector3 directionToMouse = (worldMousePosition - transform.position).normalized;

        // Use this direction to set the desired look-at point for the weapon (or the player's shoulder)
        _movementBehaviour.DesiredLookatPoint = worldMousePosition;

        // Update weapon aiming (this assumes _weaponTransform is set via AttackBehaviour)
        _weaponTransform.up = directionToMouse; // or _weaponTransform.forward depending on your weapon's local rotation
    }

    private void HandleMovementInput()
    {
        if (_movementBehaviour == null || _movementAction == null) return;

        float movementInput = _movementAction.action.ReadValue<float>();
        Vector3 movement = movementInput * Vector3.right;
        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (_isControlEnabled && _movementBehaviour != null)
        {
            _movementBehaviour.Jump();
        }
    }

    private void HandleAttackInput()
    {
        if (_playerAttackBehaviour == null || _shootAction == null) return;
        if (_isControlEnabled && _shootAction.IsPressed())
            _playerAttackBehaviour.Attack();
    }

    
    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        if (_currentButtonDoor != null)
        {
            _currentButtonDoor.ToggleDoor(); 
        }
    }

    public void SetCurrentButtonDoor(ButtonDoor buttonDoor)
    {
        _currentButtonDoor = buttonDoor;
    }

    public override void EnableControl()
    {
        _isControlEnabled = true;
    }

    public override void DisableControl()
    {
        _isControlEnabled = false;
        _movementBehaviour.DesiredMovementDirection = Vector3.zero;
    }

    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
        _interactAction.performed -= HandleInteractInput;
    }
}
