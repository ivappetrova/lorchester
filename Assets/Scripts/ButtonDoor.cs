using UnityEngine;
using System.Collections;

public class ButtonDoor : MonoBehaviour
{
    [SerializeField] private GameObject door; 

    private bool isMoving = false;
    private float elapsedTime = 0f;
    private bool isDoorOpen = false;  
    public float doorOpenHeight = 3f; 
    public float doorClosedHeight = 0f; 

    public void ToggleDoor()
    {
        Debug.Log("Toggling door state.");

        if (isDoorOpen&& !isMoving)
        {
            // Close the door
            elapsedTime = 0f;
            StartCoroutine(MoveDoor(doorClosedHeight));
            
            isDoorOpen = !isDoorOpen;
        }
        else if (!isMoving)
        {
            // Open the door
            elapsedTime = 0f;
            StartCoroutine(MoveDoor(doorOpenHeight));
           
            isDoorOpen = !isDoorOpen;
        }
    }

    private IEnumerator MoveDoor(float targetHeight)
    {
        isMoving = true;
        float duration = 2f; 

        Vector3 startingPosition = door.transform.position;
        Vector3 targetPosition = new Vector3(door.transform.position.x, targetHeight, door.transform.position.z);

        while (elapsedTime < duration)
        {
            door.transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = targetPosition; 
        isMoving = false;
    }
}
