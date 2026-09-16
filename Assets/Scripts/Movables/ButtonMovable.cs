using UnityEngine;
using System.Collections;

namespace Movables
{
    public class ButtonMovable : MonoBehaviour
    {
        [SerializeField] private GameObject door; 

        private float _elapsedTime;
        private bool _isMoving;
        private bool _isDoorOpen;  
        public float doorOpenHeight = 3f; 
        public float doorClosedHeight; 

        public void ToggleDoor()
        {
            Debug.Log("Toggling door state.");

            if (_isDoorOpen&& !_isMoving)
            {
                // Close the door
                _elapsedTime = 0f;
                StartCoroutine(MoveDoor(doorClosedHeight));
                
                _isDoorOpen = !_isDoorOpen;
            }
            else if (!_isMoving)
            {
                // Open the door
                _elapsedTime = 0f;
                StartCoroutine(MoveDoor(doorOpenHeight));
               
                _isDoorOpen = !_isDoorOpen;
            }
        }

        private IEnumerator MoveDoor(float targetHeight)
        {
            _isMoving = true;
            float duration = 2f; 

            Vector3 startingPosition = door.transform.position;
            Vector3 targetPosition = new Vector3(door.transform.position.x, targetHeight, door.transform.position.z);

            while (_elapsedTime < duration)
            {
                door.transform.position = Vector3.Lerp(startingPosition, targetPosition, (_elapsedTime / duration));
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            door.transform.position = targetPosition; 
            _isMoving = false;
        }
    }
}
