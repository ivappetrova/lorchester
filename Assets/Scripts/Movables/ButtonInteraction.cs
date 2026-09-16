using UnityEngine;
using UnityEngine.InputSystem;

namespace Movables
{
    public class ButtonInteraction : MonoBehaviour
    {
        public ButtonMovable[] currentDoors;  
        private bool _isPlayerInRange;  
        void Update()
        {
            if (_isPlayerInRange)
            {
                if (Keyboard.current.eKey.wasReleasedThisFrame)
                {
                    foreach (var t in currentDoors)
                    {
                        if (t != null)
                        {
                            t.ToggleDoor(); 
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))  
            {
                Debug.Log("Player entered button's trigger zone BUTTON INTERACTION!");

                ButtonMovable[] buttonMovable = gameObject.GetComponents<ButtonMovable>();
               
                    if (buttonMovable != null)
                    {
                        currentDoors = buttonMovable;
                       
                        _isPlayerInRange = true;
                    }        
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player exited button's trigger zone! BUTTON INTERACTION");
                _isPlayerInRange = false;

                currentDoors = null;
        
            }
        }
    }
}