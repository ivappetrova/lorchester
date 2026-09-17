using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{

    public class CompanionCharacter : BasicCharacter
    {
        [SerializeField] private InputActionAsset inputAsset;
        [SerializeField] private InputActionReference verticalMovementAction; // Reference for up/down movement input
        [SerializeField] private InputActionReference horizontalMovementAction; // Reference for left/right movement input

        private bool _isControlEnabled = true;
        private float _verticalInput;   // Store the vertical input (W/S)
        private float _horizontalInput; // Store the horizontal input (A/D)

        [SerializeField] private float flySpeed = 5f; // Speed for flying up/down

        [Header("Obstacle Avoidance")]
        [Tooltip("Layers this companion is physically blocked by while flying (walls, doors, etc). Do NOT include the Player/Friendly layers here.")]
        [SerializeField] private LayerMask obstacleMask = ~0;

        [Tooltip("Radius used for the obstacle sweep. Auto-synced from a SphereCollider on this object in Awake if present.")]
        [SerializeField] private float castRadius = 0.4f;

        [Tooltip("Small buffer kept between the companion and the surface it stops against.")]
        [SerializeField] private float skinWidth = 0.05f;

        private Collider _companionCollider;
        private Vector3 _colliderCenterLocal;

        protected override void Awake()
        {
            base.Awake();

            _companionCollider = GetComponent<Collider>();
            if (_companionCollider is SphereCollider sphere)
            {
                castRadius = sphere.radius * transform.lossyScale.x;
                _colliderCenterLocal = sphere.center;
            }

            if (inputAsset == null) return;

            // Subscribe to input actions
            verticalMovementAction.action.performed += HandleVerticalMovementInput;
            verticalMovementAction.action.canceled += HandleVerticalMovementInput;

            horizontalMovementAction.action.performed += HandleHorizontalMovementInput;
            horizontalMovementAction.action.canceled += HandleHorizontalMovementInput;
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
            if (MovementBehaviour == null) return;

            Vector3 centerOffset = transform.TransformVector(_colliderCenterLocal);
            Vector3 currentPosition = transform.position;

            // Resolve each axis separately so a wall blocking horizontal motion
            // doesn't also block unrelated vertical motion (and vice versa).
            Vector3 horizontalMovement = Vector3.right * (_horizontalInput * flySpeed * Time.deltaTime);
            currentPosition = MoveAxis(currentPosition, horizontalMovement, centerOffset);

            Vector3 verticalMovement = Vector3.up * (_verticalInput * flySpeed * Time.deltaTime);
            currentPosition = MoveAxis(currentPosition, verticalMovement, centerOffset);

            transform.position = currentPosition;
        }

        private Vector3 MoveAxis(Vector3 currentPosition, Vector3 moveVector, Vector3 centerOffset)
        {
            float moveDistance = moveVector.magnitude;
            if (moveDistance <= 0.0001f) return currentPosition;

            Vector3 moveDirection = moveVector / moveDistance;
            Vector3 castOrigin = currentPosition + centerOffset;
            Vector3 desiredPosition = currentPosition + moveVector;

            if (Physics.SphereCast(castOrigin, castRadius, moveDirection, out RaycastHit hit,
                    moveDistance, obstacleMask, QueryTriggerInteraction.Ignore))
            {
                float safeDistance = Mathf.Max(hit.distance - skinWidth, 0f);
                desiredPosition = currentPosition + moveDirection * safeDistance;
            }

            if (Physics.CheckSphere(desiredPosition + centerOffset, castRadius, obstacleMask, QueryTriggerInteraction.Ignore))
            {
                desiredPosition = currentPosition;
            }

            return desiredPosition;
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

        // CompanionCharacter
        protected void OnDestroy()
        {
            if (verticalMovementAction != null && verticalMovementAction.action != null)
            {
                verticalMovementAction.action.performed -= HandleVerticalMovementInput;
                verticalMovementAction.action.canceled -= HandleVerticalMovementInput;
            }

            if (horizontalMovementAction != null && horizontalMovementAction.action != null)
            {
                horizontalMovementAction.action.performed -= HandleHorizontalMovementInput;
                horizontalMovementAction.action.canceled -= HandleHorizontalMovementInput;
            }
        }
    }
}