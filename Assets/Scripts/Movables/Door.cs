using UnityEngine;

namespace Movables
{
    public class Door : MonoBehaviour
    {
        public float moveHeight = 10f;
        public float moveSpeed = 2f;
        private bool _isUnlocked;
        private bool _isOpen;
        private Vector3 _closedPosition;
        private Vector3 _openPosition;
        
        void Start()
        {
            // Store the initial position (closed position)
            _closedPosition = transform.position;
            // Define the open position
            _openPosition = _closedPosition + Vector3.up * moveHeight;
        }

        void Update()
        {
            // If unlocked and open, move the door upwards
            if (_isUnlocked && !_isOpen && transform.position.y < _openPosition.y)
            {
                transform.position = Vector3.MoveTowards(transform.position, _openPosition, moveSpeed * Time.deltaTime);
            }
           
        }

        public virtual void UnlockDoor()
        {
            if (!_isUnlocked)
            {
                _isUnlocked = true;
                Debug.Log("Unlocking the door!");
            }
        }
    }
}