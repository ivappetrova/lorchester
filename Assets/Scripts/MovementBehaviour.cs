using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _shoulderObject = null;
    private Vector3 _desiredLookatPoint = Vector3.zero;


    [SerializeField]
    private float _movementSpeed = 1.0f;
    [SerializeField]
    private float _jumpStrength = 10.0f;
    private Rigidbody _rigidBody;
    private Vector3 _desiredMovementDirection = Vector3.zero;
    private bool _grounded = false;
    private const float GROUND_CHECK_DISTANCE = 0.2f;
    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    public Vector3 DesiredLookatPoint
    {
        get { return _desiredLookatPoint; }
        set { _desiredLookatPoint = value; }
    }


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
        //check if there is ground beneath our feet
        _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask("Ground"));
           
    }

    private void HandleMovement()
    {
        if (_rigidBody == null) return;
        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;
        movement.y = _rigidBody.velocity.y;
        _rigidBody.velocity = movement;
    }

    private void Update()
    {
        HandleLookat();
    }
    private void HandleLookat()
    {
        if (_shoulderObject == null) return;
        _shoulderObject.transform.LookAt(_desiredLookatPoint);
    }

    public void Jump()
    {
        if (_grounded)
            _rigidBody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }
}
