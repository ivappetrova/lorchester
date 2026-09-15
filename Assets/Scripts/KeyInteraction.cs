using UnityEngine;

public class KeyInteraction : MonoBehaviour
{
    public Key currentKey; 
    void Update()
    {
        if (currentKey != null && !currentKey.isCollected)
        {
          //  Debug.Log("In range of the key.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
         
            currentKey = other.GetComponent<Key>();

            if (currentKey != null && !currentKey.isCollected)
            {
                currentKey.CollectKey(); 
                Debug.Log("Key collected!");
            }
            else
            {
               // Debug.LogWarning("Key is either already collected or not found.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        { 
            currentKey = null;
        }
    }
}
