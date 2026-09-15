using UnityEngine;
using UnityEngine.InputSystem;  

public class ButtonInteraction : MonoBehaviour
{
    public ButtonDoor[] currentDoors;  
    private bool isPlayerInRange = false;  
    void Update()
    {
        if (isPlayerInRange)
        {
            if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                for (int i = 0; i < currentDoors.Length; i++)
                {
                    if (currentDoors[i] != null)
                    {
                        currentDoors[i].ToggleDoor(); 
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

            ButtonDoor[] buttonDoor = gameObject.GetComponents<ButtonDoor>();
           
                if (buttonDoor != null)
                {
                    currentDoors = buttonDoor;
                   
                    isPlayerInRange = true;
                }        
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited button's trigger zone! BUTTON INTERACTION");
            isPlayerInRange = false;

            currentDoors = null;
    
        }
    }
}
