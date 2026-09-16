using UnityEngine;

namespace Player
{
    public class MovementBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject shoulderObject;
        private Vector3 _desiredLookAtPoint = Vector3.zero;


        [SerializeField] private float movementSpeed = 1.0f;
        [SerializeField] private float jumpStrength = 10.0f;
        private Rigidbody _rigidBody;
        private Vector3 _desiredMovementDirection = Vector3.zero;
        private bool _isGrounded;
        private const float GroundCheckDistance = 0.2f;
        public Vector3 DesiredMovementDirection
        {
            get => _desiredMovementDirection;
            set => _desiredMovementDirection = value;
        }

        public Vector3 DesiredLookAtPoint
        {
            get => _desiredLookAtPoint;
            set => _desiredLookAtPoint = value;
        }


        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }


        private void FixedUpdate()
        {
            HandleMovement();
            //check if there is ground beneath our feet
            _isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GroundCheckDistance, LayerMask.GetMask("Ground"));
               
        }

        private void HandleMovement()
        {
            if (_rigidBody == null) return;
            Vector3 movement = _desiredMovementDirection.normalized;
            movement *= movementSpeed;
            movement.y = _rigidBody.linearVelocity.y;
            _rigidBody.linearVelocity = movement;
        }

        private void Update()
        {
            HandleLookAt();
        }
        private void HandleLookAt()
        {
            if (shoulderObject == null) return;
            shoulderObject.transform.LookAt(_desiredLookAtPoint);
        }

        public void Jump()
        {
            if (_isGrounded)
                _rigidBody.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }
}
