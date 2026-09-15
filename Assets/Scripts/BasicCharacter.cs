using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    protected AttackBehaviour _attackBehaviour;
    protected MovementBehaviour _movementBehaviour;

    protected bool isControlEnabled = true;

    protected virtual void Awake()
    {
        _attackBehaviour = GetComponent<AttackBehaviour>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
        
    }

    public virtual void EnableControl()
    {
        isControlEnabled = true;
    }

    public virtual void DisableControl()
    {
        isControlEnabled = false;
        if (_movementBehaviour != null)
        {
            // Stop movement when control is disabled
            _movementBehaviour.DesiredMovementDirection = Vector3.zero;
        }
 
    }
}