using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    [SerializeField] private Transform player;           
    [SerializeField] private float followDistance = 2.0f;
    [SerializeField] private float verticalOffset = 1.0f;
    [SerializeField] private float followSpeed = 5.0f;   
    [SerializeField] private LayerMask collisionLayer;   

    private bool isFollowing = true;                     
    private Rigidbody rb;
    private Collider companionCollider;

 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        companionCollider = GetComponent<Collider>();

        // Freeze rotation to prevent unwanted rotation
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            // Follow the player with offset (horizontal position and height offset)
            Vector3 targetPosition = player.position + player.right * followDistance;

            // Maintain vertical offset
            targetPosition.y = player.position.y + verticalOffset / 2; 

            // Smoothly move the companion towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Keep the companion upright (match the player's Y rotation)
            transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

           
        }
    }
 
    public void SetFollowMode()
    {
        isFollowing = true;

        if (player != null && companionCollider != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                // Ignore collisions with the player
                Physics.IgnoreCollision(companionCollider, playerCollider, true);
              
            }
            //companionCollider.enabled = false;
        }
    }

    public void SetFlyingMode()
    {
        isFollowing = false;
        
        if (player != null && companionCollider != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                // Ignore collisions with the player
                Physics.IgnoreCollision(companionCollider, playerCollider, true);
            }
            //companionCollider.enabled = true;
        }
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;

        if (player != null && companionCollider != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                // Ignore collisions with the player
                Physics.IgnoreCollision(companionCollider, playerCollider, true);
            }
        }
    }
}
