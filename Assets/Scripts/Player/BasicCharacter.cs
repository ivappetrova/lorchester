using UnityEngine;

namespace Player
{
    public class BasicCharacter : MonoBehaviour
    {
        protected AttackBehaviour AttackBehaviour;
        protected MovementBehaviour MovementBehaviour;

        protected bool IsControlEnabled = true;

        protected virtual void Awake()
        {
            AttackBehaviour = GetComponent<AttackBehaviour>();
            MovementBehaviour = GetComponent<MovementBehaviour>();
            
        }

        public virtual void EnableControl()
        {
            IsControlEnabled = true;
        }

        public virtual void DisableControl()
        {
            IsControlEnabled = false;
            if (MovementBehaviour != null)
            {
                // Stop movement when control is disabled
                MovementBehaviour.DesiredMovementDirection = Vector3.zero;
            }
     
        }
    }
}