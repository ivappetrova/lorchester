using UnityEngine;

public class Door : MonoBehaviour
{
    public float moveHeight = 10f;
    public float moveSpeed = 2f;
    private bool isUnlocked = false;
    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Store the initial position (closed position)
        closedPosition = transform.position;
        // Define the open position
        openPosition = closedPosition + Vector3.up * moveHeight;
    }

    void Update()
    {
        // If unlocked and open, move the door upwards
        if (isUnlocked && !isOpen && transform.position.y < openPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, moveSpeed * Time.deltaTime);
        }
       
    }

    virtual public void UnlockDoor()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log("Unlocking the door!");
        }
    }
}

