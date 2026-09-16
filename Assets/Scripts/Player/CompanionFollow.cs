using UnityEngine;

namespace Player
{
    public class CompanionFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float followDistance = 2.0f;
        [SerializeField] private float verticalOffset = 1.0f;
        [SerializeField] private float followSpeed = 5.0f;

        [Tooltip("How fast the companion swings around to the new 'behind' direction.")]
        [SerializeField] private float directionTurnSpeed = 6.0f;

        [Tooltip("Smoothed player speed (m/s) required before the companion re-aims. Set this well above your idle jitter but below walk speed.")]
        [SerializeField] private float movementThreshold = 0.75f;

        [Tooltip("Higher = velocity reacts faster but is noisier. 8-12 works for most controllers.")]
        [SerializeField] private float velocitySmoothing = 10.0f;

        [Header("Initial Facing")]
        [Tooltip("Fallback 'behind' direction used only before any real movement has happened to compute one (i.e. the very first frame). The player moves along X only (see PlayerCharacter.HandleMovementInput), so this should be Vector3.left or Vector3.right, never a Z-axis value.")]
        [SerializeField] private Vector3 initialBehindDirection = Vector3.left;

        [Header("Obstacle Avoidance")]
        [Tooltip("Layers the companion should be physically blocked by (walls, doors, etc). Do NOT include the Player/Friendly layers here.")]
        [SerializeField] private LayerMask obstacleMask = ~0;

        [Tooltip("Radius used for the obstacle sweep. Auto-synced from the companion's SphereCollider in Awake if present.")]
        [SerializeField] private float castRadius = 0.4f;

        [Tooltip("Small buffer kept between the companion and the surface it stops against.")]
        [SerializeField] private float skinWidth = 0.05f;

        [SerializeField] private bool debugDrawDirection;

        private bool _isFollowing = true;
        private Collider _companionCollider;
        private Vector3 _colliderCenterLocal;

        private Vector3 _lastPlayerPosition;
        private Vector3 _smoothedVelocity;

        // Seeded from initialBehindDirection in Awake (not a field initializer), so the
        // Inspector value is actually respected. Kept separate from the tunable default so
        // "what the user configured" is never conflated with "what the companion is
        // currently doing".
        private Vector3 _behindDirection;

        private void Awake()
        {
            _companionCollider = GetComponent<Collider>();

            if (_companionCollider is SphereCollider sphere)
            {
                castRadius = sphere.radius * transform.lossyScale.x;
                _colliderCenterLocal = sphere.center;
            }

            _behindDirection = initialBehindDirection.sqrMagnitude > 0.0001f
                ? initialBehindDirection.normalized
                : Vector3.left;

            // _behindDirection keeps this seeded value until real gameplay movement in
            // Update() recalculates it. Do not reset it from mode-switch methods below,
            // or you throw away a perfectly good direction the moment the player takes
            // back control.
            ResetVelocityTracking();
        }

        private void Start()
        {
            if (_isFollowing)
            {
                SetFollowMode();
            }
        }

        private void Update()
        {
            if (!_isFollowing || player == null)
                return;

            float dt = Mathf.Max(Time.deltaTime, 0.0001f);

            Vector3 delta = player.position - _lastPlayerPosition;
            _lastPlayerPosition = player.position;
            delta.y = 0f;
            Vector3 rawVelocity = delta / dt;

            _smoothedVelocity = Vector3.Lerp(_smoothedVelocity, rawVelocity, 1f - Mathf.Exp(-velocitySmoothing * dt));

            if (_smoothedVelocity.magnitude > movementThreshold)
            {
                Vector3 desired = -_smoothedVelocity.normalized;

                if (Vector3.Dot(desired, _behindDirection) < -0.999f)
                {
                    _behindDirection = Quaternion.Euler(0f, 1f, 0f) * _behindDirection;
                }

                _behindDirection = Vector3.Slerp(_behindDirection, desired, directionTurnSpeed * dt).normalized;
            }

            Vector3 targetPosition = player.position + _behindDirection * followDistance;
            targetPosition.y = player.position.y + verticalOffset;

            MoveTowards(targetPosition, dt);

            Vector3 lookDirection = -_behindDirection;
            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            }

            if (debugDrawDirection)
            {
                Debug.DrawRay(player.position, _behindDirection * followDistance, Color.red);
                Debug.DrawRay(player.position, _smoothedVelocity, Color.green);
            }
        }

        private void MoveTowards(Vector3 targetPosition, float dt)
        {
            Vector3 currentPosition = transform.position;
            Vector3 desiredPosition = Vector3.Lerp(currentPosition, targetPosition, followSpeed * dt);

            Vector3 moveVector = desiredPosition - currentPosition;
            float moveDistance = moveVector.magnitude;

            Vector3 centerOffset = transform.TransformVector(_colliderCenterLocal);

            if (moveDistance > 0.0001f)
            {
                Vector3 moveDirection = moveVector / moveDistance;
                Vector3 castOrigin = currentPosition + centerOffset;

                if (Physics.SphereCast(castOrigin, castRadius, moveDirection, out RaycastHit hit,
                        moveDistance, obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    float safeDistance = Mathf.Max(hit.distance - skinWidth, 0f);
                    desiredPosition = currentPosition + moveDirection * safeDistance;
                }
            }

            if (Physics.CheckSphere(desiredPosition + centerOffset, castRadius, obstacleMask, QueryTriggerInteraction.Ignore))
            {
                desiredPosition = currentPosition;
            }

            transform.position = desiredPosition;
        }

        private void ResetVelocityTracking()
        {
            if (player == null)
                return;

            _lastPlayerPosition = player.position;
            _smoothedVelocity = Vector3.zero;
        }

        public void SetFollowMode()
        {
            _isFollowing = true;
            ResetVelocityTracking();
            SnapToFollowPosition();
            IgnorePlayerCollision();
        }

        public void SetFlyingMode()
        {
            _isFollowing = false;
            IgnorePlayerCollision();
        }

        public void SetPlayer(Transform newPlayer)
        {
            player = newPlayer;
            ResetVelocityTracking();
            SnapToFollowPosition();
            IgnorePlayerCollision();
        }

        private void SnapToFollowPosition()
        {
            if (player == null) return;

            Vector3 targetPosition = player.position + _behindDirection * followDistance;
            targetPosition.y = player.position.y + verticalOffset;
            transform.position = targetPosition;
        }

        private void IgnorePlayerCollision()
        {
            if (player == null || _companionCollider == null)
                return;

            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(_companionCollider, playerCollider, true);
            }
        }
    }
}