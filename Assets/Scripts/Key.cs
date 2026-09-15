using UnityEngine;

public class Key : MonoBehaviour
{
    public Door targetDoor; 
    public bool isCollected = false;

    public void CollectKey()
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log("Key has been collected!");
            gameObject.SetActive(false); // Hide the key after collection

            if (targetDoor != null)
            {
                targetDoor.UnlockDoor();  // Unlock only the assigned door
            }
            else
            {
                Debug.LogWarning("No door assigned to this key!");
            }
        }
        else
        {
            Debug.Log("Key was already collected.");
        }
    }
}

