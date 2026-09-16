using Movables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCharacter : BasicCharacter
    {
        [SerializeField] private InputActionAsset inputAsset;
        [SerializeField] private InputActionReference movementAction;
        private InputAction _jumpAction;
        private InputAction _shootAction;
        public InputAction interactAction; 

        private bool _isControlEnabled = true;

        private Transform _weaponTransform;  
        private AttackBehaviour _playerAttackBehaviour;  
        private ButtonMovable _currentButtonMovable;
        private Camera _camera;

        protected override void Awake()
        {
            _camera = Camera.main;
            base.Awake();

            _playerAttackBehaviour = GetComponent<AttackBehaviour>();
            if (_playerAttackBehaviour != null)
            {
                // Get the weapon's transform
                _weaponTransform = _playerAttackBehaviour.WeaponTransform; 
              
            }
          

            if (inputAsset == null) return;

            _jumpAction = inputAsset.FindActionMap("Gameplay").FindAction("Jump");
            _shootAction = inputAsset.FindActionMap("Gameplay").FindAction("Shoot");
            interactAction = inputAsset.FindActionMap("Gameplay").FindAction("Interact");

        

            _jumpAction.performed += HandleJumpInput;
            interactAction.performed += HandleInteractInput;
        }

        private void OnEnable()
        {
            if (inputAsset == null) return;
            inputAsset.Enable();
        }

        private void OnDisable()
        {
            if (inputAsset == null) return;
            inputAsset.Disable();
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
            if (_weaponTransform == null || _camera == null)
            {
                return;
            }


            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(transform.position.z - _camera.transform.position.z); // Correct Z distance from camera to player

            // Convert the mouse position to world space
            Vector3 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition);
            worldMousePosition.z = 0; // Keep Z coordinate at 0, assuming game takes place in the XY plane

            // Calculate the direction from the player to the mouse position in world space
            Vector3 directionToMouse = (worldMousePosition - transform.position).normalized;

            // Use this direction to set the desired look-at point for the weapon (or the player's shoulder)
            MovementBehaviour.DesiredLookAtPoint = worldMousePosition;

            // Update weapon aiming (this assumes _weaponTransform is set via AttackBehaviour)
            _weaponTransform.up = directionToMouse; // or _weaponTransform.forward depending on your weapon's local rotation
        }

        private void HandleMovementInput()
        {
            if (MovementBehaviour == null || movementAction == null) return;

            float movementInput = movementAction.action.ReadValue<float>();
            Vector3 movement = movementInput * Vector3.right;
            MovementBehaviour.DesiredMovementDirection = movement;
        }

        private void HandleJumpInput(InputAction.CallbackContext context)
        {
            if (_isControlEnabled && MovementBehaviour != null)
            {
                MovementBehaviour.Jump();
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
            if (_currentButtonMovable != null)
            {
                _currentButtonMovable.ToggleDoor(); 
            }
        }

        public void SetCurrentButtonMovable(ButtonMovable buttonMovable)
        {
            _currentButtonMovable = buttonMovable;
        }

        public override void EnableControl()
        {
            _isControlEnabled = true;
        }

        public override void DisableControl()
        {
            _isControlEnabled = false;
            MovementBehaviour.DesiredMovementDirection = Vector3.zero;
        }
        
        protected void OnDestroy()
        {
            if (_jumpAction != null) _jumpAction.performed -= HandleJumpInput;
            if (interactAction != null) interactAction.performed -= HandleInteractInput;
        }
    }
}